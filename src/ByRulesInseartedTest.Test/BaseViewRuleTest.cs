using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ByRulesInseartedTest.Test.Datas;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Autodictor.ForProviderImpl.IndependentInseartsImpl.Factory;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders.Rules;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;
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
        protected readonly IReadOnlyDictionary<string, StringInsertModelExt> StringInsertModelExtDictionary;

        #region ctor
        public BaseViewRuleTest()
        {
            var mock = new Mock<ILogger>();
            mock.Setup(loger => loger.Debug(""));
            mock.Setup(loger => loger.Error(""));
            mock.Setup(loger => loger.Warning(""));
            Logger = mock.Object;

            InTypeIndependentInsertsHandlerFactory = new AdInputTypeIndependentInseartsHandlersFactory();
            StringInsertModelExtDictionary = GetStringInsertModelExtDict.SimpleDictionary;
        }
        #endregion



        [Fact]
        public async Task CreateStringRequestEmptyRequestOptionTest()
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
            var viewRule =  ViewRule<AdInputType>.Create(addressDevice, viewRuleOption, inputTypeIndependentInsertsHandlersFactory, StringInsertModelExtDictionary, Logger);

            //Act
            var requestTransfer =  await viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.InputTypesDefault).ToArrayAsync();
            var (isSuccess, isFailure, providerTransfer, error) = requestTransfer.FirstOrDefault();

            //Assert
            requestTransfer.Should().NotBeNull();
            requestTransfer.Length.Should().Be(1);
            isSuccess.Should().BeTrue();

            var request = providerTransfer.Request;
            request.StrRepresent.Str.Should().Be(String.Empty);
            request.StrRepresent.Format.Should().Be(viewRuleOption.RequestOption.Format);
            request.StrRepresentBase.Str.Should().Be(String.Empty);
            request.StrRepresentBase.Format.Should().Be(viewRuleOption.RequestOption.Format);
            request.ProcessedItemsInBatch.ProcessedItems.Count.Should().Be(1);
        }


        [Fact]
        public async Task CheckError_CreateStringRequestMaxBodyLenght_Test()
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
                    Body = "%00001032{MATH(rowNumber*16):D3}3%10$12$00$60$t3{TDepart:t}%00033240{MATH(rowNumber*16):D3}4%10$12$00$60$t3{StationArrival}%00241256{MATH(rowNumber*16):D3}3%10$12$00$60$t1{PathNumber}%400012561451%000012561603%10$10$00$60$t2Московское время {Hour:D2}.{Minute:D2}",
                    Footer = "",
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
            var viewRule =  ViewRule<AdInputType>.Create(addressDevice, viewRuleOption, InTypeIndependentInsertsHandlerFactory, StringInsertModelExtDictionary, Logger);

            //Act
            var requestTransfer = await viewRule.CreateProviderTransfer4Data(GetData4ViewRuleTest.InputTypesDefault).ToArrayAsync();
            var (isSuccess, isFailure, providerTransfer, error) = requestTransfer.FirstOrDefault();

            //Assert
            isSuccess.Should().BeFalse();
            error.Should().Be("Строка тела запроса СЛИШКОМ БОЛЬШАЯ. Превышение на 149");
        }
    }
}