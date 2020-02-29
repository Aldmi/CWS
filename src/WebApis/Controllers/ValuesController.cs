using System.Collections.Generic;
using App.Services.Mediators;
using Autofac;
using AutoMapper;
using Domain.Exchange;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersOption;
using Infrastructure.Background.Abstarct;
using Infrastructure.Transport;
using Infrastructure.Transport.Base.Abstract;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using WebApiSwc.DTO.JSON.InputTypesDto;
using WebApiSwc.DTO.JSON.OptionsDto.ExchangeOption.ProvidersOption;
using WebApiSwc.Extensions;
using WebApiSwc.Settings;



namespace WebApiSwc.Controllers
{
    /// <summary>
    /// Все singleton сервисы передаваемые через DI, в контроллеры, должны быть ПОТОКОБЕЗОПАСНЫЕ.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ValuesController: Controller
    {
        private readonly MediatorForStorages<AdInputType> _mediatorForStorages;
        private readonly TransportStorage _spSrStorage;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IExchange<AdInputType>> _excBehaviors;
        private readonly IEnumerable<ITransportBackground> _backgroundServices;
        private readonly ILifetimeScope _scope;
        private readonly ISerailPort _spService;



        public ValuesController(TransportStorage spSrStorage, IMapper mapper)
        {
            _spSrStorage = spSrStorage;
            _mapper = mapper;
        }




        // GET api/values
        [HttpGet]
        // public IEnumerable<InputData<AdInputType>> Get()
        public ResponseOptionDto Get()
        {
            ResponseOptionDto responseOptionDto = new ResponseOptionDto()
            {
                ValidatorName = "EqualValidator",
                TimeRespone = 500,
                EqualValidator = new EqualResponseValidatorOption
                {
                    Body = "0246463038254130373741434B454103",
                    Format = "HEX"
                }
            };
            return responseOptionDto;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IEnumerable<string> Get(int id)
        {
            switch (id)
            {
                case 0:
                    SerilogExtensions.ChangeLogEventLevel(LogEventLevel.Debug);
                    break;

                case 1:
                    SerilogExtensions.ChangeLogEventLevel(LogEventLevel.Error);
                    break;
            }
    
            return new List<string>{"str1", "str2"};
        }

        //POST api/values
       [HttpPost]
        public void Post([FromBody]AdInputTypeDto value)
        {
            var res= _mapper.Map<AdInputType>(value);

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
