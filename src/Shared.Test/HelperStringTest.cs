using System;
using System.Collections.Generic;
using FluentAssertions;
using Shared.Helpers;
using Xunit;

namespace Shared.Test
{
    public class HelperStringTest
    {
        [Fact]
        public void RemovingExtraSpacesNormalUseTest()
        {
            //Arrage
            var str = " Строка вот такая  то    555  69     ";

            //Act
            var res = str.RemovingExtraSpaces();

            //Asert
            res.Should().Be("Строка вот такая то 555 69");
        }


        [Fact]
        public void RemovingExtraSpaceseEmptyStringTest()
        {
            //Arrage
            var str = String.Empty;

            //Act
            var res = str.RemovingExtraSpaces();

            //Asert
            res.Should().BeEmpty();
        }


        [Fact]
        public void RemovingExtraSpaceseNullTest()
        {
            //Arrage
            string str = null;

            //Act
            var res = str.RemovingExtraSpaces();

            //Asert
            res.Should().BeNull();
        }





       #region TheoryData
        public static IEnumerable<object[]>  SubstringBetweenCharactersDatas => new[]
        {
            //new object[]
            //{
            //    "0xFF0xFF0x020x1B0x57Москва0x094560x090x0x1F0x03",
            //    "0x02",
            //    "0x03",
            //    "0x020x1B0x57Москва0x094560x090x0x1F0x03",
            //    "0x1B0x57Москва0x094560x090x0x1F"
            //},
            //new object[]
            //{
            //    "0x02001122333440x03",
            //    "0x02",
            //    "0x03",
            //    "0x02001122333440x03",
            //    "00112233344"
            //},
            //new object[]
            //{
            //    "00112233340x0240x03",
            //    "0x02",
            //    "0x03",
            //    "0x0240x03",
            //    "4"
            //},
            //new object[]
            //{
            //    "00112233340x40x020x03",
            //    "0x02",
            //    "0x03",
            //    "0x020x03",
            //    ""
            //},
            //new object[]
            //{
            //    "0*12233340x40x02-0x03",
            //    "*",
            //    "-",
            //    "*12233340x40x02-",
            //    "12233340x40x02"
            //},
            //new object[]
            //{
            //    "0*12233340x40x02-",
            //    "*",
            //    "-",
            //    "*12233340x40x02-",
            //    "12233340x40x02"
            //},
            new object[]
            {
                "00xffaa12233340x40xffbbx02-",
                "0xffaa",
                "0xffbb",
                "0xffaa12233340x40xffbb",
                "12233340x4"
            },



            //new object[]                      // endCh Not found 
            //{
            //    "00112233340x40x02",
            //    "0x02",
            //    "0x03",
            //    "0x020x03",
            //    ""
            //},
            //new object[]                       // startCh Not found 
            //{ 
            //    "00112233340x40x03",
            //    "0x02",
            //    "0x03",
            //    "0x020x03",
            //    ""
            //},

        };
        #endregion



        [Theory]
        [MemberData(nameof(SubstringBetweenCharactersDatas))]
        public void SubstringBetweenCharactersTest(string str, string startCh, string endCh, string expectedStrIncludeBorder, string expectedStrExcludeBorder)
        {
            //Act
            var resIncludeBorder = str.SubstringBetweenCharacters(startCh, endCh, true);
            var resExcludeBorder = str.SubstringBetweenCharacters(startCh, endCh, false);

            //Asert
            resIncludeBorder.Should().Be(expectedStrIncludeBorder);
            resExcludeBorder.Should().Be(expectedStrExcludeBorder);
        }

    }
}
