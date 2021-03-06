﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Services.Actions;
using App.Services.Facade;
using App.Services.Mediators;
using AutoMapper;
using CSharpFunctionalExtensions;
using Domain.Device.Repository.Entities.ResponseProduser;
using Domain.InputDataModel.Autodictor.Model;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApiSwc.DTO.JSON.OptionsDto.ProduserUnionOption;

namespace WebApiSwc.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProdusersUnionController : Controller
    {
        #region fields

        private readonly ProduserUnionOptionRepositoryFacade _produserUnionOptionRepositoryFacade;
        private readonly BuildProdusersUnionService<AdInputType> _buildDeviceService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        #endregion




        #region ctor

        public ProdusersUnionController(ProduserUnionOptionRepositoryFacade produserUnionOptionRepositoryFacade,
            BuildProdusersUnionService<AdInputType> buildDeviceService,
            IMapper mapper,
            ILogger logger)
        {
            _produserUnionOptionRepositoryFacade = produserUnionOptionRepositoryFacade;
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
                var produserUnionOptions = await _produserUnionOptionRepositoryFacade.GetProduserUnionOptionsAsync();
                var produserUnionOptionsDto = _mapper.Map<List<ProduserUnionOptionDto>>(produserUnionOptions);
                return new JsonResult(produserUnionOptionsDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка в ProdusersUnionController/Get");
                throw;
            }
        }


        // GET api/ProdusersUnion/id
        [HttpGet("{id}", Name = "GetProduserUnion")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            try
            {
                if (!await _produserUnionOptionRepositoryFacade.IsExistProduserUnionAsyncById(id))
                {
                    return NotFound(id);
                }
                var produserUnionOption = await _produserUnionOptionRepositoryFacade.GetProduserUnionOptionAsync(id);
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
                var (_, isFailure, value, error) = await _buildDeviceService.AddOrUpdateAndBuildProduserAsync(produserUnionOption);
                if(isFailure)
                    return BadRequest($"{error}");

                return CreatedAtAction("Get", new { deviceName = value.GetKey }, data);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Критическая Ошибка в ProduserUnionOptionDto/Post");
                return BadRequest(ex);
            }
        }


        // DELETE api/ProdusersUnion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var produserUnionOption = await _produserUnionOptionRepositoryFacade.GetProduserUnionOptionAsync(id);
            if (produserUnionOption == null)
                return NotFound(id);

            try
            {
                var deletedOptionDeleted = await _buildDeviceService.RemoveProduserAsync(produserUnionOption);
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