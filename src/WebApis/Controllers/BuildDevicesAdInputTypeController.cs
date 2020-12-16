using System;
using System.Threading.Tasks;
using App.Services.Actions;
using App.Services.Exceptions;
using Domain.InputDataModel.Autodictor.Model;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace WebApiSwc.Controllers
{
    /// <summary>
    /// REST api с команлами по созданию Девайсов работающих с AdInputType на базе опций из БД
    /// На базе опций можно сбилдить Device и сохранить его в Storage
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BuildDevicesAdInputTypeController : Controller
    {
        #region fields
        private readonly BuildDeviceService<AdInputType> _buildDeviceService;
        private readonly ILogger _logger;
        #endregion



        #region ctor
        public BuildDevicesAdInputTypeController(BuildDeviceService<AdInputType> buildDeviceService, ILogger logger)
        {

            _buildDeviceService = buildDeviceService;

            _logger = logger;
        }
        #endregion


        #region ApiMethode
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