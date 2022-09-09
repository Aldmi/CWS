using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using App.Services.InputData;
using AutoMapper;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using WebApiSwc.DTO.JSON.InputTypesDto;
using WebApiSwc.DTO.JSON.ResponseWebApiTypesDto;
using WebApiSwc.DTO.XML;

namespace WebApiSwc.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class InputDataController : Controller
    {
        #region fields
        private readonly InputDataApplyService<AdInputType> _inputDataApplyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion



        #region ctor
        public InputDataController(IConfiguration config,
                                   InputDataApplyService<AdInputType> inputDataApplyService,
                                   IMapper mapper,
                                   ILogger logger)
        {
            _inputDataApplyService = inputDataApplyService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion



        #region Api
        [HttpPost("SendData4Devices")]
        public async Task<IActionResult> SendData4Devices([FromBody] IReadOnlyList<InputData<AdInputTypeDto>> inputDatasDto)
        {
            try
            {
                var inputDatas = _mapper.Map<IReadOnlyList<InputData<AdInputType>>>(inputDatasDto);
                var res = await InputDataHandler(inputDatas);
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{Type}", "Ошибка в InputDataController/SendData4Devices");
                throw;
            }
        }


        /// <summary>
        /// Отправить данные на 1 Device.
        /// Идентификационные данные задаются в Header запроса
        /// Формат POST запроса application/xml
        /// </summary>
        /// <param name="adInputType4XmlList"></param>
        /// <param name="deviceName"></param>
        /// <param name="exchangeName"></param>
        /// <param name="directHandlerName"></param>
        /// <param name="dataAction"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("SendDataXml4Devices")]
        [Produces("application/xml")]
        public async Task<IActionResult> SendDataXml4Devices([FromBody] AdInputType4XmlDtoContainer adInputType4XmlList,
                                                             [FromHeader] string deviceName,
                                                             [FromHeader] string exchangeName,
                                                             [FromHeader] string directHandlerName,
                                                             [FromHeader] string dataAction,
                                                             [FromHeader] string command)
        {
            try
            {
                if (string.IsNullOrEmpty(deviceName))
                {
                    ModelState.AddModelError("SendDataXml4Devices", "deviceName == null");
                    return BadRequest(ModelState);
                }
                if (!Enum.TryParse(dataAction, out DataAction dataActionParsed))
                {
                    ModelState.AddModelError("SendDataXml4Devices", "DataAction Error Parse");
                    return BadRequest(ModelState);
                }
                if (!Enum.TryParse(command, out Command4Device commandParse))
                {
                    ModelState.AddModelError("SendDataXml4Devices", "Command4Device Error Parse");
                    return BadRequest(ModelState);
                }
                var data = _mapper.Map<List<AdInputType>>(adInputType4XmlList.Trains);
                var inputData = new InputData<AdInputType>
                {
                    DeviceName = deviceName,
                    ExchangeName = exchangeName,
                    DirectHandlerName = directHandlerName,
                    DataAction = dataActionParsed,
                    Command = commandParse,
                    Data = data
                };

                var res = await InputDataHandler(new List<InputData<AdInputType>> { inputData });
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{Type}", "InputDataController/SendDataXml4Devices");
                throw;
            }
        }


        /// <summary>
        /// Отправить данные на 1 Device.
        /// Формат POST запроса Multipart.
        /// XML передается как файл с именем username в FromForm виде.
        /// </summary>
        /// <param name="userfile">Имя фала</param>
        /// <param name="deviceName"></param>
        /// <param name="exchangeName"></param>
        /// <param name="directHandlerName"></param>
        /// <param name="dataAction"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("SendDataXmlMultipart4Devices")]
        public async Task<IActionResult> SendDataXmlMultipart4Devices([FromForm] IFormFile userfile,
                                                                      [FromHeader] string deviceName,
                                                                      [FromHeader] string exchangeName = "",
                                                                      [FromHeader] string directHandlerName = "",
                                                                      [FromHeader] string dataAction = "",
                                                                      [FromHeader] string command = "None")
        {
            //TODO: убрать выбор способа передачи имен. Сделать 2 метода контроллера с разными способами.
            var values = userfile.FileName.Split('+');
            if (values.Length == 2)
            {
                var fileName = values[0];
                dataAction = values[1];
                deviceName ??= fileName;//Если deviceName не переданн в заголовке 
                var str = $"FileName= {fileName}";
                _logger.Information("{Type} {MessageShort}", "InputDataController/SendDataXmlMultipart4Devices", str);
            }
            var (_, isFailureHeader, (actionParse, commandParse), errorHeader) = AcceptHeaders(deviceName, dataAction, command);
            if (isFailureHeader)
            {
                _logger.Warning("{Type} {MessageShort}", "InputDataController/SendDataXmlMultipart4Devices", errorHeader);
                return BadRequest($"SendDataXmlMultipart4Devices. {errorHeader}");
            }
            var (_, isFailureXml, xmlDto, erroXml) = await AcceptXmlFile<AdInputType4XmlDtoContainer>(userfile);
            if (isFailureXml)
            {
                _logger.Warning("{Type} {MessageShort}", "InputDataController/SendDataXmlMultipart4Devices", erroXml);
                return BadRequest($"SendDataXmlMultipart4Devices. {erroXml}");
            }
            try
            {
                var trains = xmlDto.Trains;
                InitDataId(trains);
                var data = _mapper.Map<List<AdInputType>>(trains);
                var inputData = new InputData<AdInputType>
                {
                    DeviceName = deviceName,
                    ExchangeName = exchangeName,
                    DirectHandlerName = directHandlerName,
                    DataAction = actionParse,
                    Command = commandParse,
                    Data = data
                };
                var res = await InputDataHandler(new List<InputData<AdInputType>> { inputData });
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error(ex ,"{Type}", "Ошибка Mapping");
                throw;
            }
        }


        /// <summary>
        /// Отправить бегущую строку на 1 Device.
        /// Формат POST запроса Multipart.
        /// XML передается как файл с именем username в FromForm виде.
        /// </summary>
        /// <param name="userfile">Имя фала</param>
        /// <param name="deviceName"></param>
        /// <param name="exchangeName"></param>
        /// <param name="directHandlerName"></param>
        /// <param name="dataAction"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("SendCreepLineXmlMultipart4Devices")]
        public async Task<IActionResult> SendCreepLineXmlMultipart4Devices([FromForm] IFormFile userfile,
                                                                      [FromHeader] string deviceName,
                                                                      [FromHeader] string exchangeName = "",
                                                                      [FromHeader] string directHandlerName = "",
                                                                      [FromHeader] string dataAction = "",
                                                                      [FromHeader] string command = "None")
        {
            //TODO: убрать выбор способа передачи имен. Сделать 2 метода контроллера с разными способами.
            var values = userfile.FileName.Split('+');
            if (values.Length == 2)
            {
                var fileName = values[0];
                dataAction = values[1];
                deviceName ??= fileName;//Если deviceName не передан в заголовке 
                var str = $"FileName= {fileName}";
                _logger.Information("{Type} {MessageShort}", "InputDataController/SendCreepLineXmlMultipart4Devices", str);
            }
            var (_, isFailureHeader, (actionParse, commandParse), errorHeader) = AcceptHeaders(deviceName, dataAction, command);
            if (isFailureHeader)
            {
                _logger.Warning("{Type} {MessageShort}", "InputDataController/SendCreepLineXmlMultipart4Devices", errorHeader);
                return BadRequest($"SendCreepLineXmlMultipart4Devices. {errorHeader}");
            }
            var (_, isFailureXml, xmlDto, erroXml) = await AcceptXmlFile<CreepingLine4XmlDto>(userfile);
            if (isFailureXml)
            {
                _logger.Warning("{Type} {MessageShort}", "InputDataController/SendCreepLineXmlMultipart4Devices", erroXml);
                return BadRequest($"SendCreepLineXmlMultipart4Devices. {erroXml}");
            }
            try
            {
                var trains = xmlDto;
               // InitDataId(trains); //TODO: выставить Id
                var data = _mapper.Map<AdInputType>(trains);
                var inputData = new InputData<AdInputType>
                {
                    DeviceName = deviceName,
                    ExchangeName = exchangeName,
                    DirectHandlerName = directHandlerName,
                    DataAction = actionParse,
                    Command = commandParse,
                    Data = new List<AdInputType>{data}
                };
                var res = await InputDataHandler(new List<InputData<AdInputType>> { inputData });
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{Type}", "Ошибка Mapping");
                throw;
            }
        }



        private async Task<Result<TXmlDto>> AcceptXmlFile<TXmlDto>(IFormFile xmlFile)
        {
            if (xmlFile == null)
            {
                return Result.Failure<TXmlDto>("xmlFile == null");
            }
            if (xmlFile.Length == 0)
            {
                return Result.Failure<TXmlDto>("Размер XML файла равен 0");
            }
            try
            {
                await using var memoryStream = new MemoryStream();
                await xmlFile.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                const string encoding = "utf-8";

                #region Debug
                //System.IO.File.WriteAllBytes(@"D:\\InDataXml_NewAd.xml", memoryStream.ToArray());
                var xmlContent = Encoding.GetEncoding(encoding).GetString(memoryStream.ToArray()); //utf-8   "Windows-1251"
                _logger.Debug($"{xmlContent}");
                #endregion

                using var reader = new StreamReader(memoryStream, Encoding.GetEncoding(encoding), true);
                var serializer = new XmlSerializer(typeof(TXmlDto));
                var dto = (TXmlDto)serializer.Deserialize(reader);
                return Result.Ok(dto);
            }
            catch (Exception ex)
            {
                return Result.Failure<TXmlDto>($"Ошибка преобразования XML файла к Dto объекту.  Exception= '{ex}'");
            }
        }


        private Result<(DataAction actionParse, Command4Device commandParse)> AcceptHeaders(string deviceName, string dataAction, string command)
        {
            if (string.IsNullOrEmpty(deviceName))
            {
                return Result.Failure<(DataAction, Command4Device)>("DataAction Error Parse");
            }
            if (!Enum.TryParse(dataAction, out DataAction dataActionParsed))
            {
                return Result.Failure<(DataAction, Command4Device)>("deviceName == null");
            }
            if (!Enum.TryParse(command, out Command4Device commandParse))
            {
                return Result.Failure<(DataAction, Command4Device)>("Command4Device Error Parse");
            }
            return Result.Ok((dataActionParsed, commandParse));
        }

        #endregion



        #region Methods

        /// <summary>
        /// Присвоить Id, если он не установлен
        /// </summary>
        private static void InitDataId(IList<AdInputType4XmlDto> datas)
        {
            for (var i = 0; i < datas.Count; i++)
            {
                var data = datas[i];
                if (data.Id == 0)
                    data.Id = i + 1;
            }
        }


        /// <summary>
        /// Возвращает СЛОВАРЬ состояний обменов для каждого устройства. (У каждого уст-тва список сотояний обменов)
        ///ФОРМАТ ОТВЕТА
        ///{
        ///    "DeviceName": [
        ///      {
        ///        "keyExchange": "ExchnageName",
        ///        "isConnect": true,
        ///        "isOpen": true
        ///      }
        ///    ]
        ///}
        /// </summary>
        /// <param name="inputDatas"></param>
        /// <returns></returns>
        private async Task<ActionResult> InputDataHandler(IReadOnlyList<InputData<AdInputType>> inputDatas)
        {
            var (_, isFailure, value, error) = await _inputDataApplyService.ApplyInputData(inputDatas);
            if(isFailure)
                return Ok(new IndigoResponseDto(0, error)); //Запрос верный, НО при обработве данных возникли ошибки

            var respThisDevice = value.Values.First();                                                         
            var notConnectExch = respThisDevice.Where(model => !model.IsConnect).ToList();   //Объединяет по И IsConnect от всех обменов этого устройства.
            var flag = notConnectExch.Any() ? 0 : 1;
            var message = flag == 1 ? "Ok" : "Not connect";
            var resp =  new IndigoResponseDto(flag,  message);                  
            return Ok(resp);                                    //Запрос верный, Обработано успешно
        }

        #endregion
    }
}