using DAL.Abstract.Entities.Options.Device;
using FluentAssertions;
using Xunit;

namespace Option.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var gg = new DeviceOption(){Id = 10};

            gg.Id.Should().Be(10);
        }
    }
}
