using System.Linq;
using DAL.Abstract.Entities.Options.MiddleWare;
using DeviceForExchnage.Test.Datas;
using Domain.Device.MiddleWares;
using Domain.InputDataModel.Autodictor.Model;
using FluentAssertions;
using Moq;
using Serilog;
using Xunit;

namespace DeviceForExchnage.Test
{
    public class MiddleWareInDataTest
    {
         private readonly MiddleWareInDataOption _optionLimitStringHandler;
        private readonly MiddleWareInDataOption _optionTwoStringHandler;
        private readonly MiddleWareInDataOption _optionOneStringHandlerSubStringMemConverterInseartEndLineMarkerConverter;
        private readonly ILogger _logger;


        public MiddleWareInDataTest()
        {
             _optionLimitStringHandler = GetMiddleWareInDataOption.GetMiddleWareInDataOption_LimitStringConverter("Note.NameRu");
             _optionTwoStringHandler = GetMiddleWareInDataOption.GetMiddleWareInDataOption_TwoStringHandlers("Note.NameRu", "StationsСut.NameRu");
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
            var inData = InDataSourse.GetData(1);
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
            var inData = InDataSourse.GetData(1);
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
            var inData = InDataSourse.GetData(2);
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
            valStep1Data1.Should().Be("С остановками: Волочаевская 0, Климская0x090, Октябрьская 0, Новосибирская 0,0x09Красноярская 0, 25 Километр 0,");
            valStep1Data2.Should().Be("С остановками: Волочаевская 1, Климская0x091, Октябрьская 1, Новосибирская 1,0x09Красноярская 1, 25 Километр 1,"); 

            resStep2.IsSuccess.Should().BeTrue();
            valStep2Data1.Should().NotBeNull();
            valStep2Data2.Should().NotBeNull();
            valStep2Data1.Should().Be("С остановками: Волховские холмы 0,0x09Ленинско кузнецкие золотые сопки0x09верхней пыжмы 0, Куйбышевская 0,0x09Казахстанская 0,"); 
            valStep2Data2.Should().Be("С остановками: Волховские холмы 1,0x09Ленинско кузнецкие золотые сопки0x09верхней пыжмы 1, Куйбышевская 1,0x09Казахстанская 1,"); 

            resStep3.IsSuccess.Should().BeTrue();
            valStep3Data1.Should().NotBeNull();
            valStep3Data2.Should().NotBeNull();
            valStep3Data1.Should().Be("С остановками: Свердлолвская 0,0x09Московская 0, Горьковская 0"); 
            valStep3Data2.Should().Be("С остановками: Свердлолвская 1,0x09Московская 1, Горьковская 1");

            valStep4Data1.Should().NotBeNull();
            valStep4Data2.Should().NotBeNull();
            valStep4Data1.Should().Be("С остановками: Волочаевская 0, Климская0x090, Октябрьская 0, Новосибирская 0,0x09Красноярская 0, 25 Километр 0,");
            valStep4Data2.Should().Be("С остановками: Волочаевская 1, Климская0x091, Октябрьская 1, Новосибирская 1,0x09Красноярская 1, 25 Километр 1,");

            resStep5.IsSuccess.Should().BeTrue();
            valStep5Data1.Should().NotBeNull();
            valStep5Data2.Should().NotBeNull();
            valStep5Data1.Should().Be("С остановками: Волховские холмы 0,0x09Ленинско кузнецкие золотые сопки0x09верхней пыжмы 0, Куйбышевская 0,0x09Казахстанская 0,");
            valStep5Data2.Should().Be("С остановками: Волховские холмы 1,0x09Ленинско кузнецкие золотые сопки0x09верхней пыжмы 1, Куйбышевская 1,0x09Казахстанская 1,");

            resStep6.IsSuccess.Should().BeTrue();
            valStep6Data1.Should().NotBeNull();
            valStep6Data2.Should().NotBeNull();
            valStep6Data1.Should().Be("С остановками: Свердлолвская 0,0x09Московская 0, Горьковская 0");
            valStep6Data2.Should().Be("С остановками: Свердлолвская 1,0x09Московская 1, Горьковская 1");
        }


        [Fact]
        public void NormalUse_Note_2Data_StationDeparture_Convert_6Step()
        {
            //Arrage
            var inData = InDataSourse.GetData(2);
            var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_TwoStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter("Note.NameRu", "StationDeparture.NameRu");
            var middleWareinData = new MiddleWareInData<AdInputType>(option, _logger);

            //Act
            var resStep1 = middleWareinData.HandleInvoke(inData);
            var valNoteStep1Data1 = resStep1.Value?.Data[0].Note.NameRu;
            var valNoteStep1Data2 = resStep1.Value?.Data[1].Note.NameRu;
            var valStationDepartureStep1Data1 = resStep1.Value?.Data[0].StationDeparture.NameRu;
            var valStationDepartureStep1Data2 = resStep1.Value?.Data[1].StationDeparture.NameRu;

            var resStep2 = middleWareinData.HandleInvoke(inData);
            var valNoteStep2Data1 = resStep2.Value?.Data[0].Note.NameRu;
            var valNoteStep2Data2 = resStep2.Value?.Data[1].Note.NameRu;
            var valStationDepartureStep2Data1 = resStep2.Value?.Data[0].StationDeparture.NameRu;
            var valStationDepartureStep2Data2 = resStep2.Value?.Data[1].StationDeparture.NameRu;

            var resStep3 = middleWareinData.HandleInvoke(inData);
            var valNoteStep3Data1 = resStep3.Value?.Data[0].Note.NameRu;
            var valNoteStep3Data2 = resStep3.Value?.Data[1].Note.NameRu;
            var valStationDepartureStep3Data1 = resStep3.Value?.Data[0].StationDeparture.NameRu;
            var valStationDepartureStep3Data2 = resStep3.Value?.Data[1].StationDeparture.NameRu;

            var resStep4 = middleWareinData.HandleInvoke(inData);
            var valNoteStep4Data1 = resStep4.Value?.Data[0].Note.NameRu;
            var valNoteStep4Data2 = resStep4.Value?.Data[1].Note.NameRu;
            var valStationDepartureStep4Data1 = resStep4.Value?.Data[0].StationDeparture.NameRu;
            var valStationDepartureStep4Data2 = resStep4.Value?.Data[1].StationDeparture.NameRu;

            var resStep5 = middleWareinData.HandleInvoke(inData);
            var valNoteStep5Data1 = resStep5.Value?.Data[0].Note.NameRu;
            var valNoteStep5Data2 = resStep5.Value?.Data[1].Note.NameRu;
            var valStationDepartureStep5Data1 = resStep5.Value?.Data[0].StationDeparture.NameRu;
            var valStationDepartureStep5Data2 = resStep5.Value?.Data[1].StationDeparture.NameRu;

            var resStep6 = middleWareinData.HandleInvoke(inData);
            var valNoteStep6Data1 = resStep6.Value?.Data[0].Note.NameRu;
            var valNoteStep6Data2 = resStep6.Value?.Data[1].Note.NameRu;
            var valStationDepartureStep6Data1 = resStep6.Value?.Data[0].StationDeparture.NameRu;
            var valStationDepartureStep6Data2 = resStep6.Value?.Data[1].StationDeparture.NameRu;

            //Asert
            resStep1.IsSuccess.Should().BeTrue();
            valNoteStep1Data1.Should().Be("С остановками: Волочаевская 0, Климская0x090, Октябрьская 0, Новосибирская 0,0x09Красноярская 0,");
            valNoteStep1Data2.Should().Be("С остановками: Волочаевская 1, Климская0x091, Октябрьская 1, Новосибирская 1,0x09Красноярская 1,");
            valStationDepartureStep1Data1.Should().Be("Станция0x09Отпр 0");
            valStationDepartureStep1Data2.Should().Be("Станция0x09Отпр 1");

            resStep2.IsSuccess.Should().BeTrue();
            valNoteStep2Data1.Should().Be("С остановками: 25 Километр 0,0x09Волховские холмы 0, Ленинско кузнецкие0x09золотые сопки верхней пыжмы 0,");
            valNoteStep2Data2.Should().Be("С остановками: 25 Километр 1,0x09Волховские холмы 1, Ленинско кузнецкие0x09золотые сопки верхней пыжмы 1,");
            valStationDepartureStep2Data1.Should().Be("Станция0x09Отпр 0");
            valStationDepartureStep2Data2.Should().Be("Станция0x09Отпр 1");

            resStep3.IsSuccess.Should().BeTrue();
            valNoteStep3Data1.Should().Be("С остановками: Куйбышевская 0,0x09Казахстанская 0, Свердлолвская 0,0x09Московская 0, Горьковская 0");
            valNoteStep3Data2.Should().Be("С остановками: Куйбышевская 1,0x09Казахстанская 1, Свердлолвская 1,0x09Московская 1, Горьковская 1");
            valStationDepartureStep3Data1.Should().Be("Станция0x09Отпр 0");
            valStationDepartureStep3Data2.Should().Be("Станция0x09Отпр 1");

            resStep4.IsSuccess.Should().BeTrue();
            valNoteStep4Data1.Should().Be("С остановками: Волочаевская 0, Климская0x090, Октябрьская 0, Новосибирская 0,0x09Красноярская 0,");
            valNoteStep4Data2.Should().Be("С остановками: Волочаевская 1, Климская0x091, Октябрьская 1, Новосибирская 1,0x09Красноярская 1,");
            valStationDepartureStep4Data1.Should().Be("Станция0x09Отпр 0");
            valStationDepartureStep4Data2.Should().Be("Станция0x09Отпр 1");

            resStep5.IsSuccess.Should().BeTrue();
            valNoteStep5Data1.Should().Be("С остановками: 25 Километр 0,0x09Волховские холмы 0, Ленинско кузнецкие0x09золотые сопки верхней пыжмы 0,");
            valNoteStep5Data2.Should().Be("С остановками: 25 Километр 1,0x09Волховские холмы 1, Ленинско кузнецкие0x09золотые сопки верхней пыжмы 1,");
            valStationDepartureStep5Data1.Should().Be("Станция0x09Отпр 0");
            valStationDepartureStep5Data2.Should().Be("Станция0x09Отпр 1");

            resStep6.IsSuccess.Should().BeTrue();
            valNoteStep6Data1.Should().Be("С остановками: Куйбышевская 0,0x09Казахстанская 0, Свердлолвская 0,0x09Московская 0, Горьковская 0");
            valNoteStep6Data2.Should().Be("С остановками: Куйбышевская 1,0x09Казахстанская 1, Свердлолвская 1,0x09Московская 1, Горьковская 1");
            valStationDepartureStep6Data1.Should().Be("Станция0x09Отпр 0");
            valStationDepartureStep6Data2.Should().Be("Станция0x09Отпр 1");

        }


        [Fact]
        public void SubStringLenghtHightStringLenght_3Step()
        {
            //Arrage
            //Arrage
            var inData = InDataSourse.GetData(1);
            var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter("Note.NameRu");
            option.StringHandlers[0].SubStringMemConverterOption.Lenght = 500;
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
            var assertStr = "С остановками: Волочаевская 0, Климская0x090, Октябрьская 0, Новосибирская 0,0x09Красноярская 0, 25 Километр 0,0x09Волховские холмы 0, Ленинско кузнецкие0x09золотые сопки верхней пыжмы 0,0x09Куйбышевская 0, Казахстанская 0,0x09Свердлолвская 0, Московская 0,0x09Горьковская 0";

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
            var inData = InDataSourse.GetData(1);
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
            inData = InDataSourse.GetData(1);
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
            valStep3.Should().Be("С остановками: Жуковская 0, Новые0x09пушистые холмы 0, Кемеровская 0,0x09Медвежий остров 0, Собачья пасть 0,0x09Кенгуровый зуб 0,");

            resStep4.IsSuccess.Should().BeTrue();
            valStep4.Should().NotBeNull();
            valStep4.Should().Be("С остановками: Харьковская 0");

            resStep5.IsSuccess.Should().BeTrue();
            valStep5.Should().NotBeNull();
            valStep5.Should().Be("С остановками: Жуковская 0, Новые0x09пушистые холмы 0, Кемеровская 0,0x09Медвежий остров 0, Собачья пасть 0,0x09Кенгуровый зуб 0,");

            resStep6.IsSuccess.Should().BeTrue();
            valStep6.Should().NotBeNull();
            valStep6.Should().Be("С остановками: Волочаевская 0, Климская0x090, Октябрьская 0, Новосибирская 0,0x09Красноярская 0, 25 Километр 0,");
        }


        [Fact]
        public void EmptyNote_3Step()
        {
            //Arrage
            var inData = InDataSourse.GetData(1);
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
            var assertError ="MiddlewareInvokeService.HandleInvoke.StringConvert.  Ошибка получения стркового свойства:  Родительский объект == Null. Note.NameRu. Невозможно обратится к свойству NameRu";
            resStep1.IsSuccess.Should().BeFalse();
            errorStep1.Should().NotBeNull();
            errorStep1.Should().Be(assertError);

            resStep2.IsSuccess.Should().BeFalse();
            errorStep2.Should().NotBeNull();
            errorStep2.Should().Be(assertError);

            resStep3.IsSuccess.Should().BeFalse();
            errorStep3.Should().NotBeNull();
            errorStep3.Should().Be(assertError);
        }


        [Fact]
        public void OnePropertyByStringType_ErrorPropName_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData(1);
            _optionLimitStringHandler.StringHandlers[0].PropName = "Note.xxxx";
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionLimitStringHandler, _logger);

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
            var inData = InDataSourse.GetData(1);
            _optionLimitStringHandler.StringHandlers[0].PropName = "ZZZ.xxxx";
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionLimitStringHandler, _logger);

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
            var countData = 9;
            var inData = InDataSourse.GetData(countData);
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionLimitStringHandler, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var datas = result.Value?.Data;
            
            //Asert
            result.IsSuccess.Should().BeTrue();
            datas.Should().NotBeNull();
            datas.Count.Should().Be(countData);
            for (var i = 0; i < datas.Count; i++)
            {
                var data = datas[i];
                data.Note.NameRu.Should().Be($"С остановками: Волочаевская {i}, Климская {i}, Октябрьская {i}, Новосибирская {i}, Красн");
            }
        }


        [Fact]
        public void ManyPropertyByStringType_ErrorPropName_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData(1000);
            _optionLimitStringHandler.StringHandlers[0].PropName = "Note.xxxx";
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionLimitStringHandler, _logger);

            //Act
            var result = middleWareinData.HandleInvoke(inData);
            var errors = result.Error.GetErrors;

            //Asert
            result.IsSuccess.Should().BeFalse();
            result.Error.IsEmpty.Should().BeFalse();
            errors.Count.Should().Be(1000);
            foreach (var error in errors)
            {
                error.Should().Be($"MiddlewareInvokeService.HandleInvoke.StringConvert.  Ошибка получения стркового свойства:  метаданные для xxxx не найдены");
            }
        }


        [Fact]
        public void ManyPropertyByStringType_TwoStringHandler_Test()
        {
            //Arrage
            var numberOfData =4;
            var inData = InDataSourse.GetData_Stations_Null(numberOfData);
            var middleWareinData = new MiddleWareInData<AdInputType>(_optionTwoStringHandler, _logger);

            for (int step = 0; step < 5; step++)
            {
                //Act
                var result = middleWareinData.HandleInvoke(inData);
                var datas = result.Value?.Data;

                //Asert
                result.IsSuccess.Should().BeTrue();
                datas.Count.Should().Be(numberOfData);
                for (var i = 0; i < datas.Count; i++)
                {
                    var data = datas[i];

                    switch (step)
                    {
                        case 0:
                            data.Note.NameRu.Should().Be($"С остановками: Волочаевская {i}, Климская0x09{i}, Октябрьская {i}, Новосибирская {i},0x09Красноярская {i}, 25 Километр {i},");
                            break;
                        case 1:
                            data.Note.NameRu.Should().Be($"С остановками: Волховские холмы {i},0x09Ленинско кузнецкие золотые сопки0x09верхней пыжмы {i}, Куйбышевская {i},0x09Казахстанская {i},");
                            break;
                        case 2:
                            data.Note.NameRu.Should().Be($"С остановками: Свердлолвская {i},0x09Московская {i}, Горьковская {i}");
                            break;
                        case 3:
                            data.Note.NameRu.Should().Be($"С остановками: Волочаевская {i}, Климская0x09{i}, Октябрьская {i}, Новосибирская {i},0x09Красноярская {i}, 25 Километр {i},");
                            break;
                        case 4:
                            data.Note.NameRu.Should().Be($"С остановками: Волховские холмы {i},0x09Ленинско кузнецкие золотые сопки0x09верхней пыжмы {i}, Куйбышевская {i},0x09Казахстанская {i},");
                            break;
                    }

                    data.StationsСut.NameRu.Should().Be("Посадки нет");
                }
            }

        }


        [Fact]
        public void PropertyNoteEqualNull_OptionErrorPropName_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData_WithoutNote(1);
            var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_LimitStringConverter("Note");
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



        [Fact]
        public void PropertyNumberOfTrain_Null_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData_NumberOfTrain_Null(1);
           // inData.Data.First().NumberOfTrain = null;
            var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_LimitStringConverter("NumberOfTrain");
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
            var inData = InDataSourse.GetData_Note_LongWord(3); //3
            var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler_SubStringMemConverter_InseartEndLineMarkerConverter_Khazanskiy("Note.NameRu");
            var middleWareinData = new MiddleWareInData<AdInputType>(option, _logger);

            //Act
            var resStep1 = middleWareinData.HandleInvoke(inData);
            var valStep1Data1 = resStep1.Value?.Data[0].Note.NameRu;
            var valStep1Data2 = resStep1.Value?.Data[1].Note.NameRu;
            var valStep1Data3 = resStep1.Value?.Data[2].Note.NameRu;

            var resStep2 = middleWareinData.HandleInvoke(inData);
            var valStep2Data1 = resStep2.Value?.Data[0].Note.NameRu;
            var valStep2Data2 = resStep2.Value?.Data[1].Note.NameRu;
            var valStep2Data3 = resStep2.Value?.Data[2].Note.NameRu;

            var resStep3 = middleWareinData.HandleInvoke(inData);
            var valStep3Data1 = resStep3.Value?.Data[0].Note.NameRu;
            var valStep3Data2 = resStep3.Value?.Data[1].Note.NameRu;
            var valStep3Data3 = resStep3.Value?.Data[2].Note.NameRu;

            //Asert
            resStep1.IsSuccess.Should().BeTrue();
            valStep1Data1.Should().Be("Без остановок:0x09Трофимово0,0x09Воскресенск0, Шиферная0,0x09Москворецкая0,0x09Цемгигант0, Пески0,");
            valStep1Data2.Should().Be("Без остановок:0x09Трофимово1,0x09Воскресенск1, Шиферная1,0x09Москворецкая1,0x09Цемгигант1, Пески1,");
            valStep1Data3.Should().Be("Без остановок:0x09Трофимово2,0x09Воскресенск2, Шиферная2,0x09Москворецкая2,0x09Цемгигант2, Пески2,");

            resStep2.IsSuccess.Should().BeTrue();
            valStep2Data1.Should().Be("Без остановок: Золотая0,0x09Конев Бор0, Хорошово0,0x09Весенняя0, Сказочная0,0x09Платформа 113 км0,");
            valStep2Data2.Should().Be("Без остановок: Золотая1,0x09Конев Бор1, Хорошово1,0x09Весенняя1, Сказочная1,0x09Платформа 113 км1,");
            valStep2Data3.Should().Be("Без остановок: Золотая2,0x09Конев Бор2, Хорошово2,0x09Весенняя2, Сказочная2,0x09Платформа 113 км2,");

            resStep3.IsSuccess.Should().BeTrue();
            valStep3Data1.Should().Be("Без остановок: Коломна0");
            valStep3Data2.Should().Be("Без остановок: Коломна1");
            valStep3Data3.Should().Be("Без остановок: Коломна2");
        }


        [Fact]
        public void PropertyStationCut_NotEmpty_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData_StationCut(1); //3
            var option =GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler_ReplaceEmptyStringConverterTest("StationsСut.NameRu");
            var middleWareinData = new MiddleWareInData<AdInputType>(option, _logger);

            //Act
            var resStep1 = middleWareinData.HandleInvoke(inData);
            var valStep1Data1 = resStep1.Value?.Data[0].StationsСut.NameRu;


            //Assert
            valStep1Data1.Should().Be("Станция Отпр 0-Станция Пртб 0");
        }


        [Fact]
        public void PropertyStationCut_Empty_Test()
        {
            //Arrage
            var inData = InDataSourse.GetData_Stations_Null(1); //3
            var option = GetMiddleWareInDataOption.GetMiddleWareInDataOption_OneStringHandler_ReplaceEmptyStringConverterTest("StationsСut.NameRu");
            var middleWareinData = new MiddleWareInData<AdInputType>(option, _logger);

            //Act
            var resStep1 = middleWareinData.HandleInvoke(inData);
            var valStep1Data1 = resStep1.Value?.Data[0].StationsСut.NameRu;

            //Assert
            valStep1Data1.Should().Be("ПОСАДКИ НЕТ");
        }

    }

}