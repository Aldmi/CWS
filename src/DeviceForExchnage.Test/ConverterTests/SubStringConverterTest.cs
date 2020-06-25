using FluentAssertions;
using Shared.MiddleWares.Converters.StringConverters;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class SubStringConverterTest
    {
        private const string Str = "Транзит";


        [Fact]
        public void StartIndex_NormalUse()
        {
            //Arrage
            var option = new SubStringConverterOption
            {
                StartIndex = 3
            };
            var converter = new SubStringConverter(option);

            //Act
            var res = converter.Convert(Str, 0);

            //Asert
            res.Should().Be("нзит");
        }

        [Fact]
        public void EndIndex_NormalUse()
        {
            //Arrage
            var option = new SubStringConverterOption
            {
                EndIndex = 3
            };
            var converter = new SubStringConverter(option);

            //Act
            var res = converter.Convert(Str, 0);

            //Asert
            res.Should().Be("Тра");
        }


        [Fact]
        public void StartIndex_And_EndIndex_NormalUse()
        {
            //Arrage
            var option = new SubStringConverterOption
            {
                StartIndex = 2,
                EndIndex = 4
            };
            var converter = new SubStringConverter(option);

            //Act
            var res = converter.Convert(Str, 0);

            //Asert
            res.Should().Be("ан");
        }



        [Fact]
        public void StartIndex_Highter_Lenght()
        {
            //Arrage
            var option = new SubStringConverterOption
            {
                StartIndex = 10
            };
            var converter = new SubStringConverter(option);

            //Act
            var res = converter.Convert(Str, 0);

            //Asert
            res.Should().Be("Транзит");
        }


        [Fact]
        public void EndIndex_Highter_Lenght()
        {
            //Arrage
            var option = new SubStringConverterOption
            {
                EndIndex = 10
            };
            var converter = new SubStringConverter(option);

            //Act
            var res = converter.Convert(Str, 0);

            //Asert
            res.Should().Be("Транзит");
        }


        [Fact]
        public void StartIndex_Equal_0()
        {
            //Arrage
            var option = new SubStringConverterOption
            {
                StartIndex = 0
            };
            var converter = new SubStringConverter(option);

            //Act
            var res = converter.Convert(Str, 0);

            //Asert
            res.Should().Be("Транзит");
        }


        [Fact]
        public void EndIndex_Equal_0()
        {
            //Arrage
            var option = new SubStringConverterOption
            {
                EndIndex = 0
            };
            var converter = new SubStringConverter(option);

            //Act
            var res = converter.Convert(Str, 0);

            //Asert
            res.Should().Be("");
        }



        [Fact]
        public void StartIndex_Equal_Lenght_Minus1()
        {
            //Arrage
            var option = new SubStringConverterOption
            {
                StartIndex = Str.Length-1
            };
            var converter = new SubStringConverter(option);

            //Act
            var res = converter.Convert(Str, 0);

            //Asert
            res.Should().Be("т");
        }


        [Fact]
        public void EndIndex_Equal_Lenght_Minus1()
        {
            //Arrage
            var option = new SubStringConverterOption
            {
                EndIndex = Str.Length - 1
            };
            var converter = new SubStringConverter(option);

            //Act
            var res = converter.Convert(Str, 0);

            //Asert
            res.Should().Be("Транзи");
        }


        [Fact]
        public void StartIndex_Equal_0_And_EndIndex_Equal_Lenght_Minus1()
        {
            //Arrage
            var option = new SubStringConverterOption
            {
                StartIndex = 0,
                EndIndex = Str.Length - 1
            };
            var converter = new SubStringConverter(option);

            //Act
            var res = converter.Convert(Str, 0);

            //Asert
            res.Should().Be("Транзи");
        }


        [Fact]
        public void StartIndex_Equal_Lenght_Minus2_And_EndIndex_Equal_Lenght_Minus1()
        {
            //Arrage
            var option = new SubStringConverterOption
            {
                StartIndex = Str.Length - 1,
                EndIndex = Str.Length
            };
            var converter = new SubStringConverter(option);

            //Act
            var res = converter.Convert(Str, 0);

            //Asert
            res.Should().Be("т");
        }


        [Fact]
        public void NullString()
        {
            //Arrage
            var option = new SubStringConverterOption
            {
                EndIndex = Str.Length - 1
            };
            var converter = new SubStringConverter(option);

            //Act
            var res = converter.Convert(null, 0);

            //Asert
            res.Should().BeNull();
        }
    }
}