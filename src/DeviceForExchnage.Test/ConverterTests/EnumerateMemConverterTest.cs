using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core.Exceptions;
using Domain.Device.MiddleWares.Converters.EnumsConverters;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption.EnumsConvertersOption;
using Domain.InputDataModel.Autodictor.Entities;
using FluentAssertions;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class EnumerateMemConverterTest
    {
        private EnumMemConverterOption Option { get; }
        public Lang InLang => Lang.Ru;


        public EnumerateMemConverterTest()
        {
            Option = new EnumMemConverterOption()
            {
                DictChain = new Dictionary<string, int>
                 {
                     {"Ru", 3},
                     {"Eng", 2},
                     {"Fin", 1}
                 }
            };
        }



        [Fact]
        public void NormalUse_20Step()
        {
            //Arrage
            var converer = new EnumerateConverter(Option);

            //Act
            var resStep1 = converer.Convert(InLang, 1);
            var resStep2 = converer.Convert(InLang, 1);
            var resStep3 = converer.Convert(InLang, 1);
            var resStep4 = converer.Convert(InLang, 1);
            var resStep5 = converer.Convert(InLang, 1);
            var resStep6 = converer.Convert(InLang, 1);
            var resStep7 = converer.Convert(InLang, 1);
            var resStep8 = converer.Convert(InLang, 1);
            var resStep9 = converer.Convert(InLang, 1);
            var resStep10 = converer.Convert(InLang, 1);
            var resStep11 = converer.Convert(InLang, 1);
            var resStep12 = converer.Convert(InLang, 1);
            var resStep13 = converer.Convert(InLang, 1);
            var resStep14 = converer.Convert(InLang, 1);
            var resStep15 = converer.Convert(InLang, 1);
            var resStep16 = converer.Convert(InLang, 1);
            var resStep17 = converer.Convert(InLang, 1);
            var resStep18 = converer.Convert(InLang, 1);
            var resStep19 = converer.Convert(InLang, 1);
            var resStep20 = converer.Convert(InLang, 1);

            //Asert
            resStep1.Should().Be(Lang.Ru);
            resStep2.Should().Be(Lang.Ru);
            resStep3.Should().Be(Lang.Ru);
            resStep4.Should().Be(Lang.Eng);
            resStep5.Should().Be(Lang.Eng);
            resStep6.Should().Be(Lang.Fin);
            resStep7.Should().Be(Lang.Ru);
            resStep8.Should().Be(Lang.Ru);
            resStep9.Should().Be(Lang.Ru);
            resStep10.Should().Be(Lang.Eng);
            resStep11.Should().Be(Lang.Eng);
            resStep12.Should().Be(Lang.Fin);
            resStep13.Should().Be(Lang.Ru);
            resStep14.Should().Be(Lang.Ru);
            resStep15.Should().Be(Lang.Ru);
            resStep16.Should().Be(Lang.Eng);
            resStep17.Should().Be(Lang.Eng);
            resStep18.Should().Be(Lang.Fin);
            resStep19.Should().Be(Lang.Ru);
            resStep20.Should().Be(Lang.Ru);
        }


        [Fact]
        public void Exception_DictChain_Key()
        {
            //Arrage
            var option = new EnumMemConverterOption()
            {
                DictChain = new Dictionary<string, int>
                {
                    {"hhh", 3},
                    {"Eng", 2},
                    {"Fin", 1}
                }
            };
            var converer = new EnumerateConverter(option);

            //Act & Asert
            var exception = Assert.Throws<ParseException>(() => converer.Convert(InLang, 1));
            exception.Should().BeOfType<ParseException>();
            exception.Message.Should().Contain("В типе Enum Domain.InputDataModel.Autodictor.Entities.Lang не найдено значение перечисления hhh");
        }
    }
}