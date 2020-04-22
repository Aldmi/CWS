using System;
using System.Linq;
using ByRulesInseartedTest.Test.Datas;
using Domain.InputDataModel.Autodictor.IndependentInseartsImpl.Factory;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using FluentAssertions;
using Moq;
using Serilog;
using Xunit;

namespace ByRulesInseartedTest.Test
{
    public class BaseViewRuleTest
    {
        protected readonly ILogger Logger;
        protected readonly IIndependentInseartsHandlersFactory InTypeIndependentInsertsHandlerFactory;


        #region ctor
        public BaseViewRuleTest()
        {
            var mock = new Mock<ILogger>();
            mock.Setup(loger => loger.Debug(""));
            mock.Setup(loger => loger.Error(""));
            mock.Setup(loger => loger.Warning(""));
            Logger = mock.Object;

            InTypeIndependentInsertsHandlerFactory = new AdInputTypeIndependentInseartsHandlersFactory();
        }
        #endregion


        public string ReplaceFirstOccurrence (string Source, string Find, string Replace)
        {
            int Place = Source.IndexOf(Find);
            string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }

        

        [Fact]
        public void CreateStringRequestEmptyRequestOptionTest()
        {
            //Arrange
            string addressDevice = "5";
            var viewRuleOption = new ViewRuleOption()
            {
                Id = 1,
                StartPosition = 0,
                Count = 1,
                BatchSize = 1,
                RequestOption = new RequestOption
                {
                    Header = "",
                    Body ="",
                    Footer = "",
                    Format = "Windows-1251",
                    MaxBodyLenght = 245
                },
                ResponseOption = new ResponseOption
                {
                    //Format = "HEX",
                    //Lenght = 16,
                    //Body = "0246463038254130373741434B454103"
                }
            }; 

            IIndependentInseartsHandlersFactory inputTypeIndependentInsertsHandlersFactory = new AdInputTypeIndependentInseartsHandlersFactory();
            var viewRule =  ViewRule<AdInputType>.Create(addressDevice, viewRuleOption, inputTypeIndependentInsertsHandlersFactory, Logger);

            //Act
            var requestTransfer = viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.InputTypesDefault)?.ToArray();
            var firstItem = requestTransfer.FirstOrDefault();

            //Assert
            requestTransfer.Should().NotBeNull();
            requestTransfer.Length.Should().Be(1);
            firstItem.Request.StrRepresent.Str.Should().Be(String.Empty);
            firstItem.Request.StrRepresent.Format.Should().Be(viewRuleOption.RequestOption.Format);
            firstItem.Request.StrRepresentBase.Str.Should().Be(String.Empty);
            firstItem.Request.StrRepresentBase.Format.Should().Be(viewRuleOption.RequestOption.Format);
            firstItem.Request.ProcessedItemsInBatch.ProcessedItems.Count.Should().Be(1);
        }


        [Fact]
        public void CreateStringRequestMaxBodyLenghtTest()
        {
            //Arrange
            string addressDevice = "5";
            var viewRuleOption = new ViewRuleOption()
            {
                Id = 1,
                StartPosition = 0,
                Count = 1,
                BatchSize = 1,
                RequestOption = new RequestOption
                {
                    Header = "\u0002{AddressDevice:X2}{Nbyte:X2}",
                    Body = "%00001032{(rowNumber*16):D3}3%10$12$00$60$t3{TDepart:t}%00033240{(rowNumber*16):D3}4%10$12$00$60$t3{StationArrival}%00241256{(rowNumber*16):D3}3%10$12$00$60$t1{PathNumber}%400012561451%000012561603%10$10$00$60$t2Московское время {Hour:D2}.{Minute:D2}",
                    Footer = "{CRCXorInverse:X2}\u0003",
                    Format = "Windows-1251",
                    MaxBodyLenght = 10
                },
                ResponseOption = new ResponseOption
                {
                    //Format = "HEX",
                    //Lenght = 16,
                    //Body = "0246463038254130373741434B454103"
                }
            };
            var viewRule =  ViewRule<AdInputType>.Create(addressDevice, viewRuleOption, InTypeIndependentInsertsHandlerFactory, Logger);

            //Act
            var requestTransfer = viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.InputTypesDefault)?.ToArray();
            var firstItem = requestTransfer?.FirstOrDefault();

            //Assert
            firstItem.Should().BeNull();
        }

    }
}