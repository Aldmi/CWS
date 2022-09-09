using System.Linq;
using System.Text;
using FluentAssertions;
using Shared.Helpers;
using Xunit;

namespace Shared.Test
{
    public class HelpersByteArrayTest
    {
        [Fact]
        public void ConvertString2ByteArray_HEX()
        {
            //Arrage
          

            //Act
 

            //Asert
      
        }
        
        [Fact]
        public void ConvertString2ByteArray_Win1251()
        {
            //Arrage
            //var str = "--------- В НОВОСИБИРСК----------........";
            var str = "6605";  //D2 C5 D1 D2
            var str1 = "0x07ТЕСТ";

            var arr = str.ToCharArray();
            var arr1 = str1.ToCharArray();
            
            var format = "Windows-1251";   //"utf-8"


            //Act
            var res= str1.ConvertStringWithHexEscapeChars2ByteArray(format);

            //Asert

        }

    }
}