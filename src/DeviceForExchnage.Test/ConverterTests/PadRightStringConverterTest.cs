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
        public void NullString_Padright_5Spaces()
        {
            //Arrage
            var converer = new PadRightStringConverter(Option);
            string str = null;

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
        
        
        [Fact]
        public void NullString_With_PaddingChar_Option_Padright_5Spaces()
        {
            //Arrage
            var converer = new PadRightStringConverter(new PadRightStringConverterOption
            {
                Lenght = 5,
                PaddingChar = '_'
            });
            string str = null;

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be("_____");
        }
        
        
        [Fact]
        public void Inseart_With_PaddingChar_Option_Padright_5Spaces()
        {
            //Arrage
            var converer = new PadRightStringConverter(new PadRightStringConverterOption
            {
                Lenght = 5,
                PaddingChar = '_'
            });
            string str = "123";

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be("123__");
        }
        
        
        [Fact]
        public void Inseart_String_Spase_With_PaddingChar_Option_Padright_5Spaces()
        {
            //Arrage
            var converer = new PadRightStringConverter(new PadRightStringConverterOption
            {
                Lenght = 5,
                PaddingChar = '_'
            });
            string str = "1  ";

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be("1  __");
        }
    }
}