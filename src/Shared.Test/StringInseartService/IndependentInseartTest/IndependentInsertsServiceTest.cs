using System;
using System.Collections.Generic;
using System.Linq;
using Domain.InputDataModel.Autodictor.ForProviderImpl.IndependentInseartsImpl.Factory;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using FluentAssertions;
using Moq;
using Serilog;
using Shared.Test.StringInseartService.Datas;
using Xunit;

namespace Shared.Test.StringInseartService.IndependentInseartTest
{
    public class IndependentInsertsServiceTest
    {
        private readonly ILogger _logger;
        private readonly List<Func<StringInsertModel, IIndependentInsertsHandler>> _handlerFactorys;


        #region ctor
        public IndependentInsertsServiceTest()
        {
            IIndependentInseartsHandlersFactory inputTypeInseartsHandlersFactory = new AdInputTypeIndependentInseartsHandlersFactory(); //TODO: попробовать внедрять через DI
            _handlerFactorys = new List<Func<StringInsertModel, IIndependentInsertsHandler>>
            {
                new DefaultIndependentInseartsHandlersFactory().Create,
                inputTypeInseartsHandlersFactory.Create
            };

            var mock = new Mock<ILogger>();
            mock.Setup(loger => loger.Debug(""));
            mock.Setup(loger => loger.Error(""));
            mock.Setup(loger => loger.Warning(""));
            _logger = mock.Object;
        }
        #endregion


        [Fact]
        public void HeaderExecuteInseartsTest()
        {
            //Arrage
            var str = "\u0002{AddressDevice:X2} {Nbyte:D3}";
            var independentInsertsService = IndependentInsertsServiceFactory.CreateIndependentInsertsService(str,  _handlerFactorys, GetStringInsertModelExtDict.SimpleDictionary, _logger);

            //Act
            var headerExecuteInseartsResult = independentInsertsService.ExecuteInsearts(new Dictionary<string, string> { { "AddressDevice", "5" } }).result;
            var resStr = headerExecuteInseartsResult.ToString();

            //Assert
            resStr.Should().Be("\u000205 {Nbyte:D3}");
        }


        [Fact]
        public void FooterExecuteInseartsTest()
        {
            //Arrage
            var str = "{CRCXorInverse:X2}\u0003";
            var independentInsertsService = IndependentInsertsServiceFactory.CreateIndependentInsertsService(str,  _handlerFactorys, GetStringInsertModelExtDict.SimpleDictionary, _logger);

            //Act
            var headerExecuteInseartsResult = independentInsertsService.ExecuteInsearts(null).result;
            var resStr = headerExecuteInseartsResult.ToString();

            //Assert
            resStr.Should().Be(str);
        }


        #region TheoryData
        public static IEnumerable<object[]>  BoodyExecuteInseartsDatas => new[]
        {
            //Default Extension для разных типов: string, DateTime, int
            new object[]
            {
                "{NumberOfTrain} {TArrival} {MATH(rowNumber+64)}",
                $"456 {DateTime.Now:dd.MM.yyyy} 15:25:00 66"
            },
            new object[]
            {
                "0x03^{NumberOfTrain}^ {StationsCut}^{TArrival:t}^{TDepart:t} 0x{MATH(rowNumber+64):X1}0bb",
                "0x03^456^ Питер^15:25^16:18 0x420bb"
            },
            new object[]
            {
                "0x{MATH((rowNumber+64)-(rowNumber*2)):X1}0bb",
                "0x3E0bb"
            },
            new object[]
            {
                "",
                ""
            },
            new object[]
            {
                "0x03^{NumberOfTrain}^ {Note}^{Event}^{DelayTime:t} {} {DelayTime:t} {PathNumber}",
                "0x03^456^ Станция 1,Станция 2,Станция 3,Станция 4,Станция 5,Станция 6,Станция 7^Отправление^0:15 {} 0:15 5"
            },
            new object[]
            {
                "0FF065564457657876867",
                "0FF065564457657876867"
            },
        };
        #endregion
        [Theory]
        [MemberData(nameof(BoodyExecuteInseartsDatas))]
        public void BoodyExecuteInseartsTest(string str, string expectedStr)
        { 
            //Arrage
            var independentInsertsService = IndependentInsertsServiceFactory.CreateIndependentInsertsService(str,  _handlerFactorys, GetStringInsertModelExtDict.SimpleDictionary, _logger);

            //Act
            var adInputType = GetListAdInputType.ListInputType1Item.First();
            var headerExecuteInseartsResult = independentInsertsService.ExecuteInsearts(adInputType, new Dictionary<string, string> { { "rowNumber", "2" } }).result;

            var resStr = headerExecuteInseartsResult.ToString();

            //Assert
            resStr.Should().Be(expectedStr);
        }




        #region TheoryData
        public static IEnumerable<object[]> BoodyExecuteInseartsDatasWithStringMiddleware => new[]
        {
            //конвертор LimitStringConverterOption.
            new object[]
            {
                "0x03^{NumberOfTrain:MW_Limit(2)}^",
                "0x03^45^"
            },
            //конвертор LimitStringConverterOption и PadRightStringConverterOption.
            new object[]
            {
                "0x03^{NumberOfTrain:MW_Limit(2)}^ {StationsCut:MW_PadR(6)}^{TArrival:t_MW_PadR(10)}^{TDepart:t_MW_PadR(11)}0x{MATH(rowNumber+64):X1}0bb",
                "0x03^45^ Питер ^15:25     ^16:18      0x420bb"
            },
            //Цепочка конверторов.
            new object[]
            {
                "0x03^{NumberOfTrain:MW_Limit(2)->PadR(6)}",
                "0x03^45    "
            },
            //НЕ верный ключ для Ext.
            new object[]
            {
                "0x03^{NumberOfTrain:ErrorKey}",
                "0x03^!!!ExtKeyNotFound!!!" 
            }
        };
        #endregion
        [Theory]
        [MemberData(nameof(BoodyExecuteInseartsDatasWithStringMiddleware))]
        public void BoodyExecuteInsearts_With_StringMiddleware_Test(string str, string expectedStr)
        {
            //Arrage
            var independentInsertsService = IndependentInsertsServiceFactory.CreateIndependentInsertsService(str, _handlerFactorys, GetStringInsertModelExtDict.SimpleDictionary, _logger);

            //Act
            var adInputType = GetListAdInputType.ListInputType1Item.First();
            var headerExecuteInseartsResult = independentInsertsService.ExecuteInsearts(adInputType, new Dictionary<string, string> { { "rowNumber", "2" } }).result;

            var resStr = headerExecuteInseartsResult.ToString();

            //Assert
            resStr.Should().Be(expectedStr);
        }



        [Fact]
        public void BoodyExecuteInseartsNullTest()
        {
            //Arrage
            string str = null;

            //Act & Asert
            var exception = Assert.Throws<ArgumentNullException>(() => IndependentInsertsServiceFactory.CreateIndependentInsertsService(str, _handlerFactorys, GetStringInsertModelExtDict.SimpleDictionary, _logger));
            exception.Message.Should().Contain("Невозможно создать словарь вставок из NULL строки");
        }
    }
}