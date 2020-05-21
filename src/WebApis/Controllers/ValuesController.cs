using System.Collections.Generic;
using App.Services.Mediators;
using Autofac;
using AutoMapper;
using Domain.Exchange;
using Domain.InputDataModel.Autodictor.Model;
using Infrastructure.Background.Abstarct;
using Infrastructure.Transport;
using Infrastructure.Transport.Base.Abstract;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Shared.MiddleWares.HandlersOption;
using WebApiSwc.Extensions;


namespace WebApiSwc.Controllers
{
    /// <summary>
    /// Все singleton сервисы передаваемые через DI, в контроллеры, должны быть ПОТОКОБЕЗОПАСНЫЕ.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ValuesController : Controller
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
        public UnitStringConverterOption Get()
        {
            UnitStringConverterOption p= new UnitStringConverterOption
            {
                ReplaseCharStringConverterOption = new ReplaseCharStringConverterOption
                {
                    ToLowerInvariant = false,
                    Mapping = new Dictionary<char, string>
                    {
                      {' ', "pp" },
                      {'1', "rt" },
                      {'2', "ui" }
                    }
                }

                //  ReplaseStringConverterOption = new ReplaseStringConverterOption
                //  {
                //  ToLowerInvariant = false,
                //  Mapping = new Dictionary<string, string>
                //  {
                //      {"1" , "pp" },
                //      {"2", "rt" },
                //      {"3", "ui" }
                //  }
                //}
            };
            return p;
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

            return new List<string> { "str1", "str2" };
        }

        //POST api/values
        [HttpPost]
        public void Post([FromBody]List<StringHandlerMiddleWareOption111> handlers)
        {

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




        //-----------------------------------------
        public class StringHandlerMiddleWareOption111
        {
            public string PropName { get; set; }                       //Имя свойства для обработки

            public List<UnitStringConverterOption> Converters { get; set; }
        }





        //------------------------------------------
    }
}
