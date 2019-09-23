using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Services.Actions;
using App.Services.Exceptions;
using App.Services.Mediators;
using Autofac.Features.Indexed;
using AutoMapper;
using Domain.Device.Repository.Entities.MiddleWareOption;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Base.Response;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Types;
using WebApiSwc.DTO.JSON.DevicesStateDto;
using WebApiSwc.DTO.JSON.OptionsDto.ExchangeOption.ProvidersOption;
using WebApiSwc.DTO.JSON.OptionsDto.MiddleWareOption;

namespace WebApiSwc.Controllers
{
    /// <summary>
    /// REST api доступа к оперативному хранилищу устройств (Devices, Exchanges, Transports).
    /// Удалить устройство из хранилища, остановить все эксклюзивные сервисы, связанные с ним (Exchanges, Transports)
    /// Start/Stop цикл. обмен на Exchange.
    /// Start/Stop БГ для транспорта.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DevicesController : Controller
    {
        #region fields

        private readonly MediatorForStorages<AdInputType> _mediatorForStorages;
        private readonly DeviceActionService<AdInputType> _deviceActionService;
        private readonly IIndex<string, Func<ProviderOption, IDataProvider<AdInputType, ResponseInfo>>> _dataProviderFactory;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        #endregion




        #region ctor

        public DevicesController(MediatorForStorages<AdInputType> mediatorForStorages,
                                 DeviceActionService<AdInputType> deviceActionService,
                                 IIndex<string, Func<ProviderOption, IDataProvider<AdInputType, ResponseInfo>>> dataProviderFactory, //TODO: попробовать вендрять в prop
                                 IMapper mapper,
                                 ILogger logger)
        {
            _mediatorForStorages = mediatorForStorages;
            _deviceActionService = deviceActionService;
            _dataProviderFactory = dataProviderFactory;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion




        #region Methode

        // GET api/Devices/GetDevices
        [HttpGet("GetDevices")]
        public async Task<IActionResult> GetDevices()
        {
            var devices = _mediatorForStorages.GetDevices();
            var devicesStateDto = _mapper.Map<List<DeviceStateDto>>(devices);
            await Task.CompletedTask;
            return new JsonResult(devicesStateDto);
        }


        // GET api/Devices/GetDevicesUsingExchange/exchnageKey
        [HttpGet("{exchnageKey}")]
        public async Task<IActionResult> GetDevicesUsingExchange([FromRoute] string exchnageKey)
        {
            var devices= _mediatorForStorages.GetDevicesUsingExchange(exchnageKey);
            var devicesStateDto = _mapper.Map<List<DeviceStateDto>>(devices);
            await Task.CompletedTask;
            return new JsonResult(devicesStateDto);
        }


        // GET api/Devices/GetExchangesState/{deviceName}
        [HttpGet("GetExchangesState/{deviceName}")]
        public async Task<IActionResult> GetExchangesState([FromRoute] string deviceName)
        {
            var device = _mediatorForStorages.GetDevice(deviceName);
            if (device == null)
            {
                return NotFound(deviceName);
            }
            var exchangesStateDto = _mapper.Map<List<ExchangeStateDto>>(device.Exchanges);
            await Task.CompletedTask;
            return new JsonResult(exchangesStateDto);
        }



        // DELETE api/Devices/{deviceName}
        [HttpDelete("{deviceName}")]
        public async Task<IActionResult> RemoveDevice([FromRoute] string deviceName)
        {
            var device= _mediatorForStorages.GetDevice(deviceName);
            if (device == null)
            {
                return NotFound(deviceName);
            }
            try
            {
                await _mediatorForStorages.RemoveDevice(deviceName);
                return Ok(device);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в DevicesController/RemoveDevice");
                throw;
            }
        }



        /// <summary>
        /// Запустить обмен по ключу
        /// </summary>
        /// <param name="exchnageKeys">Установить функции цикл. обмена на БГ для этих обменов</param>
        /// <returns></returns>
        // PUT api/Devices/StartCycleExchange
        [HttpPut("StartCycleExchange")]
        public IActionResult StartCycleExchange([FromBody] IReadOnlyList<string> exchnageKeys)
        {
            try
            {
                _deviceActionService.StartCycleExchanges(exchnageKeys);
                var resp = (from exchangeKey in exchnageKeys
                            let devices = _mediatorForStorages.GetDevicesUsingExchange(exchangeKey)
                            select new { message = "ЗАПУЩЕННЫЙ ЦИКЛ. ОБМЕН",  devices, exchangeKey }).ToList();
                return Ok(resp);
            }
            catch (ActionHandlerException ex)
            {
                _logger.Error(ex, "Ошибка в DevicesController/StartCycleExchange");
                ModelState.AddModelError("StartCycleExchangeException", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в DevicesController/StartCycleExchange");
                throw;
            }
        }



        /// <summary>
        /// Остановить обмен по ключу
        /// </summary>
        /// <param name="exchnageKeys">Список устройств которые используют данный обмен</param>
        /// <returns></returns>
        // PUT api/Devices/StopCycleExchange
        [HttpPut("StopCycleExchange")]
        public IActionResult StopCycleExchange([FromBody] IReadOnlyList<string> exchnageKeys)
        {
            try
            {
                _deviceActionService.StopCycleExchanges(exchnageKeys);
                var resp = (from exchangeKey in exchnageKeys
                    let devices = _mediatorForStorages.GetDevicesUsingExchange(exchangeKey)
                    select new { message = "ОСТАНОВЛЕННЫЙ ЦИКЛ. ОБМЕН",  devices,  exchangeKey }).ToList();
                return Ok(resp);
            }
            catch (ActionHandlerException ex)
            {
                _logger.Error(ex, "Ошибка в DevicesController/StopCycleExchange");
                ModelState.AddModelError("StopCycleExchangeException", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в DevicesController/StopCycleExchange");
                throw;
            }
        }



        /// <summary>
        /// Запустить ЦИКЛ. переоткрытие транспорта
        /// </summary>
        // PUT api/Devices/StartCycleReOpenedConnection
        [HttpPut("StartCycleReOpenedConnection")]
        public async Task<IActionResult> StartCycleReOpenedConnection([FromBody] IReadOnlyList<KeyTransport> keysTransports)
        {
            try
            {
               await _deviceActionService.StartCycleReOpenedConnections(keysTransports);
               return Ok(keysTransports);
            }
            catch (ActionHandlerException ex)
            {
                _logger.Error(ex, "Ошибка в DevicesController/StartCycleReOpenedConnection");
                ModelState.AddModelError("StartCycleReOpenedConnection", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в DevicesController/StartCycleReOpenedConnection");
                throw;
            }
        }


        /// <summary>
        /// Остановить ЦИКЛ. переоткрытие транспорта
        /// </summary>
        // PUT api/Devices/StopCycleReOpenedConnection
        [HttpPut("StopCycleReOpenedConnection")]
        public IActionResult StopCycleReOpenedConnection([FromBody] IReadOnlyList<KeyTransport> keysTransports)
        {
            try
            {
                _deviceActionService.StopCycleReOpenedConnections(keysTransports);
                return Ok(keysTransports);
            }
            catch (ActionHandlerException ex)
            {
                _logger.Error(ex, "Ошибка в DevicesController/StopCycleReOpenedConnection");
                ModelState.AddModelError("StopCycleReOpenedConnections", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в DevicesController/StopCycleReOpenedConnection");
                throw;
            }
        }


        /// <summary>
        /// Запустить БГ транспорта по ключу
        /// </summary>
        /// <param name="keysTransport">Список ключей транспорта</param>
        /// <returns></returns>
        // PUT api/Devices/StartBackgrounds
        [HttpPut("StartBackgrounds")]
        public async Task<IActionResult> StartBackgrounds([FromBody] IReadOnlyList<KeyTransport> keysTransport)
        {
            try
            {
                foreach (var keyTransport in keysTransport)
                {
                  var bg= _mediatorForStorages.GetBackground(keyTransport);
                  if (bg == null)
                     return BadRequest($"keysTransport {keyTransport}");
                }

                await _deviceActionService.StartBackgrounds(keysTransport);
                var resp = (from keyTransport in keysTransport
                            let exchangesUsingTransport = _mediatorForStorages.GetExchangesUsingTransport(keyTransport).Select(e => e.KeyExchange)
                            select new { message = "ЗАПУЩЕННЫЙ ТРАНСПОРТ", exc = exchangesUsingTransport, keyTranspor = keyTransport }).ToList();
                return Ok(resp);
            }
            catch (ActionHandlerException ex)
            {
                _logger.Error(ex, "Ошибка в DevicesController/StartBackgrounds");
                ModelState.AddModelError("StartBackgrounds", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в DevicesController/StartBackgrounds");
                throw;
            }
        }


        /// <summary>
        /// Остановить БГ транспорта по ключу
        /// </summary>
        /// <param name="keysTransport">Список ключей транспорта</param>
        /// <returns></returns>
        [HttpPut("StopBackgrounds")]
        public async Task<IActionResult> StopBackgrounds([FromBody] IReadOnlyList<KeyTransport> keysTransport)
        {
            try
            {
                await _deviceActionService.StopBackgrounds(keysTransport);
                var resp = (from keyTransport in keysTransport let exchangesUsingTransport = _mediatorForStorages.GetExchangesUsingTransport(keyTransport).Select(e=>e.KeyExchange)
                            select new {message= "ОСТАНОВЛЕННЫЙ ТРАНСПОРТ", exc = exchangesUsingTransport, keyTranspor = keyTransport }).ToList();
                return Ok(resp);
            }
            catch (ActionHandlerException ex)
            {
                _logger.Error(ex, "Ошибка в DevicesController/StartBackgrounds");
                ModelState.AddModelError("StartBackgrounds", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в DevicesController/StartBackgrounds");
                throw;
            }
        }



        // GET api/Devices/GetProviderOption/{deviceName}/{exchName}
        [HttpGet("GetProviderOption/{deviceName}/{exchName}")]
        public async Task<IActionResult> GetProviderOption([FromRoute] string deviceName, [FromRoute] string exchName)
        {
            var (_, isFailure, option, error) = _deviceActionService.GetProviderOption(deviceName, exchName);
            if (isFailure)
                return BadRequest(error);

            var providerOptionDto = _mapper.Map<ProviderOptionDto>(option);

            await Task.CompletedTask;
            return new JsonResult(providerOptionDto);
        }


        // POST api/Devices/SetProviderOption/{deviceName}/{exchName}
        [HttpPost("SetProviderOption/{deviceName}/{exchName}")]
        public async Task<IActionResult> SetProviderOption(
            [FromRoute] string deviceName,
            [FromRoute] string exchName,
            [FromBody] ProviderOptionDto providerOptionDto)
        {

            var providerOption = _mapper.Map<ProviderOption>(providerOptionDto);
            var (_, isFailure, error) = _deviceActionService.SetProvider(deviceName, exchName, providerOption);
            if (isFailure)
                return BadRequest(error);

            await Task.CompletedTask;
            return Ok("Опции успешно приняты");
        }


        // GET api/Devices/GetMiddleWareInDataOption/{deviceName}
        [HttpGet("GetMiddleWareInDataOption/{deviceName}")]
        public async Task<IActionResult> GetMiddleWareInDataOption([FromRoute] string deviceName)
        {
            var device = _mediatorForStorages.GetDevice(deviceName);
            if (device == null)
            {
                return NotFound(deviceName);
            }

            var middleWareInDataOption = device.GetMiddleWareInDataOption();
            var middleWareInDataOptionDto = _mapper.Map<MiddleWareInDataOptionDto>(middleWareInDataOption);

            await Task.CompletedTask;
            return new JsonResult(middleWareInDataOptionDto);
        }


        // POST api/Devices/SetMiddleWareInDataOption/{deviceName}
        [HttpPost("SetMiddleWareInDataOption/{deviceName}")]
        public async Task<IActionResult> SetMiddleWareInDataOption([FromRoute] string deviceName, [FromBody] MiddleWareInDataOptionDto middleWareInDataOptionDto)
        {
            var device = _mediatorForStorages.GetDevice(deviceName);
            if (device == null)
            {
                return NotFound(deviceName);
            }

            var middleWareInDataOption = _mapper.Map<MiddleWareInDataOption>(middleWareInDataOptionDto);
            try
            {
                device.SetMiddleWareInDataOptionAndCreateNewMiddleWareInData(middleWareInDataOption);
            }
            catch (Exception ex)
            {
                return BadRequest($"Исключение при установке middleWareInDataOption {ex}");
            }

            await Task.CompletedTask;
            return Ok("Опции успешно приняты");
        }

        #endregion
    }
}