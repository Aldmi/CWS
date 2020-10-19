using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Services.Actions;
using AutoMapper;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApiSwc.DTO.JSON.InputTypesDto;

namespace WebApiSwc.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StringInsertModelExtController : Controller
    {
        #region fields
        private readonly BuildStringInsertModelExt _buildService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion



        #region ctor
        public StringInsertModelExtController(
            BuildStringInsertModelExt buildService,
            IMapper mapper,
            ILogger logger)
        {
            _buildService = buildService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion



        #region ApiMethode

        // GET api/StringInsertModelExt
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var list =  _buildService.GetValuesFromStorage();
                var listDto = _mapper.Map<List<StringInsertModelExtDto>>(list);
                return new JsonResult(listDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в StringInsertModelExtController/Get");
                throw;
            }
        }


        // GET api/StringInsertModelExt/varName
        [HttpGet("{varName}", Name = "GetModel")]
        public ActionResult Get([FromRoute]string varName)
        {
            try
            {
                var model = _buildService.GetValuesFromStorageByVarName(varName);
                if (model == null)
                {
                    return NotFound(varName);
                }
        
                var modelDto = _mapper.Map<StringInsertModelExtDto>(model);
                return new JsonResult(modelDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в StringInsertModelExtController/Get");
                throw;
            }
        }


        // POST api/StringInsertModelExt
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]StringInsertModelExtDto data)
        {
            if (data == null)
            {
                ModelState.AddModelError("StringInsertModelExt", "POST body is null");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var model = _mapper.Map<StringInsertModelExt>(data);
                var (_, isFailure, value, error) = await _buildService.AddOrUpdateAndBuildAsync(model);
                if(isFailure)
                    return BadRequest($"{error}");

                return CreatedAtAction("Get",  data);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в StringInsertModelExt/Post");
                return BadRequest(ex);
            }
        }


        // POST api/StringInsertModelExt/AddOrUpdateList
        [HttpPost("AddOrUpdateList")]
        public async Task<IActionResult> AddOrUpdateList([FromBody]List<StringInsertModelExtDto> listDto)
        {
            if (listDto == null)
            {
                ModelState.AddModelError("StringInsertModelExt", "POST body is null");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var listModels = _mapper.Map<List<StringInsertModelExt>>(listDto);
                var (_, isFailure, error) = await _buildService.AddOrUpdateAndBuildListAsync(listModels);
                if (isFailure)
                    return BadRequest($"{error}");

                return CreatedAtAction("Get", listDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в StringInsertModelExt/Post");
                return BadRequest(ex);
            }
        }


        // DELETE api/StringInsertModelExt/varName
        [HttpDelete("{varName}")]
        public async Task<IActionResult> Delete([FromRoute]string varName)
        {
            if (string.IsNullOrEmpty(varName))
            {
                ModelState.AddModelError("StringInsertModelExt", "varName is null or empty");
                return BadRequest(ModelState);
            }


            var model = _buildService.GetValuesFromStorageByVarName(varName);
            if (model == null)
                return NotFound(varName);

            try
            {
                var modelDeleted = await _buildService.RemoveAsync(model);
                return Ok(modelDeleted);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в StringInsertModelExt/Delete");
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
            if (!resolution.Equals("Ok"))
                return BadRequest(" Не верный resolution в теле запроса");

            var (isSuccess, _, error) = await _buildService.EraseAsync();
            if (isSuccess)
                return Ok();

            _logger.Error(error, "Ошибка в StringInsertModelExtController/Erase");
            return BadRequest(error);
        }

        #endregion
    }
}