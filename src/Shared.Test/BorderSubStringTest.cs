using FluentAssertions;
using Shared.Types;
using Xunit;

namespace Shared.Test
{
    public class BorderSubStringTest
    {
        private const string Str= "1250x02hgfhggfdhgfdfh0x03hghg{Nchar:X2}";
        private const string DelimiterStr = "{Nchar:X2}";


        [Fact]
        public void Set_DelimiterSign_None_Include_Both_Border_Test()
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
            var res = border.Calc(Str, DelimiterStr);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("0x02hgfhggfdhgfdfh0x03");
        }


        [Fact]
        public void Set_DelimiterSign_None_Include_Start_Border_Test()
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
            var res = border.Calc(Str, DelimiterStr);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("0x02hgfhggfdhgfdfh");
        }


        [Fact]
        public void Set_DelimiterSign_None_Include_End_Border_Test()
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
            var res = border.Calc(Str, DelimiterStr);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("hgfhggfdhgfdfh0x03");
        }


        [Fact]
        public void Set_DelimiterSign_None_Exclude_Both_Border_Test()
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
            var res = border.Calc(Str, DelimiterStr);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("hgfhggfdhgfdfh");
        }


        [Fact]
        public void Set_DelimiterSign_None_WithOut_Start_Border_Test()
        {
            //Arrange
            var border = new BorderSubString
            {
                EndCh = "0x03",
                EndInclude = false,
            };

            //Act
            var res = border.Calc(Str, DelimiterStr);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("1250x02hgfhggfdhgfdfh");
        }


        [Fact]
        public void Set_DelimiterSign_None_WithOut_End_Border_Test()
        {
            //Arrange
            var border = new BorderSubString
            {
                StartCh = "01",
                StartInclude = true
            };

            //Act
            var str = "1648{CRC:X2_Border}01855a12030000000000010200188020313235";
            var res = border.Calc(str, DelimiterStr);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("01855a12030000000000010200188020313235");
        }

        //--- DelimiterSign Set --------------------------------------------------------------------

        [Fact]
        public void Set_DelimiterSign_Left_And_Both_Border()
        {
            //Arrange
            var str = "1250x02hgfhgg{Nchar:X2}fdfh0x03hghg";
            const string delimiterStr = "{Nchar:X2}";
            var border = new BorderSubString
            {
                DelimiterSign = DelimiterSign.Left,
                StartCh = "25",
                EndCh = "gg",
                StartInclude = true,
                EndInclude = true,
            };
       
            //Act
            var res = border.Calc(str, delimiterStr);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("250x02hgfhgg");
        }


        [Fact]
        public void Set_DelimiterSign_Right_And_Both_Border()
        {
            //Arrange
            var str = "1250x02hgfhgg{Nchar:X2}fdfh0x03hghg963";
            const string delimiterStr = "{Nchar:X2}";
            var border = new BorderSubString
            {
                DelimiterSign = DelimiterSign.Right,
                StartCh = "0x03",
                EndCh = "63",
                StartInclude = false,
                EndInclude = true,
            };

            //Act
            var res = border.Calc(str, delimiterStr);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("hghg963");
        }


        [Fact]
        public void Set_DelimiterSign_DeleteDelemiter_And_Both_Border()
        {
            //Arrange
            var str = "1250x02hgfhgg{Nchar:X2}fdfh0x03hghg963";
            const string delimiterStr = "{Nchar:X2}";
            var border = new BorderSubString
            {
                DelimiterSign = DelimiterSign.DeleteDelemiter,
                StartCh = "25",
                EndCh = "96",
                StartInclude = false,
                EndInclude = true,
            };

            //Act
            var res = border.Calc(str, delimiterStr);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("0x02hgfhggfdfh0x03hghg96");
        }


        [Fact]
        public void Set_DelimiterSign_None_DelimiterStr_Equal_Null()
        {
            //Arrange
            var str = "1250x02hgfhgg{Nchar:X2}fdfh0x03hghg";
            var border = new BorderSubString
            {
                DelimiterSign = DelimiterSign.None,
                StartCh = "125",
                EndCh = "0x03",
                StartInclude = false,
                EndInclude = false,
            };

            //Act
            var res = border.Calc(str, null);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("0x02hgfhgg{Nchar:X2}fdfh");
        }


        [Fact]
        public void Select_Substring_Between_Two_Inserts()
        {
            //Arrange
            var str = "1250x02hgfhgg{Nchar:X2}0x02ffffffffff0x03hghg{CRCXor:X2}";
            const string delimiterStr = "{Nchar:X2}";
            var border = new BorderSubString
            {
                DelimiterSign = DelimiterSign.Right,
                EndCh = "{CRC",
                EndInclude = false
            };

            //Act
            var res = border.Calc(str, delimiterStr);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("0x02ffffffffff0x03hghg");
        }


        [Fact]
        public void CheckError_Set_DelimiterSign_None_NotFound_Start_Border_Test()
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
            var res = border.Calc(Str, DelimiterStr);

            //Assert
            res.IsSuccess.Should().BeFalse();
            res.Error.Should().Be("Not Found startCh= 0x99");
        }


        [Fact]
        public void CheckError_Set_DelimiterSign_None_NotFound_End_Border_Test()
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
            var res = border.Calc(Str, DelimiterStr);

            //Assert
            res.IsSuccess.Should().BeFalse();
            res.Error.Should().Be("Not Found endCh= 0x99");
        }


        [Fact]
        public void CheckError_Set_DelimiterSign_DelimiterStr_NotFound()
        {
            //Arrange
            var str = "1250x02hgfhgg{Nchar:X2}fdfh0x03hghg";
            const string delimiterStr = "1111";
            var border = new BorderSubString
            {
                DelimiterSign = DelimiterSign.Left,
                StartCh = "25",
                EndCh = "gg",
                StartInclude = true,
                EndInclude = true,
            };

            //Act
            var res = border.Calc(str, delimiterStr);

            //Assert
            res.IsSuccess.Should().BeFalse();
            res.Error.Should().Be("BorderSubString.Calc(...)  DelimiterSign.Left 1111 не выделили ЛЕВУЮ часть строки 1250x02hgfhgg{Nchar:X2}fdfh0x03hghg");
        }


        [Fact]
        public void CheckError_Set_DelimiterSign_Not_None_DelimiterStr_Equal_Null()
        {
            //Arrange
            var str = "1250x02hgfhgg{Nchar:X2}fdfh0x03hghg";
            var border = new BorderSubString
            {
                DelimiterSign = DelimiterSign.Left,
                StartCh = "125",
                EndCh = "0x03",
                StartInclude = false,
                EndInclude = false,
            };

            //Act
            var res = border.Calc(str, null);

            //Assert
            res.IsSuccess.Should().BeFalse();
            res.Error.Should().Be("BorderSubString.Calc(...)  delemiter не может быть null или пуст.");
        }
    }
}