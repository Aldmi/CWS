using System;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Threading;
using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;
using DeviceForExchange.MiddleWares.Converters.StringConverters;
using FluentAssertions;
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
            var converer = new LimitStringComverter(Option);
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
            var converer = new LimitStringComverter(Option);
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
            var converer = new LimitStringComverter(Option);
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
            var converer = new LimitStringComverter(Option);
            string str = null;

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().BeNull();
        }
    }
}