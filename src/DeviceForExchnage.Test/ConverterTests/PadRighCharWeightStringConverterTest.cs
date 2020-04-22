using System.Collections.Generic;
using FluentAssertions;
using Shared.MiddleWares.Converters.StringConverters;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class PadRighCharWeightStringConverterTest
    {
        private PadRighCharWeightStringConverterOption Option { get; }
        public PadRighCharWeightStringConverterTest()
        {
            Option = new PadRighCharWeightStringConverterOption()
            {
                Lenght = 20,
                DictWeight = new Dictionary<string, int>
                {
                    {":", 2},
                    {@"/\", 3},
                    {"1-", 4},
                    {" ", 2},
                    {"234567890аБбВвГгЕеЁёЗзиЙйКкЛлНнОоПпРрСсТтУуХхЧчЬьЪъЭэЯя", 6},
                    {"АМмДдЦцИ", 7},
                    {"ФфШшЖжю", 8},
                    {"ЩщЮ", 9},
                },
                Pixel = '\u001F'
            };
        }



        #region TheoryData
        public static IEnumerable<object[]> Datas => new[]
        {
            new object[]
            {
                "Орл",
                20,
                "Орл"
            },
            new object[]
            {
                "Орл",
                25,
                "Орл\u001F\u001F\u001F\u001F\u001F"
            },
            new object[]
            {
                "",
                4,
                "\u001F\u001F\u001F\u001F"
            },
            new object[]
            {
                "Очень большое название станции",
                30,
                "Очень большое название станции"
            },
            new object[]
            {
                "1111",
                0,
                "1111"
            },

        };
        #endregion
        [Theory]
        [MemberData(nameof(Datas))]
        public void NormalUse(string str, int optionLenght, string expectedstr)
        {
            //Arrage
            Option.Lenght = optionLenght;
            var converer = new PadRightCharWeightStringConverter(Option);

            //Act
            var res = converer.Convert(str,0);

            //Asert
            res.Should().Be(expectedstr);
        }



        [Fact]
        public void NullString()
        {
            //Arrage
            var converer = new PadRightCharWeightStringConverter(Option);
            string str = null;

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().BeNull();
        }
    }
}