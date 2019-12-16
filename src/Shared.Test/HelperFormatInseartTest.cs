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
            var str = "0x57{Nbyte:X2 fff {CRCXor[0x02-0x03]:X2}";     //not replace {Nbyte:X2}
            //var str2 = "0x57{ fff {CRCXor[0x02-0x03]:X2}";            //replace {CRCXor[0x02-0x03]:X2}

            //Act
            var res=  HelperStringFormatInseart.CreateInseartDict(str, pattern);


            //Asert
        }


        
        //var replasement = "Nbyte";
        //var val = 10;
        //var afteReplace = str.Replace(replasement, "0");
        //var formatStr = string.Format(afteReplace, val);
    }
}