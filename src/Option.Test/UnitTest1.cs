using DAL.Abstract.Entities.Options.Device;
using FluentAssertions;
using Shared.Helpers;
using Xunit;

namespace Option.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var stringRequset = "скор.";
            var format = "Windows-1251";
            var resultBuffer = stringRequset.ConvertString2ByteArray(format);

        }
    }
}
