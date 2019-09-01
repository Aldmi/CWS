using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BL.Services.Actions;
using BL.Services.Exceptions;
using BL.Services.Mediators;
using DAL.Abstract.Entities.Options.Device;
using DAL.Abstract.Entities.Options.Exchange;
using DAL.Abstract.Entities.Options.Transport;
using InputDataModel.Autodictor.Model;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApiSwc.DTO.JSON.OptionsDto;
using WebApiSwc.DTO.JSON.OptionsDto.DeviceOption;
using WebApiSwc.DTO.JSON.OptionsDto.ExchangeOption;
using WebApiSwc.DTO.JSON.OptionsDto.TransportOption;

namespace WebApiSwc.Controllers
{
    /// <summary>
    /// REST api доступа к опциям системы (Devices, Exchanges, Transports)
    /// На базе опций можно сбилдить Device и сохранить его в Storage
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DevicesOptionController : Controller
    {
        #region fields

        private readonly MediatorForDeviceOptions _mediatorForDeviceOptionsRep;
        private readonly BuildDeviceService<AdInputType> _buildDeviceService;

        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        #endregion




        #region ctor

        public DevicesOptionController(MediatorForDeviceOptions mediatorForDeviceOptionsRep,
                                       BuildDeviceService<AdInputType> buildDeviceService,
                                       IMapper mapper,
                                       ILogger logger)
        {
            _mediatorForDeviceOptionsRep = mediatorForDeviceOptionsRep;
            _buildDeviceService = buildDeviceService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion




        #region ApiMethode

        // GET api/devicesoption
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                //TODO: Добавить GetOptionAgregator в _mediatorForOptionsRep. Сделать по аналогии GetOptionAgregatorForDeviceAsync()
                var deviceOptions = await _mediatorForDeviceOptionsRep.GetDeviceOptionsAsync();
                var exchangeOptions = await _mediatorForDeviceOptionsRep.GetExchangeOptionsAsync();
                var transportOption = await _mediatorForDeviceOptionsRep.GetTransportOptionsAsync();

                var deviceOptionsDto = _mapper.Map<List<DeviceOptionDto>>(deviceOptions);
                var exchangeOptionsDto = _mapper.Map<List<ExchangeOptionDto>>(exchangeOptions);
                var transportOptionDto = _mapper.Map<TransportOptionsDto>(transportOption);

                var agregatorOptionDto = new OptionAgregatorDto
                {
                    DeviceOptions = deviceOptionsDto,
                    ExchangeOptions = exchangeOptionsDto,
                    TransportOptions = transportOptionDto
                };
                return new JsonResult(agregatorOptionDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в DevicesOptionController/Get");
                throw;
            }
        }



        // GET api/devicesoption/deviceName
        [HttpGet("{deviceName}", Name = "GetDevice")]
        public async Task<IActionResult> Get([FromRoute]string deviceName)
        {
            try
            {
                if (!await _mediatorForDeviceOptionsRep.IsExistDeviceAsync(deviceName))
                {
                    return NotFound(deviceName);
                }
                var optionAgregator = await _mediatorForDeviceOptionsRep.GetOptionAgregatorForDeviceAsync(deviceName);
                var agregatorOptionDto = _mapper.Map<OptionAgregatorDto>(optionAgregator);
                return new JsonResult(agregatorOptionDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в DevicesOptionController/Get");
                throw;
            }
        }



        // POST api/devicesoption
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OptionAgregatorDto data)
        {
            if (data == null)
            {
                ModelState.AddModelError("AgregatorOptionDto", "POST body is null");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var deviceOptionDto = data.DeviceOptions?.FirstOrDefault();
                var exchangeOptionDto = data.ExchangeOptions;
                var transportOptionDto = data.TransportOptions;
                var deviceOption = _mapper.Map<DeviceOption>(deviceOptionDto);
                var exchangeOption = _mapper.Map<IReadOnlyList<ExchangeOption>>(exchangeOptionDto);
                var transportOption = _mapper.Map<TransportOption>(transportOptionDto);
                await _mediatorForDeviceOptionsRep.AddDeviceOptionAsync(deviceOption, exchangeOption, transportOption);
                return CreatedAtAction("Get", new { deviceName = deviceOptionDto.Name }, data); //возвращает в ответе данные запроса. в Header пишет значение Location→ http://localhost:44138/api/DevicesOption/{deviceName}
            }
            catch (OptionHandlerException ex)
            {
                _logger.Error(ex, "Ошибка в DevicesOptionController/Post");
                ModelState.AddModelError("PostException", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в DevicesOptionController/Post");
                return BadRequest(ex);
            }
        }



        // DELETE api/devicesoption/5
        [HttpDelete("{deviceName}")]
        public async Task<IActionResult> Delete([FromRoute]string deviceName)
        {
            var deviceOption = await _mediatorForDeviceOptionsRep.GetDeviceOptionByNameAsync(deviceName);
            if (deviceOption == null)
                return NotFound(deviceName);

            try
            {
                var deletedOption = await _mediatorForDeviceOptionsRep.RemoveDeviceOptionAsync(deviceOption);
                return Ok(deletedOption);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в DevicesOptionController/Delete");
                throw;
            }
        }


        /// <summary>
        /// В теле запроса должно быть указанно "Ok" в формате application/json
        /// </summary>
        // DELETE api/devicesoption
        [HttpDelete]
        public async Task<IActionResult> Erase([FromBody] string resolution)
        {
            if(!resolution.Equals("Ok"))
                return BadRequest(" Не верный resolution в теле запроса");

            var (isSuccess, _, error) = await _mediatorForDeviceOptionsRep.EraseDeviceOptionAsync();
            if (isSuccess)
                return Ok();

            _logger.Error(error, "Ошибка в DevicesOptionController/Delete");
            return BadRequest(error);
        }


        // PUT api/devicesoption/BuildDevice/deviceName
        [HttpPut("BuildDevice/{deviceName}")]
        public async Task<IActionResult> BuildDevice([FromRoute] string deviceName)
        {
            try
            {
                var newDevice = await _buildDeviceService.BuildDevice(deviceName);
                if (newDevice == null)
                {
                    return NotFound(deviceName);
                }
                return Ok(newDevice);
            }
            catch (StorageHandlerException ex)
            {
                _logger.Error(ex, "Ошибка в DevicesOptionController/BuildDevice");
                ModelState.AddModelError("BuildAndAddDeviceException", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в DevicesOptionController/BuildDevice");
                throw;
            }
        }

        #endregion
    }
}