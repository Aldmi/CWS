using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts;
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
            new object[]
            {
                "0xFF0xFF0x020x1B0x57Москва0x094560x090x0x1F0x03",
                "0x02",
                "0x03",
                "0x020x1B0x57Москва0x094560x090x0x1F0x03",
                "0x1B0x57Москва0x094560x090x0x1F"
            },
            new object[]
            {
                "0x02001122333440x03",
                "0x02",
                "0x03",
                "0x02001122333440x03",
                "00112233344"
            },
            new object[]
            {
                "00112233340x0240x03",
                "0x02",
                "0x03",
                "0x0240x03",
                "4"
            },
            new object[]
            {
                "00112233340x40x020x03",
                "0x02",
                "0x03",
                "0x020x03",
                ""
            },
            new object[]
            {
                "0*12233340x40x02-0x03",
                "*",
                "-",
                "*12233340x40x02-",
                "12233340x40x02"
            },
            new object[]
            {
                "0*12233340x40x02-",
                "*",
                "-",
                "*12233340x40x02-",
                "12233340x40x02"
            },
            new object[]
            {
                "00xffaa12233340x40xffbbx02-",
                "0xffaa",
                "0xffbb",
                "0xffaa12233340x40xffbb",
                "12233340x4"
            }
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
            resIncludeBorder.IsSuccess.Should().BeTrue();
            resExcludeBorder.IsSuccess.Should().BeTrue();

            resIncludeBorder.Value.Should().Be(expectedStrIncludeBorder);
            resExcludeBorder.Value.Should().Be(expectedStrExcludeBorder);
        }


        [Fact]
        public void SubstringBetweenCharactersEndChNotFoundTest()
        {
            //Arrange
            var str = "00112233340x40x02";
            string startCh = "0x02";
            string endCh = "0x03";

            //Act
            var resIncludeBorder = str.SubstringBetweenCharacters(startCh, endCh, true);
            var resExcludeBorder = str.SubstringBetweenCharacters(startCh, endCh, false);

            //Asert
            resIncludeBorder.IsFailure.Should().BeTrue();
            resExcludeBorder.IsFailure.Should().BeTrue();

            resIncludeBorder.Error.Should().Be($"Not Found endCh= {endCh}");
            resExcludeBorder.Error.Should().Be($"Not Found endCh= {endCh}");
        }


        
        [Fact]
        public void SubstringBetweenCharactersStartChNotFoundTest()
        {
            //Arrange
            var str = "00112233340x40x05hgh0x03";
            string startCh = "0x02";
            string endCh = "0x03";

            //Act
            var resIncludeBorder = str.SubstringBetweenCharacters(startCh, endCh, true);
            var resExcludeBorder = str.SubstringBetweenCharacters(startCh, endCh, false);

            //Asert
            resIncludeBorder.IsFailure.Should().BeTrue();
            resExcludeBorder.IsFailure.Should().BeTrue();

            resIncludeBorder.Error.Should().Be($"Not Found startCh= {startCh}");
            resExcludeBorder.Error.Should().Be($"Not Found startCh= {startCh}");
        }



        #region TheoryData
        public static IEnumerable<object[]> CalcBatchedSequenceDatas => new[]
        {
            new object[]
            {
                7,
                3,
                new[]
                {
                    "aaa aaa aaa ",
                    "aaa aaa aaa ",
                    "aaa ",
                }
            },
            new object[]
            {
                1,
                1,
                new[]
                {
                    "aaa "
                }
            },
            new object[]
            {
                10,
                2,
                new[]
                {
                    "aaa aaa ",
                    "aaa aaa ",
                    "aaa aaa ",
                    "aaa aaa ",
                    "aaa aaa "
                }
            },
            new object[]
            {
                3,
                3,
                new[]
                {
                    "aaa aaa aaa "
                }
            },
            new object[]
            {
                1,
                1000,
                new[]
                {
                    "aaa "
                }
            }
        };
        #endregion
        [Theory]
        [MemberData(nameof(CalcBatchedSequenceDatas))]
        public void CalcBatchedSequenceTest(int count, int batchSize, string[] expectedSeq)
        {
            //Arrange
            string str = "aaa ";

            //Act
            var batchedSequence = HelperString.CalcBatchedSequence(str, count, batchSize);

            //Assert
            batchedSequence.Length.Should().Be(batchedSequence.Length);
            for (var i = 0; i < batchedSequence.Length; i++)
            {
                batchedSequence[i].Should().Be(expectedSeq[i]);
            }
        }
    }
}
