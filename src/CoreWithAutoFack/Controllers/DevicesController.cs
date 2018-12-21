using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using BL.Services.Actions;
using BL.Services.Exceptions;
using BL.Services.Mediators;
using InputDataModel.Autodictor.Model;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Types;
using WebServer.DTO.JSON.DevicesStateDto;

namespace WebServer.Controllers
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
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        #endregion




        #region ctor

        public DevicesController(MediatorForStorages<AdInputType> mediatorForStorages,
                                 DeviceActionService<AdInputType> deviceActionService,
                                 IMapper mapper,
                                 ILogger logger)
        {
            _mediatorForStorages = mediatorForStorages;
            _deviceActionService = deviceActionService;
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
        public IActionResult StartCycleExchange([FromBody] IEnumerable<string> exchnageKeys)
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
        public IActionResult StopCycleExchange([FromBody] IEnumerable<string> exchnageKeys)
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
        /// <param name="exchnageKeys"></param>
        /// <returns></returns>
        // PUT api/Devices/StartCycleReOpenedConnection
        [HttpPut("StartCycleReOpenedConnection")]
        public async Task<IActionResult> StartCycleReOpenedConnection([FromBody] IEnumerable<string> exchnageKeys)
        {
            try
            {
               await _deviceActionService.StartCycleReOpenedConnections(exchnageKeys);
               return Ok(exchnageKeys);
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



        // PUT api/Devices/StartCycleReOpenedConnection
        [HttpPut("StopCycleReOpenedConnection")]
        public IActionResult StopCycleReOpenedConnection([FromBody] IEnumerable<string> exchnageKeys)
        {
            try
            {
                _deviceActionService.StopCycleReOpenedConnections(exchnageKeys);
                return Ok(exchnageKeys);
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

        #endregion
    }
}