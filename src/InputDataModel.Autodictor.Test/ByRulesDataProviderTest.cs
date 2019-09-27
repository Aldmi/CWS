using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Autodictor.Services;
using Domain.InputDataModel.Autodictor.StronglyTypedResponse;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Base.Response;
using Domain.InputDataModel.Base.Services;
using FluentAssertions;
using Moq;
using Serilog;
using Xunit;

namespace InputDataModel.Autodictor.Test
{
    public class ByRulesDataProviderTest
    {
        #region field

        private readonly ILogger _logger;
        private readonly IIndependentInsertsService _independentInsertsService;
        private readonly IStronglyTypedResponseFactory _stronglyTypedResponseFactory;

        #endregion
         


         #region ctor

        public ByRulesDataProviderTest()
        {
            var mock = new Mock<ILogger>();
            mock.Setup(loger => loger.Debug(""));
            mock.Setup(loger => loger.Error(""));
            mock.Setup(loger => loger.Warning(""));
            _logger = mock.Object;

            _independentInsertsService= new AdInputTypeIndependentInsertsService();
            _stronglyTypedResponseFactory= new AdStronglyTypedResponseFactory();
        }

        #endregion



        public static IEnumerable<object[]> GetInDataWrapper => new[]
        {
                    new object[]
                    {
                       new InDataWrapper<AdInputType>
                       {
                           Command = Command4Device.None,
                           Datas = new List<AdInputType>
                           {
                               new AdInputType(1,
                                   "777",
                                   new Note{NameRu = ""},
                                   "5",
                                   new EventTrain(0),
                                   new TypeTrain{NameRu = ""},
                                   new Station{NameRu = ""},
                                   new Station{NameRu = ""},
                                   DateTime.Parse("21:20"),
                                   DateTime.Parse("21:44"),
                                   Lang.Ru)
                           }
                       }
                    }
        };




        [Theory]
        [MemberData(nameof(GetInDataWrapper))]
        public async Task StartExchangePipelineTest(InDataWrapper<AdInputType> inDataWrapper)
        {
            // Arrange
            var option = new ProviderOption
            {
                ByRulesProviderOption = new ByRulesProviderOption
                {
                    
                    Rules = new List<RuleOption>
                    {
                        new RuleOption
                        {
                            Name = "Data",
                            AddressDevice = "1"
                        }
                    }
                }
            };

            //var btByRulesDataProvider= new ByRulesDataProvider<AdInputType>(_stronglyTypedResponseFactory, option, _independentInsertsService, _logger);

            //// Act
            //int countSetDataByte = 0;
            //byte[] getDataByte = null;
            //var subscription = btByRulesDataProvider.RaiseSendDataRx.Subscribe(provider =>
            //    {



            //        countSetDataByte = provider.CountSetDataByte; //Сколько байт ожидаем в ответ
            //        getDataByte = provider.GetDataByte(); // ByRulesDataProvider выставляет массив байт для транспорта
            //        countSetDataByte.Should().Be(5);
            //    });

            //await btByRulesDataProvider.StartExchangePipeline(inDataWrapper);


            // Asssert
        }


    }
}