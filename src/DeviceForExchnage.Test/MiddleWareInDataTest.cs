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
        private readonly MiddleWareInDataOption _optionOneStringHandler;
        private readonly MiddleWareInDataOption _optionTwoStringHandler;
        private readonly MiddleWareInDataOption _optionOneStringHandlerSubStringMemConverterInseartEndLineMarkerConverter;
        private readonly ILogger _logger;


        public MiddleWareInDataTest()
        {
             _optionOneStringHandler = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler("Note.NameRu");
             _optionTwoStringHandler = GetMiddleWareInDataOption.GetMiddleWareInDataOption_TwoStringHandlers("Note.NameRu", "StationDeparture.NameRu");
             _optionOneStringHandlerSubStringMemConverterInseartEndLineMarkerConverter = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter("Note.NameRu");

            //Loger Moq-----------------------------------------------
            var mock = new Mock<ILogger>();
             mock.Setup(loger => loger.Debug(""));
             mock.Setup(loger => loger.Error(""));
             mock.Setup(loger => loger.Warning(""));
             _logger = mock.Object;
        }



        [Fact]
        public void NormalUse_Note_Convert_6Step()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note(1);
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionOneStringHandlerSubStringMemConverterInseartEndLineMarkerConverter, _logger);

            //Act
            var resStep1 = middleWareinData.HandleInvoke(inData);
            var valStep1 = resStep1.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep2 = middleWareinData.HandleInvoke(inData);
            var valStep2 = resStep2.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep3 = middleWareinData.HandleInvoke(inData);
            var valStep3 = resStep3.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep4 = middleWareinData.HandleInvoke(inData);
            var valStep4 = resStep4.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep5 = middleWareinData.HandleInvoke(inData);
            var valStep5 = resStep5.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep6 = middleWareinData.HandleInvoke(inData);
            var valStep6 = resStep6.Value?.Data?.FirstOrDefault()?.Note.NameRu;

            //Asert
            resStep1.IsSuccess.Should().BeTrue();
            valStep1.Should().NotBeNull();
            valStep1.Should().Be("С остановками: Волочаевская 0, Климская0x090, Октябрьская 0, Новосибирская 0,0x09Красноярская 0, 25 Километр 0,");

            resStep2.IsSuccess.Should().BeTrue();
            valStep2.Should().NotBeNull();
            valStep2.Should().Be("С остановками: Волховские холмы 0,0x09Ленинско кузнецкие золотые сопки0x09верхней пыжмы 0, Куйбышевская 0,0x09Казахстанская 0,");

            resStep3.IsSuccess.Should().BeTrue();
            valStep3.Should().NotBeNull();
            valStep3.Should().Be("С остановками: Свердлолвская 0,0x09Московская 0, Горьковская 0");

            resStep4.IsSuccess.Should().BeTrue();
            valStep4.Should().NotBeNull();
            valStep4.Should().Be("С остановками: Волочаевская 0, Климская0x090, Октябрьская 0, Новосибирская 0,0x09Красноярская 0, 25 Километр 0,");

            resStep5.IsSuccess.Should().BeTrue();
            valStep5.Should().NotBeNull();
            valStep5.Should().Be("С остановками: Волховские холмы 0,0x09Ленинско кузнецкие золотые сопки0x09верхней пыжмы 0, Куйбышевская 0,0x09Казахстанская 0,");

            resStep6.IsSuccess.Should().BeTrue();
            valStep6.Should().NotBeNull();
            valStep6.Should().Be("С остановками: Свердлолвская 0,0x09Московская 0, Горьковская 0");
        }


        [Fact]
        public void NormalUse_Note_Convert_WithOutPhrases_6Step()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note(1);
            var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter_withoutPhrases("Note.NameRu");
            var middleWareinData = new MiddleWareInData<AdInputType>(option, _logger);

            //Act
            var resStep1 = middleWareinData.HandleInvoke(inData);
            var valStep1 = resStep1.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep2 = middleWareinData.HandleInvoke(inData);
            var valStep2 = resStep2.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep3 = middleWareinData.HandleInvoke(inData);
            var valStep3 = resStep3.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep4 = middleWareinData.HandleInvoke(inData);
            var valStep4 = resStep4.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep5 = middleWareinData.HandleInvoke(inData);
            var valStep5 = resStep5.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep6 = middleWareinData.HandleInvoke(inData);
            var valStep6 = resStep6.Value?.Data?.FirstOrDefault()?.Note.NameRu;

            //Asert
            resStep1.IsSuccess.Should().BeTrue();
            valStep1.Should().NotBeNull();
            valStep1.Should().Be("С остановками: Волочаевская 0, Климская0x090, Октябрьская 0, Новосибирская 0,0x09Красноярская 0, 25 Километр 0,"); 

            resStep2.IsSuccess.Should().BeTrue();
            valStep2.Should().NotBeNull();
            valStep2.Should().Be("Волховские холмы 0, Ленинско кузнецкие0x09золотые сопки верхней пыжмы 0,0x09Куйбышевская 0, Казахстанская 0,0x09Свердлолвская 0,"); 

            resStep3.IsSuccess.Should().BeTrue();
            valStep3.Should().NotBeNull();
            valStep3.Should().Be("Московская 0, Горьковская 0"); 

            resStep4.IsSuccess.Should().BeTrue();
            valStep4.Should().NotBeNull();
            valStep4.Should().Be("С остановками: Волочаевская 0, Климская0x090, Октябрьская 0, Новосибирская 0,0x09Красноярская 0, 25 Километр 0,"); 

            resStep5.IsSuccess.Should().BeTrue();
            valStep5.Should().NotBeNull();
            valStep5.Should().Be("Волховские холмы 0, Ленинско кузнецкие0x09золотые сопки верхней пыжмы 0,0x09Куйбышевская 0, Казахстанская 0,0x09Свердлолвская 0,"); 

            resStep6.IsSuccess.Should().BeTrue();
            valStep6.Should().NotBeNull();
            valStep6.Should().Be("Московская 0, Горьковская 0"); 
        }



        [Fact]
        public void NormalUse_Note_2Data_Convert_6Step()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note(2);
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionOneStringHandlerSubStringMemConverterInseartEndLineMarkerConverter, _logger);

            //Act
            var resStep1 = middleWareinData.HandleInvoke(inData);
            var valStep1Data1 = resStep1.Value?.Data[0].Note.NameRu;
            var valStep1Data2 = resStep1.Value?.Data[1].Note.NameRu;

            var resStep2 = middleWareinData.HandleInvoke(inData);
            var valStep2Data1 = resStep2.Value?.Data[0].Note.NameRu;
            var valStep2Data2 = resStep2.Value?.Data[1].Note.NameRu;

            var resStep3 = middleWareinData.HandleInvoke(inData);
            var valStep3Data1 = resStep3.Value?.Data[0].Note.NameRu;
            var valStep3Data2 = resStep3.Value?.Data[1].Note.NameRu;

            var resStep4 = middleWareinData.HandleInvoke(inData);
            var valStep4Data1 = resStep4.Value?.Data[0].Note.NameRu;
            var valStep4Data2 = resStep4.Value?.Data[1].Note.NameRu;

            var resStep5 = middleWareinData.HandleInvoke(inData);
            var valStep5Data1 = resStep5.Value?.Data[0].Note.NameRu;
            var valStep5Data2 = resStep5.Value?.Data[1].Note.NameRu;

            var resStep6 = middleWareinData.HandleInvoke(inData);
            var valStep6Data1 = resStep6.Value?.Data[0].Note.NameRu;
            var valStep6Data2 = resStep6.Value?.Data[1].Note.NameRu;

            //Asert
            resStep1.IsSuccess.Should().BeTrue();
            valStep1Data1.Should().NotBeNull();
            valStep1Data2.Should().NotBeNull();
            valStep1Data1.Should().Be("Со всеми станциями кроме:0x09Волочаевская0, Климская0, Октябрьская0,0x09Новосибирская0, Красноярская0,");
            valStep1Data2.Should().Be("Со всеми станциями кроме:0x09Волочаевская1, Климская1, Октябрьская1,0x09Новосибирская1, Красноярская1,"); 

            resStep2.IsSuccess.Should().BeTrue();
            valStep2Data1.Should().NotBeNull();
            valStep2Data2.Should().NotBeNull();
            valStep2Data1.Should().Be("Куйбышевская0, Казахстанская0,0x09Свердлолвская0, Московская0,0x09Горьковская0, Сочинская0, Маковая0,"); 
            valStep2Data2.Should().Be("Куйбышевская1, Казахстанская1,0x09Свердлолвская1, Московская1,0x09Горьковская1, Сочинская1, Маковая1,"); 

            resStep3.IsSuccess.Should().BeTrue();
            valStep3Data1.Should().NotBeNull();
            valStep3Data2.Should().NotBeNull();
            valStep3Data1.Should().Be("Красносельская0, Уютная0, Петушки0"); 
            valStep3Data2.Should().Be("Красносельская1, Уютная1, Петушки1");

            valStep4Data1.Should().NotBeNull();
            valStep4Data2.Should().NotBeNull();
            valStep4Data1.Should().Be("Со всеми станциями кроме:0x09Волочаевская0, Климская0, Октябрьская0,0x09Новосибирская0, Красноярская0,");
            valStep4Data2.Should().Be("Со всеми станциями кроме:0x09Волочаевская1, Климская1, Октябрьская1,0x09Новосибирская1, Красноярская1,");

            resStep5.IsSuccess.Should().BeTrue();
            valStep5Data1.Should().NotBeNull();
            valStep5Data2.Should().NotBeNull();
            valStep5Data1.Should().Be("Куйбышевская0, Казахстанская0,0x09Свердлолвская0, Московская0,0x09Горьковская0, Сочинская0, Маковая0,");
            valStep5Data2.Should().Be("Куйбышевская1, Казахстанская1,0x09Свердлолвская1, Московская1,0x09Горьковская1, Сочинская1, Маковая1,");

            resStep6.IsSuccess.Should().BeTrue();
            valStep6Data1.Should().NotBeNull();
            valStep6Data2.Should().NotBeNull();
            valStep6Data1.Should().Be("Красносельская0, Уютная0, Петушки0");
            valStep6Data2.Should().Be("Красносельская1, Уютная1, Петушки1");
        }




        [Fact]
        public void NormalUse_Note_StationDeparture_Convert_6Step()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note(1);
            var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_TwoStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter("Note.NameRu", "StationDeparture.NameRu");
            var middleWareinData = new MiddleWareInData<AdInputType>(option, _logger);

            //Act
            var resStep1 = middleWareinData.HandleInvoke(inData);
            var valNoteStep1 = resStep1.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var valStationDepartureStep1 = resStep1.Value?.Data?.FirstOrDefault()?.StationDeparture.NameRu;

            var resStep2 = middleWareinData.HandleInvoke(inData);
            var valNoteStep2 = resStep2.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var valStationDepartureStep2 = resStep2.Value?.Data?.FirstOrDefault()?.StationDeparture.NameRu;

            var resStep3 = middleWareinData.HandleInvoke(inData);
            var valNoteStep3 = resStep3.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var valStationDepartureStep3 = resStep3.Value?.Data?.FirstOrDefault()?.StationDeparture.NameRu;

            var resStep4 = middleWareinData.HandleInvoke(inData);
            var valNoteStep4 = resStep4.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var valStationDepartureStep4 = resStep4.Value?.Data?.FirstOrDefault()?.StationDeparture.NameRu;

            var resStep5 = middleWareinData.HandleInvoke(inData);
            var valNoteStep5 = resStep5.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var valStationDepartureStep5 = resStep5.Value?.Data?.FirstOrDefault()?.StationDeparture.NameRu;

            var resStep6 = middleWareinData.HandleInvoke(inData);
            var valNoteStep6 = resStep6.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var valStationDepartureStep6 = resStep6.Value?.Data?.FirstOrDefault()?.StationDeparture.NameRu;

            //Asert
            resStep1.IsSuccess.Should().BeTrue();
            valNoteStep1.Should().Be("Index= 0   Со всеми станциями кроие:0x09Волочаевская, Климская, Октябрьская,0x09Новосибирская,");
            valStationDepartureStep1.Should().Be("Index= 0"); 

            resStep2.IsSuccess.Should().BeTrue();
            valNoteStep2.Should().Be("Красноярская, Куйбышевская,0x09Казахстанская, Свердлолвская,0x09Московская, Горьковская");
            valStationDepartureStep2.Should().Be("  Станция");

            resStep3.IsSuccess.Should().BeTrue();
            valNoteStep3.Should().Be("Index= 0   Со всеми станциями кроие:0x09Волочаевская, Климская, Октябрьская,0x09Новосибирская,");
            valStationDepartureStep3.Should().Be("Отпр 1");

            resStep4.IsSuccess.Should().BeTrue();
            valNoteStep4.Should().Be("Красноярская, Куйбышевская,0x09Казахстанская, Свердлолвская,0x09Московская, Горьковская");
            valStationDepartureStep4.Should().Be("Index= 0");

            resStep5.IsSuccess.Should().BeTrue();
            valNoteStep5.Should().Be("Index= 0   Со всеми станциями кроие:0x09Волочаевская, Климская, Октябрьская,0x09Новосибирская,");
            valStationDepartureStep5.Should().Be("  Станция");

            resStep6.IsSuccess.Should().BeTrue();
            valNoteStep6.Should().Be("Красноярская, Куйбышевская,0x09Казахстанская, Свердлолвская,0x09Московская, Горьковская");
            valStationDepartureStep6.Should().Be("Отпр 1");
        }


        [Fact]
        public void SubStringLenghtHightStringLenght_3Step()
        {
            //Arrage
            //Arrage
            var inData = InDataSourse.GetData_Note(1);
            var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter("Note.NameRu");
            option.StringHandlers[0].SubStringMemConverterOption.Lenght = 300;
            var middleWareinData = new MiddleWareInData<AdInputType>(option, _logger);

            //Act
            var resStep1 = middleWareinData.HandleInvoke(inData);
            var valStep1 = resStep1.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep2 = middleWareinData.HandleInvoke(inData);
            var valStep2 = resStep2.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep3 = middleWareinData.HandleInvoke(inData);
            var valStep3 = resStep3.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep4 = middleWareinData.HandleInvoke(inData);
            var valStep4 = resStep4.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep5 = middleWareinData.HandleInvoke(inData);
            var valStep5 = resStep5.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep6 = middleWareinData.HandleInvoke(inData);
            var valStep6 = resStep6.Value?.Data?.FirstOrDefault()?.Note.NameRu;

            //Asert
            var assertStr ="Index= 0   Со всеми станциями кроие:0x09Волочаевская, Климская, Октябрьская,0x09Новосибирская, Красноярская,0x09Куйбышевская, Казахстанская,0x09Свердлолвская, Московская, Горьковская";

            resStep1.IsSuccess.Should().BeTrue();
            valStep1.Should().NotBeNull();
            valStep1.Should().Be(assertStr);

            resStep2.IsSuccess.Should().BeTrue();
            valStep2.Should().NotBeNull();
            valStep1.Should().Be(assertStr);

            resStep3.IsSuccess.Should().BeTrue();
            valStep3.Should().NotBeNull();
            valStep1.Should().Be(assertStr);

            resStep4.IsSuccess.Should().BeTrue();
            valStep4.Should().NotBeNull();
            valStep1.Should().Be(assertStr);

            resStep5.IsSuccess.Should().BeTrue();
            valStep5.Should().NotBeNull();
            valStep1.Should().Be(assertStr);

            resStep6.IsSuccess.Should().BeTrue();
            valStep6.Should().NotBeNull();
            valStep1.Should().Be(assertStr);
        }


        [Fact]
        public void NewInDataAfter2StepAnd5Step_6Step()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note(1);
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionOneStringHandlerSubStringMemConverterInseartEndLineMarkerConverter, _logger);

            //Act
            var resStep1 = middleWareinData.HandleInvoke(inData);
            var valStep1 = resStep1.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep2 = middleWareinData.HandleInvoke(inData);
            var valStep2 = resStep2.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            inData = InDataSourse.GetData_NewNote(1);
            var resStep3 = middleWareinData.HandleInvoke(inData);
            var valStep3 = resStep3.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep4 = middleWareinData.HandleInvoke(inData);
            var valStep4 = resStep4.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep5 = middleWareinData.HandleInvoke(inData);
            var valStep5 = resStep5.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            inData = InDataSourse.GetData_Note(1);
            var resStep6 = middleWareinData.HandleInvoke(inData);
            var valStep6 = resStep6.Value?.Data?.FirstOrDefault()?.Note.NameRu;

            //Asert
            resStep1.IsSuccess.Should().BeTrue();
            valStep1.Should().NotBeNull();
            valStep1.Should().Be("Index= 0   Со всеми станциями кроие:0x09Волочаевская, Климская, Октябрьская,0x09Новосибирская,");

            resStep2.IsSuccess.Should().BeTrue();
            valStep2.Should().NotBeNull();
            valStep2.Should().Be("Красноярская, Куйбышевская,0x09Казахстанская, Свердлолвская,0x09Московская, Горьковская");

            resStep3.IsSuccess.Should().BeTrue();
            valStep3.Should().NotBeNull();
            valStep3.Should().Be("Index= 0 Поезд Следует без остановок");

            resStep4.IsSuccess.Should().BeTrue();
            valStep4.Should().NotBeNull();
            valStep4.Should().Be("Index= 0 Поезд Следует без остановок");

            resStep5.IsSuccess.Should().BeTrue();
            valStep5.Should().NotBeNull();
            valStep5.Should().Be("Index= 0 Поезд Следует без остановок");

            resStep6.IsSuccess.Should().BeTrue();
            valStep6.Should().NotBeNull();
            valStep6.Should().Be("Index= 0   Со всеми станциями кроие:0x09Волочаевская, Климская, Октябрьская,0x09Новосибирская,");
        }


        [Fact]
        public void EmptyNote_3Step()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note(1);
            inData.Data[0].Note.NameRu = string.Empty;
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionOneStringHandlerSubStringMemConverterInseartEndLineMarkerConverter, _logger);

            //Act
            var resStep1 = middleWareinData.HandleInvoke(inData);
            var valStep1 = resStep1.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep2 = middleWareinData.HandleInvoke(inData);
            var valStep2 = resStep2.Value?.Data?.FirstOrDefault()?.Note.NameRu;
            var resStep3 = middleWareinData.HandleInvoke(inData);
            var valStep3 = resStep3.Value?.Data?.FirstOrDefault()?.Note.NameRu;

            //Asert
            resStep1.IsSuccess.Should().BeTrue();
            valStep1.Should().NotBeNull();
            valStep1.Should().Be(string.Empty);

            resStep2.IsSuccess.Should().BeTrue();
            valStep2.Should().NotBeNull();
            valStep2.Should().Be(string.Empty);

            resStep3.IsSuccess.Should().BeTrue();
            valStep3.Should().NotBeNull();
            valStep3.Should().Be(string.Empty);
        }


        [Fact]
        public void NullNote_Error_3Step()
        {
            //Arrage
            var inData = InDataSourse.GetData_WithoutNote(1);
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionOneStringHandlerSubStringMemConverterInseartEndLineMarkerConverter, _logger);

            //Act
            var resStep1 = middleWareinData.HandleInvoke(inData);
            var errorStep1 = resStep1.Error.GetErrors.FirstOrDefault();
            var resStep2 = middleWareinData.HandleInvoke(inData);
            var errorStep2 = resStep2.Error.GetErrors.FirstOrDefault();
            var resStep3 = middleWareinData.HandleInvoke(inData);
            var errorStep3 = resStep3.Error.GetErrors.FirstOrDefault();

            //Asert
            resStep1.IsSuccess.Should().BeFalse();
            errorStep1.Should().NotBeNull();
            errorStep1.Should().Be("MiddlewareInvokeService.HandleInvoke.StringConvert.  Ошибка получения стркового свойства:  Родительский объект == Null. Note.NameRu. Невозможно обратится к свойству NameRu");

            resStep2.IsSuccess.Should().BeFalse();
            errorStep2.Should().NotBeNull();
            errorStep2.Should().Be("MiddlewareInvokeService.HandleInvoke.StringConvert.  Ошибка получения стркового свойства:  Родительский объект == Null. Note.NameRu. Невозможно обратится к свойству NameRu");

            resStep3.IsSuccess.Should().BeFalse();
            errorStep3.Should().NotBeNull();
            errorStep3.Should().Be("MiddlewareInvokeService.HandleInvoke.StringConvert.  Ошибка получения стркового свойства:  Родительский объект == Null. Note.NameRu. Невозможно обратится к свойству NameRu");
        }


        //-------------------------------------------------------------------------------







        [Fact]
        public void  OnePropertyByStringType_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note(1);
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionOneStringHandler, _logger);

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
            var inData = InDataSourse.GetData_Note(1);
            _optionOneStringHandler.StringHandlers[0].PropName = "Note.xxxx";
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionOneStringHandler, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var errors = result.Error.GetErrors;

            //Asert
            result.IsSuccess.Should().BeFalse();
            result.Error.IsEmpty.Should().BeFalse();
            errors.Count.Should().Be(1);
            errors.FirstOrDefault().Should().Be("MiddlewareInvokeService.HandleInvoke.StringConvert.  Ошибка получения стркового свойства:  метаданные для xxxx не найдены");
        }


        [Fact]
        public void OnePropertyByStringType_ErrorPropName2_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note(1);
            _optionOneStringHandler.StringHandlers[0].PropName = "ZZZ.xxxx";
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionOneStringHandler, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var errors = result.Error.GetErrors;

            //Asert
            result.IsSuccess.Should().BeFalse();
            result.Error.IsEmpty.Should().BeFalse();
            errors.Count.Should().Be(1);
            errors.FirstOrDefault().Should().Be("MiddlewareInvokeService.HandleInvoke.StringConvert.  Ошибка получения стркового свойства:  метаданные для ZZZ не найдены");
        }


        [Fact]
        public void ManyPropertyByStringType_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note(1000);
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionOneStringHandler, _logger);

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
            var inData = InDataSourse.GetData_Note(1000);
            _optionOneStringHandler.StringHandlers[0].PropName = "Note.xxxx";
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionOneStringHandler, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var errors = result.Error.GetErrors;

            //Asert
            result.IsSuccess.Should().BeFalse();
            result.Error.IsEmpty.Should().BeFalse();
            errors.Count.Should().Be(1000);
            foreach (var error in errors)  //"MiddlewareInvokeService.HandleInvoke.StringConvert.  Ошибка получения стркового свойства:  метаданные для xxxx не найдены"
            {
                error.Should().Be($"MiddlewareInvokeService.HandleInvoke.StringConvert.  Ошибка получения стркового свойства:  метаданные для xxxx не найдены");
            }
        }



        [Fact]
        public void ManyPropertyByStringType_TwoStringHandler_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note(1000);
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionTwoStringHandler, _logger);

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
        public void PropertyNoteEqualNull_OptionErrorPropName_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note(1);
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
            error.Should().Be("MiddlewareInvokeService.HandleInvoke.StringConvert.  Ошибка получения стркового свойства:  Тип свойства Note не соответвует типу обработчика handler System.String");
        }


        //[Fact]
        //public void PropertyNumberOfTrain_Test()
        //{
        //    //Arrage
        //    var inData = InDataSourse.GetData_Note(1);
        //    var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler("NumberOfTrain");
        //    var middleWareinData = new MiddleWareInData<AdInputType>(option, _logger);

        //    //Act
        //    var result = middleWareinData.HandleInvoke(inData);
        //    var data = result.Value?.Data.FirstOrDefault();

        //    //Asert
        //    result.IsSuccess.Should().BeTrue();
        //    data.NumberOfTrain.Should().Be("956After InseartStringConverterAfter LimitStringComverter");
        //}



        [Fact]
        public void PropertyNumberOfTrain_Null_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note(1);
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


        [Fact]
        public void PropertyNote_LongWord_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData_Note_LongWord(1); //3
            var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter_Khazanskiy("Note.NameRu");
            var middleWareinData = new MiddleWareInData<AdInputType>(option, _logger);

            //Act
            var resStep1 = middleWareinData.HandleInvoke(inData);
            var valStep1Data1 = resStep1.Value?.Data[0].Note.NameRu;
            //var valStep1Data2 = resStep1.Value?.Data[1].Note.NameRu;
            //var valStep1Data3 = resStep1.Value?.Data[2].Note.NameRu;

            //var resStep2 = middleWareinData.HandleInvoke(inData);
            //var valStep2Data1 = resStep2.Value?.Data[0].Note.NameRu;
            //var valStep2Data2 = resStep2.Value?.Data[1].Note.NameRu;
            //var valStep2Data3 = resStep2.Value?.Data[2].Note.NameRu;

            //var resStep3 = middleWareinData.HandleInvoke(inData);
            //var valStep3Data1 = resStep3.Value?.Data[0].Note.NameRu;
            //var valStep3Data2 = resStep3.Value?.Data[1].Note.NameRu;
            //var valStep3Data3 = resStep3.Value?.Data[2].Note.NameRu;

            
            
            

            
            //"Без остановок:0x09Золотая1, Конев Бор1,0x09Хорошово1,  Весенняя1,0x09Сказочная1,"
            //"Без остановок:0x09Золотая2, Конев Бор2,0x09Хорошово2,  Весенняя2,0x09Сказочная2,"

            //"Без остановок:0x09Платформа 113 км0,0x09Коломна0"
            //"Без остановок:0x09Платформа 113 км1,0x09Коломна1"
            //"Без остановок:0x09Платформа 113 км2,0x09Коломна2"

            //Asert
            resStep1.IsSuccess.Should().BeTrue();
            valStep1Data1.Should().Be("Без остановок:0x09Трофимово0,0x09Воскресенск0, Шиферная0,0x09Москворецкая0,0x09Цемгигант0, Пески0,");
            //valStep1Data2.Should().Be("Без остановок:0x09Трофимово1,0x09Воскресенск1, Шиферная1,0x09Москворецкая1,0x09Цемгигант1, Пески1,");
            //valStep1Data3.Should().Be("Без остановок:0x09Трофимово2,0x09Воскресенск2, Шиферная2,0x09Москворецкая2,0x09Цемгигант2, Пески2,");

            //resStep2.IsSuccess.Should().BeTrue();
            //valStep2Data1.Should().Be("Без остановок:0x09Золотая0, Конев Бор0,0x09Хорошово0,  Весенняя0,0x09Сказочная0,");
            //valStep2Data2.Should().Be("Без остановок:0x09Золотая1, Конев Бор1,0x09Хорошово1,  Весенняя1,0x09Сказочная1,");
            //valStep2Data3.Should().Be();
            
            //resStep3.IsSuccess.Should().BeTrue();
            //valStep3Data1.Should().Be();
            //valStep3Data2.Should().Be();
            //valStep3Data3.Should().Be();
        }

    }

}