using FluentAssertions;
using Shared.Types;
using Xunit;

namespace Shared.Test
{
    public class BorderSubStringTest
    {
        private const string Str= "1250x02hgfhggfdhgfdfh0x03hghg";


        [Fact]
        public void Include_Both_Border_Test()
        {
            //Arrange
            var border = new BorderSubString
            {
                StartCh = "0x02",
                EndCh = "0x03",
                StartInclude = true,
                EndInclude = true,
            };

            //Act
            var res = border.Calc(Str);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("0x02hgfhggfdhgfdfh0x03");
        }


        [Fact]
        public void Include_Start_Border_Test()
        {
            //Arrange
            var border = new BorderSubString
            {
                StartCh = "0x02",
                EndCh = "0x03",
                StartInclude = true,
                EndInclude = false,
            };

            //Act
            var res = border.Calc(Str);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("0x02hgfhggfdhgfdfh");
        }


        [Fact]
        public void Include_End_Border_Test()
        {
            //Arrange
            var border = new BorderSubString
            {
                StartCh = "0x02",
                EndCh = "0x03",
                StartInclude = false,
                EndInclude = true,
            };

            //Act
            var res = border.Calc(Str);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("hgfhggfdhgfdfh0x03");
        }


        [Fact]
        public void Exclude_Both_Border_Test()
        {
            //Arrange
            var border = new BorderSubString
            {
                StartCh = "0x02",
                EndCh = "0x03",
                StartInclude = false,
                EndInclude = false,
            };

            //Act
            var res = border.Calc(Str);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("hgfhggfdhgfdfh");
        }



        [Fact]
        public void NotFound_Start_Border_Test()
        {
            //Arrange
            var border = new BorderSubString
            {
                StartCh = "0x99",
                EndCh = "0x03",
                StartInclude = false,
                EndInclude = false,
            };

            //Act
            var res = border.Calc(Str);

            //Assert
            res.IsSuccess.Should().BeFalse();
            res.Error.Should().Be("Not Found startCh= 0x99");
        }



        [Fact]
        public void NotFound_End_Border_Test()
        {
            //Arrange
            var border = new BorderSubString
            {
                StartCh = "0x02",
                EndCh = "0x99",
                StartInclude = false,
                EndInclude = false,
            };

            //Act
            var res = border.Calc(Str);

            //Assert
            res.IsSuccess.Should().BeFalse();
            res.Error.Should().Be("Not Found endCh= 0x99");
        }


        [Fact]
        public void WithOut_Start_Border_Test()
        {
            //Arrange
            var border = new BorderSubString
            {
                EndCh = "0x03",
                EndInclude = false,
            };

            //Act
            var res = border.Calc(Str);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("1250x02hgfhggfdhgfdfh");
        }


        [Fact]
        public void WithOut_End_Border_Test()
        {
            //Arrange
            var border = new BorderSubString
            {
                StartCh = "01",
                StartInclude = true
            };

            //Act
            var str = "1648{CRC:X2_Border}01855a12030000000000010200188020313235";
            var res = border.Calc(str);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("01855a12030000000000010200188020313235");
        }
    }
}