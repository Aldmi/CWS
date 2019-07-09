using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Autofac.Features.Indexed;
using AutoMapper;
using BL.Services.InputData;
using InputDataModel.Autodictor.Model;
using InputDataModel.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using WebApiSwc.DTO.XML;
using Worker.Background.Abstarct;

namespace WebApiSwc.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class InputDataController : Controller
    {
        #region fields

        private readonly ISimpleBackground _backgroundMessageBrokConsum;
        private readonly InputDataApplyService<AdInputType> _inputDataApplyService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        #endregion




        #region ctor

        public InputDataController(IConfiguration config,
                                   IIndex<string, ISimpleBackground> background,
                                   InputDataApplyService<AdInputType> inputDataApplyService,
                                   IMapper mapper,
                                   ILogger logger)
                                  
        {
            _inputDataApplyService = inputDataApplyService;
            _mapper = mapper;
            _logger = logger;
            var backgroundName= config["MessageBrokerConsumer4InData:Name"]; //TODO: работу с _backgroundMessageBrokConsum вынести в отдельный сервис
            _backgroundMessageBrokConsum = background[backgroundName];
        }

        #endregion




        #region Api

        // GET api/InputData/GetMessageBrokerConsumerBgState
        [HttpGet("GetMessageBrokerConsumerBgState")]
        public async Task<IActionResult> GetMessageBrokerConsumerBgState()
        {          
            var bgState = _backgroundMessageBrokConsum.IsStarted ? "Started" : "Stoped";
            await Task.CompletedTask;
            return new JsonResult(bgState);
        }


        /// <summary>
        /// Запустить бекграунд слушателя выходных сообшений от messageBroker
        /// </summary>
        // GET api/InputData/StartMessageBrokerConsumerBg
        [HttpPut("StartMessageBrokerConsumerBg")]
        public async Task<IActionResult> StartMessageBrokerConsumerBg()
        {
            try
            {
                if (_backgroundMessageBrokConsum.IsStarted)
                {
                    ModelState.AddModelError("StartMessageBrokerConsumerBg", "Listener already started !!!");
                    return BadRequest(ModelState);
                }

                await _backgroundMessageBrokConsum.StartAsync(CancellationToken.None);
                return Ok("Listener starting");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в InputDataController/StartListener");
                throw;
            }
        }


        /// <summary>
        /// Запустить бекграунд слушателя выходных сообшений от messageBroker
        /// </summary>
        [HttpPut("StopMessageBrokerConsumerBg")]
        public async Task<IActionResult> StopMessageBrokerConsumerBg()
        {
            try
            {
                if (!_backgroundMessageBrokConsum.IsStarted)
                {
                    ModelState.AddModelError("StopMessageBrokerConsumerBg", "Listener already staopped !!!");
                    return BadRequest(ModelState);
                }

                await _backgroundMessageBrokConsum.StopAsync(CancellationToken.None);
                return Ok("Listener Stoping");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в InputDataController/StopListener");
                throw;
            }
        }



        [HttpPost("SendData4Devices")]
        public async Task<IActionResult> SendData4Devices([FromBody] IReadOnlyList<InputData<AdInputType>> inputDatas)
        {
            try
            {
                var res = await InputDataHandler(inputDatas);
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в InputDataController/SendData4Devices");
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
                _logger.Error(ex, "Ошибка в InputDataController/SendDataXml4Devices");
                throw;
            }
        }


        /// <summary>
        /// Отправить данные на 1 Device.
        /// Формат POST запроса Multipart.
        /// XML передается как файл с именем username в FromForm виде.
        /// </summary>
        /// <param name="username">Имя фала</param>
        /// <param name="deviceName"></param>
        /// <param name="exchangeName"></param>
        /// <param name="directHandlerName"></param>
        /// <param name="dataAction"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("SendDataXmlMultipart4Devices")]
        public async Task<IActionResult> SendDataXmlMultipart4Devices([FromForm] IFormFile username,
                                                                      [FromHeader] string deviceName,
                                                                      [FromHeader] string exchangeName = "",
                                                                      [FromHeader] string directHandlerName = "",
                                                                      [FromHeader] string dataAction = "",
                                                                      [FromHeader] string command = "None") // [FromHeader] string fileEncoding = "Windows-1251"
        {
            var xmlFile = username;
            if (xmlFile == null)
            {
                _logger.Warning("SendDataXmlMultipart4Devices. deviceName == null");
                return BadRequest("SendDataXmlMultipart4Devices. xmlFile == null");
            }
            var values = xmlFile.FileName.Split('+');
            if (values.Length == 2)
            {
                var fileName = values[0];
                dataAction = values[1];
                deviceName = deviceName ?? fileName;//Если deviceName не переданн в заголовке 
                _logger.Information(fileName != null ? $"FileName= {fileName}" : "FileName= NULL");//DEBUG
            }
            if (string.IsNullOrEmpty(deviceName))
            {
                ModelState.AddModelError("SendDataXmlMultipart4Devices", "deviceName == null");
                _logger.Warning($"SendDataXmlMultipart4Devices. deviceName == null");
                return BadRequest(ModelState);
            }
            if (!Enum.TryParse(dataAction, out DataAction dataActionParsed))
            {
                ModelState.AddModelError("SendDataXmlMultipart4Devices", "DataAction Error Parse");
                _logger.Warning($"SendDataXmlMultipart4Devices. DataAction Error Parse");
                return BadRequest(ModelState);
            }
            if (!Enum.TryParse(command, out Command4Device commandParse))
            {
                ModelState.AddModelError("SendDataXmlMultipart4Devices", "Command4Device Error Parse");
                _logger.Warning($"SendDataXmlMultipart4Devices. Command4Device Error Parse");
                return BadRequest(ModelState);
            }

            try
            {
                
                if (xmlFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {    
                        await xmlFile.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                        const string encoding = "utf-8";
                        #region Debug
                        //System.IO.File.WriteAllBytes(@"D:\\InDataXml_NewAd.xml", memoryStream.ToArray());
                        var xmlContent = Encoding.GetEncoding(encoding).GetString(memoryStream.ToArray()); //utf-8   "Windows-1251"
                        _logger.Debug($"{xmlContent}");
                        #endregion
                        using (var reader = new StreamReader(memoryStream, Encoding.GetEncoding(encoding), true))
                        {
                            var serializer = new XmlSerializer(typeof(AdInputType4XmlDtoContainer));
                            var adInputType4XmlList = (AdInputType4XmlDtoContainer)serializer.Deserialize(reader);
                            var data = _mapper.Map<List<AdInputType>>(adInputType4XmlList.Trains); //TODO: выставить язык из запроса для всех данных AdInputType
                            InitDataId(data);
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
                    }
                }
                _logger.Warning($"SendDataXmlMultipart4Devices. Размер XML файла равен 0");
                return BadRequest("Размер XML файла равен 0");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в InputDataController/SendDataXmlMultipart4Devices");
                throw;
            }
        }


        /// <summary>
        /// helthCheck по адресу api/InputData/SendDataXml4Devices
        /// Нужно для Клиентского ПО, для поднятия флага ISConnect 
        /// </summary>
        // GET api/InputData/SendDataXml4Devices
        [HttpGet("SendDataXmlMultipart4Devices")]
        public IActionResult SendDataXml4DevicesGet()
        {
            return Ok();
        }

        #endregion



        #region Methods

        /// <summary>
        /// Присвоить Id, если он не установлен
        /// </summary>
        private void InitDataId(IList<AdInputType> datas)
        {
            for (var i = 0; i < datas.Count; i++)
            {
                var data = datas[i];
                if (data.Id == 0)
                    data.Id = i + 1;
            }
        }


        private async Task<ActionResult> InputDataHandler(IReadOnlyList<InputData<AdInputType>> inputDatas)
        {
            var errors = await _inputDataApplyService.ApplyInputData(inputDatas);
            if (errors.Any())
            {
                var errorCompose = new StringBuilder("Error in sending data: ");
                foreach (var err in errors)
                {
                    errorCompose.AppendLine(err);
                }
                ModelState.AddModelError("SendData4Devices", errorCompose.ToString());
                return BadRequest(ModelState);
            }
            return Ok();
        }

        #endregion
    }
}