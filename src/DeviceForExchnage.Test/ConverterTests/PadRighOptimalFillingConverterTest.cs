using System.Collections.Generic;
using FluentAssertions;
using Shared.MiddleWares.Converters.StringConverters;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class PadRighOptimalFillingConverterTest
    {
        private PadRighOptimalFillingConverterOption Option { get; }
       

        public PadRighOptimalFillingConverterTest()
        {
            Option = new PadRighOptimalFillingConverterOption()
            {
                Lenght = 20,
                DictWeight = new Dictionary<int, string>
                {
                    {1, "0x1f" },
                    {2, "0x1e" },
                    {3, "0x1d" },
                    {4, "0x1c" },
                    {5, "0x1b" },
                    {6, "0x1a" },
                    {7, "0x19" },
                    {8, "0x18" },
                    {9, "0x17" },
                    {10, "0x16" }
                }
            };
        }



        [Fact]
        public void Inseart_Not_Needed()
        {
            //Arrage
            const string str = "123456789qwertyuiiopoppasdff";
            var converer = new PadRighOptimalFillingConverter(Option);

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be(str);
        }


        [Fact]
        public void Inseart_()
        {
            //Arrage
            const string str = "12345";
            var converer = new PadRighOptimalFillingConverter(Option);

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be("1234");
        }
    }
}