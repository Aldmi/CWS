using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using WebServer.DTO.XML;
using Worker.Background.Abstarct;

namespace WebServer.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class InputDataController : Controller
    {
        #region fields

        private readonly ISimpleBackground _background;
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
            var backgroundName= config["MessageBrokerConsumer4InData:Name"];
           _background = background[backgroundName];
        }

        #endregion




        #region Api

        // GET api/InputData/GetBackgroundState
        [HttpGet("GetBackgroundState")]
        public async Task<IActionResult> GetBackgroundState()
        {          
            var bgState = _background.IsStarted ? "Started" : "Stoped";
            await Task.CompletedTask;
            return new JsonResult(bgState);
        }

        //TODO: добавить POST - "SendData4Devices", PUT - "StartBg", PUT - "StopBg".  


        /// <summary>
        /// Запустить бекграунд слушателя выходных сообшений от messageBroker
        /// </summary>
        // GET api/InputData/StartListener
        [HttpPut("StartListener")]
        public async Task<IActionResult> StartListener()
        {
            try
            {
                if (_background.IsStarted)
                {
                    ModelState.AddModelError("StartListener", "Listener already started !!!");
                    return BadRequest(ModelState);
                }

                await _background.StartAsync(CancellationToken.None);
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
        [HttpPut("StopListener")]
        public async Task<IActionResult> StopListener()
        {
            try
            {
                if (!_background.IsStarted)
                {
                    ModelState.AddModelError("StartListener", "Listener already staopped !!!");
                    return BadRequest(ModelState);
                }

                await _background.StopAsync(CancellationToken.None);
                return Ok("Listener Stoping");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в InputDataController/StopListener");
                throw;
            }
        }



        [HttpPost("SendData4Devices")]
        public async Task<IActionResult> SendData4Devices([FromBody] IEnumerable<InputData<AdInputType>> inputDatas)
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
                                                                      [FromHeader] string exchangeName,
                                                                      [FromHeader] string directHandlerName,
                                                                      [FromHeader] string dataAction,
                                                                      [FromHeader] string command)
        {

            //DEBUG---------------------------------------------------
            //deviceName = "TestPeronn_45_55_9";
            //exchangeName = "TcpIp_table_TestPeronn_9_Slim";
            //directHandlerName = null;
            //dataAction = "OneTimeAction";
            //command = "None";
            //DEBUG---------------------------------------------------

            var xmlFile = username;
            if (xmlFile == null)
            {
                return BadRequest("Multipart Data == null");
            }
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

            try
            {
                if (username.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await xmlFile.CopyToAsync(memoryStream);
                        #region Debug
                        //System.IO.File.WriteAllBytes(@"D:\\Git\\CWS\\src\\CoreWithAutoFack\\InDataXml.xml", memoryStream.ToArray());
                        //var str= Encoding.Default.GetString(memoryStream.ToArray());
                        #endregion
                        var formatter = new XmlSerializer(typeof(AdInputType4XmlDtoContainer));
                        memoryStream.Position = 0;
                        var adInputType4XmlList = (AdInputType4XmlDtoContainer)formatter.Deserialize(memoryStream);
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
                }
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

        private async Task<ActionResult> InputDataHandler(IEnumerable<InputData<AdInputType>> inputDatas)
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