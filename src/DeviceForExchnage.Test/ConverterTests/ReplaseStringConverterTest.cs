using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Shared.MiddleWares.Converters.StringConverters;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class ReplaseStringConverterTest
    {
        private ReplaseStringConverterOption Option { get; }

        public ReplaseStringConverterTest()
        {
            Option = new ReplaseStringConverterOption()
            {
                Mapping = new Dictionary<string, string>
                {
                    {"Транзит","Стоянка" },
                    {"_", "DefaultValue" }
                }
            };
        }



        [Fact]
        public void NormalUse()
        {
            //Arrage
            var converter = new ReplaseStringConverter(Option);
            var str = "Транзит";

            //Act
            var res = converter.Convert(str,0);

            //Asert
            res.Should().Be("Стоянка");
        }


        [Fact]
        public void DefaultValue_Mapping_Test()
        {
            //Arrage
            var converer = new ReplaseStringConverter(Option);
            var str = "Какая-то строка";

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be("DefaultValue");
        }


        [Fact]
        public void NullString()
        {
            //Arrage
            var converer = new ReplaseStringConverter(Option);
            string str = null;

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().BeNull();
        }


        [Fact]
        public void WithOut_DefaultValue_Mapping_Test()
        {
            //Arrage
            var option = new ReplaseStringConverterOption()
            {
                Mapping = new Dictionary<string, string>
                {
                    {"Транзит","Стоянка" }
                }
            };
            var converter = new ReplaseStringConverter(option);
            var str = "Какая-то строка";

            //Act
            var res = converter.Convert(str, 0);

            //Asert
            res.Should().Be("Какая-то строка");
        }

    }
}