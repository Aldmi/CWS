using System;
using System.Collections.Generic;
using FluentAssertions;
using Shared.MiddleWares.Converters.Exceptions;
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
                    {1, "_1_" },
                    {2, "_2_" },
                    {3, "_3_" },
                    {4, "_4_" },
                    {5, "_5_" },
                    {6, "_6_" },
                    {7, "_7_" },
                    {8, "_8_" },
                    {9, "_9_" },
                    {10, "_10_" }
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
        public void Inseart_addition15()
        {
            //Arrage
            const string str = "12345";
            var converer = new PadRighOptimalFillingConverter(Option);

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            var addition = Option.DictWeight[10] + Option.DictWeight[5];
            res.Should().Be(str + addition);
        }


        [Fact]
        public void Inseart_addition0()
        {
            //Arrage
            const string str = "123456789aaaaaaaaaaa";
            var converer = new PadRighOptimalFillingConverter(Option);

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            var addition = String.Empty;
            res.Should().Be(str + addition);
        }


        [Fact]
        public void Inseart_addition1()
        {
            //Arrage
            const string str = "123456789aaaaaaaaaa";
            var converer = new PadRighOptimalFillingConverter(Option);

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            var addition = Option.DictWeight[1];
            res.Should().Be(str + addition);
        }


        [Fact]
        public void Inseart_addition20()
        {
            //Arrage
            const string str = "";
            var converer = new PadRighOptimalFillingConverter(Option);

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            var addition = Option.DictWeight[10] + Option.DictWeight[10];
            res.Should().Be(str + addition);
        }


        [Fact]
        public void Inseart_addition16()
        {
            //Arrage
            const string str = "1234";
            var converer = new PadRighOptimalFillingConverter(Option);

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            var addition = Option.DictWeight[10] + Option.DictWeight[6];
            res.Should().Be(str + addition);
        }


        [Fact]
        public void Null_Str()
        {
            //Arrage
            const string str = null;
            var converer = new PadRighOptimalFillingConverter(Option);

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().BeNull();
        }



        [Fact]
        public void NotValid_Option_DictWeight()
        {
            //Arrage
            var option = new PadRighOptimalFillingConverterOption()
            {
                Lenght = 20,
                DictWeight = new Dictionary<int, string>
                {
                    {7, "_7_" },
                    {8, "_8_" },
                    {9, "_9_" },
                    {10, "_10_" }
                }
            };

            const string str = "1234"; //нужно добваить DictWeight[10] + DictWeight[6], но DictWeight[6]- НЕТ.
            var converer = new PadRighOptimalFillingConverter(option);


            //Act & Asert
            var exception = Assert.Throws<StringConverterException>(() => converer.Convert(str, 0));
            exception.Should().BeOfType<StringConverterException>();
            exception.Message.Should().Contain("PadRighOptimalFillingConverter ОПЦИИ заданы не верно. DictWeight не содержит элементов с коэфицентами меньше 6.");
        }
    }

}