using FluentAssertions;
using Shared.Helpers;
using Xunit;

namespace Shared.Test
{
    public class HelperFormatInseartTest
    {
        [Fact]
        public void StringFormatReplaceTest()
        {
            //Arrange
            const string pattern = @"\{(.*?)(:.+?)?\}";

            //"\u0002{AddressDevice:X2 {Nbyte:X2}"
            //var str = "0x57{Nbyte:X2 fff {CRCXor[0x02-0x03]:X2}";     //not replace {Nbyte:X2}
            var str = "0x57fff {CRCXor[0x02-0x03]:X2}"; 

            //var str2 = "0x57{ fff {CRCXor[0x02-0x03]:X2}";            //replace {CRCXor[0x02-0x03]:X2}

            //Act
            var res=  HelperStringFormatInseart.CreateInseartDictDistinctByReplacement(str, pattern);

            var existKey=  res.TryGetValue("CRCXor[0x02-0x03]", out var value);
            existKey.Should().BeTrue();
            value.Replacement.Should().Be("{CRCXor[0x02-0x03]:X2}");
            value.VarName.Should().Be("CRCXor[0x02-0x03]");
            value.Format.Should().Be(":X2");


            //Asert
        }


        
        //var replasement = "Nbyte";
        //var val = 10;
        //var afteReplace = str.Replace(replasement, "0");
        //var formatStr = string.Format(afteReplace, val);
    }
}