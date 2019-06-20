using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CSharpFunctionalExtensions;
using DAL.Abstract.Entities.Options.MiddleWare;
using DeviceForExchange.MiddleWares;
using DeviceForExchnage.Test.Datas;
using FluentAssertions;
using InputDataModel.Autodictor.Model;
using InputDataModel.Base;
using Moq;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace DeviceForExchnage.Test
{
    public class MiddleWareInDataTest
    {
        private readonly MiddleWareInDataOption option_OneStringHandler;
        private readonly MiddleWareInDataOption option_TwoStringHandler;
        private readonly MiddleWareInDataOption option_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter;
        private readonly ILogger _logger;


        public MiddleWareInDataTest()
        {
             option_OneStringHandler = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler("Note.NameRu");
             option_TwoStringHandler = GetMiddleWareInDataOption.GetMiddleWareInDataOption_TwoStringHandlers("Note.NameRu", "StationDeparture.NameRu");
             option_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter("Note.NameRu");

            //Loger Moq-----------------------------------------------
            var mock = new Mock<ILogger>();
             mock.Setup(loger => loger.Debug(""));
             mock.Setup(loger => loger.Error(""));
             mock.Setup(loger => loger.Warning(""));
             _logger = mock.Object;
        }



        [Fact]
        public void NormalUse_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note(1);
            var middleWareinData = new MiddleWareInData<AdInputType>(option_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var val = result.Value?.Data?.FirstOrDefault()?.Note.NameRu;

            //Asert
            result.IsSuccess.Should().BeTrue();
            val.Should().NotBeNull();
            val.Should().Be("Index= 0   Со всеми станциями кроие: Волочаевская, КлимскаяAfter InseartStringConverterAfter LimitStringComverter");
        }






        [Fact]
        public void  OnePropertyByStringType_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData(1);
            var middleWareinData = new MiddleWareInData<AdInputType>(option_OneStringHandler, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var val = result.Value?.Data?.FirstOrDefault();

            //Asert
            result.IsSuccess.Should().BeTrue();
            val.Should().NotBeNull();
            val.Note.NameRu.Should().Be("Index= 0   Со всеми станциями кроие: Волочаевская, КлимскаяAfter InseartStringConverterAfter LimitStringComverter");
        }


        [Fact]
        public void OnePropertyByStringType_ErrorPropName_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData(1);
            option_OneStringHandler.StringHandlers[0].PropName = "Note.xxxx";
            var middleWareinData = new MiddleWareInData<AdInputType>(option_OneStringHandler, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var errors = result.Error.GetErrors;

            //Asert
            result.IsSuccess.Should().BeFalse();
            result.Error.IsEmpty.Should().BeFalse();
            errors.Count.Should().Be(1);
            errors.FirstOrDefault().Should().Be("MiddlewareInvokeService.HandleInvoke Ошибка получения стркового свойства.  метаданные для xxxx не найдены");
        }


        [Fact]
        public void OnePropertyByStringType_ErrorPropName2_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData(1);
            option_OneStringHandler.StringHandlers[0].PropName = "ZZZ.xxxx";
            var middleWareinData = new MiddleWareInData<AdInputType>(option_OneStringHandler, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var errors = result.Error.GetErrors;

            //Asert
            result.IsSuccess.Should().BeFalse();
            result.Error.IsEmpty.Should().BeFalse();
            errors.Count.Should().Be(1);
            errors.FirstOrDefault().Should().Be("MiddlewareInvokeService.HandleInvoke Ошибка получения стркового свойства.  метаданные для ZZZ не найдены");
        }


        [Fact]
        public void ManyPropertyByStringType_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData(1000);
            var middleWareinData = new MiddleWareInData<AdInputType>(option_OneStringHandler, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var datas = result.Value?.Data;
            
            //Asert
            result.IsSuccess.Should().BeTrue();
            datas.Should().NotBeNull();
            datas.Count.Should().Be(1000);
            for (var i = 0; i < datas.Count; i++)
            {
                var data = datas[i];
                data.Note.NameRu.Should().Be($"Index= {i}   Со всеми станциями кроие: Волочаевская, КлимскаяAfter InseartStringConverterAfter LimitStringComverter");
            }
        }



        [Fact]
        public void ManyPropertyByStringType_ErrorPropName_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData(1000);
            option_OneStringHandler.StringHandlers[0].PropName = "Note.xxxx";
            var middleWareinData = new MiddleWareInData<AdInputType>(option_OneStringHandler, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var errors = result.Error.GetErrors;

            //Asert
            result.IsSuccess.Should().BeFalse();
            result.Error.IsEmpty.Should().BeFalse();
            errors.Count.Should().Be(1000);
            foreach (var error in errors)
            {
                error.Should().Be($"MiddlewareInvokeService.HandleInvoke Ошибка получения стркового свойства.  метаданные для xxxx не найдены");
            }
        }



        [Fact]
        public void ManyPropertyByStringType_TwoStringHandler_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData(1000);
            var middleWareinData = new MiddleWareInData<AdInputType>(option_TwoStringHandler, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var datas = result.Value?.Data;

            //Asert
            result.IsSuccess.Should().BeTrue();
            datas.Should().NotBeNull();
            datas.Count.Should().Be(1000);
            for (var i = 0; i < datas.Count; i++)
            {
                var data = datas[i];
                data.Note.NameRu.Should().Be($"Index= {i}   Со всеми станциями кроие: Волочаевская, КлимскаяAfter InseartStringConverterAfter LimitStringComverter");
                data.StationDeparture.NameRu.Should().Be($"Index= {i}    Станция Отпр 1After ReplaceEmptyStringConverter");
            }
        }



        [Fact]
        public void PropertyNoteEqualNull_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData(1);
            inData.Data.First().Note = null;
            var middleWareinData = new MiddleWareInData<AdInputType>(option_OneStringHandler, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var errors = result.Error.GetErrors;
            var error = errors.First();

            //Asert
            result.IsSuccess.Should().BeFalse();
            errors.Count.Should().Be(1);
            error.Should().Be("MiddlewareInvokeService.HandleInvoke Ошибка получения стркового свойства.  Родительский объект == Null. Note.NameRu. Невозможно обратится к свойству NameRu");
        }


        [Fact]
        public void PropertyNoteEqualNull_OptionErrorPropName_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData(1);
            inData.Data.First().Note = null;
            var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler("Note");
            var middleWareinData = new MiddleWareInData<AdInputType>(option, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var errors = result.Error.GetErrors;
            var error = errors.First();

            //Asert
            result.IsSuccess.Should().BeFalse();
            errors.Count.Should().Be(1);
            error.Should().Be("MiddlewareInvokeService.HandleInvoke Ошибка получения стркового свойства.  Тип свойства Note не соответвует типу обработчика handler System.String");
        }


        [Fact]
        public void PropertyNumberOfTrain_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData(1);
            var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler("NumberOfTrain");
            var middleWareinData = new MiddleWareInData<AdInputType>(option, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var data = result.Value?.Data.FirstOrDefault();

            //Asert
            result.IsSuccess.Should().BeTrue();
            data.NumberOfTrain.Should().Be("956After InseartStringConverterAfter LimitStringComverter");
        }



        [Fact]
        public void PropertyNumberOfTrain_Null_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData(1);
            inData.Data.First().NumberOfTrain = null;
            var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler("NumberOfTrain");
            var middleWareinData = new MiddleWareInData<AdInputType>(option, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var data = result.Value?.Data.FirstOrDefault();

            //Asert
            result.IsSuccess.Should().BeTrue();
            data.Should().NotBeNull();
            data.NumberOfTrain.Should().BeNull();
        }
    }

}