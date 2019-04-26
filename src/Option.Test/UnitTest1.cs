using DAL.Abstract.Entities.Options.Device;
using FluentAssertions;
using Shared.CrcCalculate;
using Shared.Helpers;
using Xunit;

namespace Option.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //var stringRequset = "скор.";
            //var format = "Windows-1251";
            //var resultBuffer = stringRequset.ConvertString2ByteArray(format);


            //DEBUG
            //1B 57 41 46 32 36 09 F1 EA EE F0 09 C8 C6 C5 C2 D1 CA 09 31 37 3A 33 38 09 36 09 - XOR = 50
            var crcBytes = new byte[] { 0x1B, 0x57, 0x41, 0x46, 0x32, 0x36, 0x09, 0xF1, 0xEA, 0xEE, 0xF0, 0x09, 0xC8, 0xC6, 0xC5, 0xC2, 0xD1, 0xCA, 0x09, 0x31, 0x37, 0x3A, 0x33, 0x38, 0x09, 0x36, 0x09 };
            var crc = CrcCalc.CalcXor(crcBytes);

        }
    }
}
