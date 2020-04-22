using System;
using FluentAssertions;
using Shared.MiddleWares.Converters.StringConverters;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class PadRightStringConverterTest
    {
        private PadRightStringConverterOption Option { get; }

        public PadRightStringConverterTest()
        {
            Option = new PadRightStringConverterOption()
            {
                Lenght = 5
            };
        }


        [Fact]
        public void Inseart_PadRight_3Spaces()
        {
            //Arrage
            var converer = new PadRightStringConverter(Option);
            var str = "12";

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be("12   ");
        }


        [Fact]
        public void EmptyString_Padright_5Spaces()
        {
            //Arrage
            var converer = new PadRightStringConverter(Option);
            var str = String.Empty;

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be("     ");
        }


        [Fact]
        public void NotInseart_Spaces()
        {
            //Arrage
            var converer = new PadRightStringConverter(Option);
            var str = "1234567";

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be(str);
        }


        [Fact]
        public void NotInseart_Spaces_Lenght_Equal_OptionLenght()
        {
            //Arrage
            var converer = new PadRightStringConverter(Option);
            var str = "12345";

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be(str);
        }
    }
}