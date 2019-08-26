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
using DAL.Abstract.Entities.Options.ResponseProduser;
using DAL.Abstract.Entities.Options.Transport;
using InputDataModel.Autodictor.Model;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApiSwc.DTO.JSON.OptionsDto;
using WebApiSwc.DTO.JSON.OptionsDto.DeviceOption;
using WebApiSwc.DTO.JSON.OptionsDto.ExchangeOption;
using WebApiSwc.DTO.JSON.OptionsDto.ProduserUnionOption;
using WebApiSwc.DTO.JSON.OptionsDto.TransportOption;
using WebClientProduser.Options;

namespace WebApiSwc.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProdusersUnionController : Controller
    {
        #region fields

        private readonly MediatorForOptions _mediatorForOptionsRep;
        private readonly BuildProdusersUnionService<AdInputType> _buildDeviceService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        #endregion




        #region ctor

        public ProdusersUnionController(MediatorForOptions mediatorForOptionsRep,
            BuildProdusersUnionService<AdInputType> buildDeviceService,
            IMapper mapper,
            ILogger logger)
        {
            _mediatorForOptionsRep = mediatorForOptionsRep;
            _buildDeviceService = buildDeviceService;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion



        #region ApiMethode

        // GET api/ProdusersUnion
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var produserUnionOptions = await _mediatorForOptionsRep.GetProduserUnionOptionsAsync();
                var produserUnionOptionsDto = _mapper.Map<List<ProduserUnionOptionDto>>(produserUnionOptions);
                return new JsonResult(produserUnionOptionsDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в ProdusersUnionController/Get");
                throw;
            }
        }


        // GET api/ProdusersUnion/Key
        [HttpGet("{key}", Name = "GetProduserUnion")]
        public async Task<IActionResult> Get([FromRoute]string key)
        {
            try
            {
                if (!await _mediatorForOptionsRep.IsExistProduserUnionAsync(key))
                {
                    return NotFound(key);
                }
                var produserUnionOption = await _mediatorForOptionsRep.GetProduserUnionOptionAsync(key);
                var produserUnionOptionDto = _mapper.Map<ProduserUnionOptionDto>(produserUnionOption);
                return new JsonResult(produserUnionOptionDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в ProdusersUnionController/Get");
                throw;
            }
        }


        // POST api/ProdusersUnion
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ProduserUnionOptionDto data)
        {
            if (data == null)
            {
                ModelState.AddModelError("ProduserUnionOption", "POST body is null");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var produserUnionOption = _mapper.Map<ProduserUnionOption>(data);
                var res=  await _mediatorForOptionsRep.AddProduserUnionOptionAsync(produserUnionOption);
                return CreatedAtAction("Get", new { deviceName = produserUnionOption.Key }, data); //возвращает в ответе данные запроса. в Header пишет значение Location→ http://localhost:44138/api/DevicesOption/{deviceName}
            }
            catch (OptionHandlerException ex)
            {
                _logger.Error(ex, "Ошибка в ProduserUnionOptionDto/Post");
                ModelState.AddModelError("PostException", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в ProduserUnionOptionDto/Post");
                return BadRequest(ex);
            }
        }




        // DELETE api/ProdusersUnion/5
        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete([FromRoute]string key)
        {
            var produserUnionOption = await _mediatorForOptionsRep.GetProduserUnionOptionAsync(key);
            if (produserUnionOption == null)
                return NotFound(key);

            try
            {
                var deletedOptionDeleted = await _mediatorForOptionsRep.RemoveProduserUnionOptionAsync(produserUnionOption);
                return Ok(deletedOptionDeleted);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в DevicesOptionController/Delete");
                throw;
            }
        }




        #endregion
    }
}