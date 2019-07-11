using System;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Threading;
using DAL.Abstract.Entities.Options.MiddleWare.Converters.StringConvertersOption;
using DeviceForExchange.MiddleWares.Converters.StringConverters;
using FluentAssertions;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class ReplaceEmptyStringConverterTest
    {
        private ReplaceEmptyStringConverterOption Option { get; }

        public ReplaceEmptyStringConverterTest()
        {
            Option = new ReplaceEmptyStringConverterOption()
            {
                ReplacementString = "ПОСАДКИ НЕТ"
            };
        }



        [Fact]
        public void NormalUse()
        {
            //Arrage
            var converer = new ReplaceEmptyStringConverter(Option);
            var str = String.Empty;

            //Act
            var res = converer.Convert(str,0);

            //Asert
            res.Should().Be("ПОСАДКИ НЕТ");
        }


        [Fact]
        public void NotEmptyString()
        {
            //Arrage
            var converer = new ReplaceEmptyStringConverter(Option);
            var str = "Какая-то строка";

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be(str);
        }




        [Fact]
        public void NullString()
        {
            //Arrage
            var converer = new ReplaceEmptyStringConverter(Option);
            string str = null;

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().BeNull();
        }
    }
}