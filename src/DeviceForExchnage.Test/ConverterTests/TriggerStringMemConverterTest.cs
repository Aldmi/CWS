using System;
using System.Threading.Tasks;
using FluentAssertions;
using Shared.MiddleWares.Converters.StringConverters;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class TriggerStringMemConverterTest
    {
        private TriggerStringMemConverterOption Option { get; }
        public string InStr => "С остановками: Волочаевская";


        public TriggerStringMemConverterTest()
        {
            Option = new TriggerStringMemConverterOption
            {
                String4Reset = String.Empty,
                ResetTime = 2000
            };
        }


        [Fact]
        public void Init_Trigger()
        {
            //Arrage
            var converer = new TriggerStringMemConverter(Option);

            //Act
            var initRes=converer.Convert(InStr, 1);

            //Assert
            initRes.Should().Be(InStr);
        }


        [Fact]
        public async Task Trigger_Fired_With_OldData()
        {
            //Arrage
            var converer = new TriggerStringMemConverter(Option);

            //Act
            var initRes = converer.Convert(InStr, 1);
            await Task.Delay(3000);
            var resStep1 = converer.Convert(InStr, 1);
            await Task.Delay(3000);
            var resStep2 = converer.Convert(InStr, 1);

            //Assert
            initRes.Should().Be(InStr);
            resStep1.Should().Be(Option.String4Reset);
            resStep2.Should().Be(Option.String4Reset);
        }


        /// <summary>
        /// Каждый раз новые данные, хотя тригер уже сработал по времени. Трегр перезапускается от новых данных.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ResetTrigger_Always()
        {
            //Arrage
            var converer = new TriggerStringMemConverter(Option);
            var newInStr1 = "NewStr1";
            var newInStr2 = "NewStr2";

            //Act
            var initRes = converer.Convert(InStr, 1);
            await Task.Delay(3000);
            var resStep1 = converer.Convert(newInStr1, 1);
            await Task.Delay(3000);
            var resStep2 = converer.Convert(newInStr2, 1);

            //Assert
            initRes.Should().Be(InStr);
            resStep1.Should().Be(newInStr1);
            resStep2.Should().Be(newInStr2);
        }



        /// <summary>
        ///На 1 шаге новые данные перезапустили триггер.
        ///между 1 и 2 шагом триггер срабоитал.
        ///на 2 шаге поступили эти же данные (старые),
        ///вернули сброшенное значение.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Step1_NewData_TrigFired_Step2_OldData_TrigFired()
        {
            //Arrage
            var converer = new TriggerStringMemConverter(Option);
            var newInStr1 = "NewStr1";
            var newInStr2 = newInStr1;

            //Act
            var initRes = converer.Convert(InStr, 1);
            await Task.Delay(3000);
            var resStep1 = converer.Convert(newInStr1, 1);
            await Task.Delay(3000);
            var resStep2 = converer.Convert(newInStr2, 1);

            //Assert
            initRes.Should().Be(InStr);
            resStep1.Should().Be(newInStr1);
            resStep2.Should().Be(Option.String4Reset);
        }



        [Fact]
        public async Task Step1_NewData_TrigFired_Step2_OldData_TrigNotFired()
        {
            //Arrage
            var converer = new TriggerStringMemConverter(Option);
            var newInStr1 = "NewStr1";
            var newInStr2 = newInStr1;

            //Act
            var initRes = converer.Convert(InStr, 1);
            await Task.Delay(3000);
            var resStep1 = converer.Convert(newInStr1, 1);
            await Task.Delay(500);
            var resStep2 = converer.Convert(newInStr2, 1);

            //Assert
            initRes.Should().Be(InStr);
            resStep1.Should().Be(newInStr1);
            resStep2.Should().Be(newInStr1);
        }

    }
}