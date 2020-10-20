using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Services.Actions;
using AutoMapper;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model.InlineStringInsert;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApiSwc.DTO.JSON.InputTypesDto;

namespace WebApiSwc.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class InlineStringInsertModelController : Controller
    {
        #region fields
        private readonly BuildInlineStringInsertModel _buildService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        #endregion



        #region ctor
        public InlineStringInsertModelController(
            BuildInlineStringInsertModel buildService,
            IMapper mapper,
            ILogger logger)
        {
            _buildService = buildService;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion




        #region ApiMethode

        // GET api/InlineStringInsertModel
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var list = _buildService.GetValuesFromStorage();
                var listDto = _mapper.Map<List<InlineStringInsertModelDto>>(list);
                return new JsonResult(listDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в InlineStringInsertModelController/Get");
                throw;
            }
        }


        // GET api/InlineStringInsertModel/key
        [HttpGet("{key}", Name = "GetInlineInsModel")]
        public ActionResult Get([FromRoute] string key)
        {
            try
            {
                var model = _buildService.GetValuesFromStorageByVarName(key);
                if (model == null)
                {
                    return NotFound(key);
                }

                var modelDto = _mapper.Map<InlineStringInsertModelDto>(model);
                return new JsonResult(modelDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в InlineStringInsertModelController/Get by key");
                throw;
            }
        }

        // POST api/InlineStringInsertModel
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InlineStringInsertModelDto data)
        {
            if (data == null)
            {
                ModelState.AddModelError("InlineStringInsertModel", "POST body is null");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var model = _mapper.Map<InlineStringInsertModel>(data);
                var (_, isFailure, value, error) = await _buildService.AddOrUpdateAndBuildAsync(model);
                if (isFailure)
                    return BadRequest($"{error}");

                return CreatedAtAction("Get", data);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в InlineStringInsertModel/Post");
                return BadRequest(ex);
            }
        }



        // POST api/InlineStringInsertModel/AddOrUpdateList
        [HttpPost("AddOrUpdateList")]
        public async Task<IActionResult> AddOrUpdateList([FromBody] List<InlineStringInsertModelDto> listDto)
        {
            if (listDto == null)
            {
                ModelState.AddModelError("InlineStringInsertModel", "POST body is null");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var listModels = _mapper.Map<List<InlineStringInsertModel>>(listDto);
                var (_, isFailure, error) = await _buildService.AddOrUpdateAndBuildListAsync(listModels);
                if (isFailure)
                    return BadRequest($"{error}");

                return CreatedAtAction("Get", listDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в InlineStringInsertModel/Post");
                return BadRequest(ex);
            }
        }


        // DELETE api/InlineStringInsertModel/varName
        [HttpDelete("{varName}")]
        public async Task<IActionResult> Delete([FromRoute] string varName)
        {
            if (string.IsNullOrEmpty(varName))
            {
                ModelState.AddModelError("InlineStringInsertModel", "varName is null or empty");
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
                _logger.Error(ex, "Ошибка в InlineStringInsertModel/Delete");
                throw;
            }
        }


        /// <summary>
        /// В теле запроса должно быть указанно "Ok" в формате application/json
        /// </summary>
        // DELETE api/InlineStringInsertModel/
        [HttpDelete]
        public async Task<IActionResult> Erase([FromBody] string resolution)
        {
            if (!resolution.Equals("Ok"))
                return BadRequest("Не верный resolution в теле запроса");

            var (isSuccess, _, error) = await _buildService.EraseAsync();
            if (isSuccess)
                return Ok();

            _logger.Error(error, "Ошибка в InlineStringInsertModel/Erase");
            return BadRequest(error);
        }

        #endregion
    }
}