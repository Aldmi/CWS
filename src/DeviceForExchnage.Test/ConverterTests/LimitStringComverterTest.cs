using System;
using FluentAssertions;
using Shared.MiddleWares.Converters.StringConverters;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class LimitStringComverterTest
    {
        private LimitStringConverterOption Option { get; }

        public LimitStringComverterTest()
        {
            Option = new LimitStringConverterOption()
            {
                Limit = 25
            };
        }



        [Fact]
        public void NormalUse()
        {
            //Arrage
            var converer = new LimitStringConverter(Option);
            var str = "Со всеми остановками кроме: серпухово, балаково, Свободное";

            //Act
            var res = converer.Convert(str,0);

            //Asert
            res.Length.Should().Be(25);
            res.Should().Be("Со всеми остановками кром");
        }


        [Fact]
        public void SmallString()
        {
            //Arrage
            var converer = new LimitStringConverter(Option);
            var str = "Привет";

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Length.Should().Be(6);
            res.Should().Be(str);
        }



        [Fact]
        public void EmptyString()
        {
            //Arrage
            var converer = new LimitStringConverter(Option);
            var str = String.Empty;

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Length.Should().Be(0);
            res.Should().Be(str);
        }


        [Fact]
        public void NullString()
        {
            //Arrage
            var converer = new LimitStringConverter(Option);
            string str = null;

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().BeNull();
        }
    }
}