using System;
using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;
using DeviceForExchange.MiddleWares.Converters.StringConverters;
using FluentAssertions;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class InseartEndLineMarkerConverterTest
    {
        [Fact]
        public void NormalUse()
        {
            //Arrage
            var option = new InseartEndLineMarkerConverterOption
            {
                LenghtLine = 30,
                Marker = "0x09"
                
            };
            var converer = new InseartEndLineMarkerConverter(option);
            var str = "Со всеми остановками кроме: серпухово, балаково, Свободное";

            //Act
            var res = converer.Convert(str,1);

            //Asert
            res.Should().Be("Со всеми остановками кроме:0x09серпухово, балаково,0x09Свободное");
        }


        [Fact]
        public void LineLenghHighterStrLenght()
        {
            //Arrage
            var option = new InseartEndLineMarkerConverterOption
            {
                LenghtLine = 100,
                Marker = "0x09"

            };
            var converer = new InseartEndLineMarkerConverter(option);
            var str = "Со всеми остановками кроме: серпухово, балаково, Свободное";

            //Act
            var res = converer.Convert(str, 1);

            //Asert
            res.Should().Be(str);
        }


        [Fact]
        public void LenghtLineEqual_3()
        {
            //Arrage
            var option = new InseartEndLineMarkerConverterOption
            {
                LenghtLine = 3,
                Marker = "0x09"

            };
            var converer = new InseartEndLineMarkerConverter(option);
            var str = "Строка1 Строка2 Строка3";

            //Act
            var res = converer.Convert(str,1);

            //Asert
            res.Should().Be("Стр0x09ока0x0910x09Стр0x09ока0x0920x09Стр0x09ока0x093"); 
        }


        [Fact]
        public void EmptyString()
        {
            //Arrage
            var option = new InseartEndLineMarkerConverterOption
            {
                LenghtLine = 100,
                Marker = "0x09"

            };
            var converer = new InseartEndLineMarkerConverter(option);
            var str = String.Empty;

            //Act
            var res = converer.Convert(str,1);

            //Asert
            res.Should().Be(str);
        }


        [Fact]
        public void NullString()
        {
            //Arrage
            var option = new InseartEndLineMarkerConverterOption
            {
                LenghtLine = 100,
                Marker = "0x09"

            };
            var converer = new InseartEndLineMarkerConverter(option);
            string str = null;

            //Act
            var res = converer.Convert(str,1);

            //Asert
            res.Should().Be(str);
        }
    }
}