using System;
using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;
using DeviceForExchange.MiddleWares.Converters.StringConverters;
using FluentAssertions;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class SubStringMemConverterTest
    {
        [Fact]
        public void NormalUse_5Step()
        {
            //Arrage
            var option = new SubStringMemConverterOption
            {
                Lenght = 25
            };
            var converer = new SubStringMemConverter(option);
            var str = "Со всеми остановками кроме: серпухово, балаково, Свободное";

            //Act
            var resStep1 = converer.Convert(str);
            var resStep2 = converer.Convert(str);
            var resStep3 = converer.Convert(str);
            var resStep4 = converer.Convert(str);
            var resStep5 = converer.Convert(str);

            //Asert
            resStep1.Should().Be("Со всеми остановками");
            resStep2.Should().Be("кроме: серпухово,");
            resStep3.Should().Be("балаково, Свободное");

            resStep4.Should().Be("Со всеми остановками");
            resStep5.Should().Be("кроме: серпухово,");
        }


        [Fact]
        public void SubStringLenghtHightStringLenght_3Step()
        {
            //Arrage
            var option = new SubStringMemConverterOption
            {
                Lenght = 150
            };
            var converer = new SubStringMemConverter(option);
            var str = "Со всеми остановками кроме: серпухово, балаково, Свободное";

            //Act
            var resStep1 = converer.Convert(str);
            var resStep2 = converer.Convert(str);
            var resStep3 = converer.Convert(str);

            //Asert
            resStep1.Should().Be(str);
            resStep2.Should().Be(str);
            resStep3.Should().Be(str);
        }


        [Fact]
        public void NewStringAfter2Step_5Step()
        {
            //Arrage
            var option = new SubStringMemConverterOption
            {
                Lenght = 25
            };
            var converer = new SubStringMemConverter(option);
            var str = "Со всеми остановками кроме: серпухово, балаково, Свободное";

            //Act
            var resStep1 = converer.Convert(str);
            var resStep2 = converer.Convert(str);
            str = "Новая строка 11 22 33 44 55 66 7777777 8888888";
            var resStep3 = converer.Convert(str);
            var resStep4 = converer.Convert(str);
            var resStep5 = converer.Convert(str);

            //Asert
            resStep1.Should().Be("Со всеми остановками");
            resStep2.Should().Be("кроме: серпухово,");

            resStep3.Should().Be("Новая строка 11 22 33 44");
            resStep4.Should().Be("55 66 7777777 8888888"); 
            resStep5.Should().Be("Новая строка 11 22 33 44"); 
        }


        [Fact]
        public void NewStringAfter1Step2Step3Step_7Step()
        {
            //Arrage
            var option = new SubStringMemConverterOption
            {
                Lenght = 25
            };
            var converer = new SubStringMemConverter(option);
            var str = "Со всеми остановками кроме: серпухово, балаково, Свободное";

            //Act
            var resStep1 = converer.Convert(str);
            str = "Новая строка После 1 шага";
            var resStep2 = converer.Convert(str);
            str = "Новая строка После 2 шага";
            var resStep3 = converer.Convert(str);
            str = "Новая строка После 3 шага. Эта строка будет обрабатываться долго";
            var resStep4 = converer.Convert(str);
            var resStep5 = converer.Convert(str);
            var resStep6 = converer.Convert(str);
            var resStep7 = converer.Convert(str);

            //Asert
            resStep1.Should().Be("Со всеми остановками");
            resStep2.Should().Be("Новая строка После 1");
            resStep3.Should().Be("Новая строка После 2");
            resStep4.Should().Be("Новая строка После 3");
            resStep5.Should().Be("шага. Эта строка будет");
            resStep6.Should().Be("обрабатываться долго");
            resStep7.Should().Be("Новая строка После 3");
        }


        [Fact]
        public void EmptyStringConvert()
        {
            //Arrage
            var option = new SubStringMemConverterOption
            {
                Lenght = 5
            };
            var converer = new SubStringMemConverter(option);
            var str = String.Empty;

            //Act
            var resStep1 = converer.Convert(str);
            var resStep2 = converer.Convert(str);
            var resStep3 = converer.Convert(str);

            //Asert
            resStep1.Should().Be(expected: string.Empty);
            resStep2.Should().Be(string.Empty);
            resStep3.Should().Be(string.Empty);
        }


        [Fact]
        public void NullStringConvert()
        {
            //Arrage
            var option = new SubStringMemConverterOption
            {
                Lenght = 25
            };
            var converer = new SubStringMemConverter(option);
            string str = null;

            //Act
            var resStep1 = converer.Convert(str);
            var resStep2 = converer.Convert(str);
            var resStep3 = converer.Convert(str);

            //Asert
            resStep1.Should().Be(expected:null);
            resStep2.Should().Be(null);
            resStep3.Should().Be(null);
        }
    }
}