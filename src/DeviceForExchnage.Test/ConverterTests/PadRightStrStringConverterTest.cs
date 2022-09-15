using System;
using FluentAssertions;
using Shared.MiddleWares.Converters.StringConverters;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class PadRightStrStringConverterTest
    {
        [Fact]
        public void Inseart_PadRight_3_Str()
        {
            //Arrage
            var converer = new PadRightStrStringConverter(new PadRightStrStringConverterOption
            {
                Lenght = 5,
                PaddingStr = "~Added~"
            });
            var str = "12";

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be("12~Added~~Added~~Added~");
        }
        
        
        [Fact]
        public void EmptyString_Padright_5_Str()
        {
            //Arrage
            var converer = new PadRightStrStringConverter(new PadRightStrStringConverterOption
            {
                Lenght = 5,
                PaddingStr = "~Added~"
            });
            var str = String.Empty;

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be("~Added~~Added~~Added~~Added~~Added~");
        }
        
        
        [Fact]
        public void Null_String_Padright_5_Str()
        {
            //Arrage
            var converer = new PadRightStrStringConverter(new PadRightStrStringConverterOption
            {
                Lenght = 5,
                PaddingStr = "~Added~"
            });
            string str = null;

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be("~Added~~Added~~Added~~Added~~Added~");
        }
        
        
        [Fact]
        public void NotInseart_Str()
        {
            //Arrage
            var converer = new PadRightStrStringConverter(new PadRightStrStringConverterOption
            {
                Lenght = 5,
                PaddingStr = "~Added~"
            });
            var str = "12345678";

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be(str);
        }
        
        
        [Fact]
        public void NotInseart_Spaces_Lenght_Equal_OptionLenght()
        {
            //Arrage
            var converer = new PadRightStrStringConverter(new PadRightStrStringConverterOption
            {
                Lenght = 5,
                PaddingStr = "~Added~"
            });
            var str = "12345";

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be(str);
        }
    }
}