using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Shared.MiddleWares.Converters.StringConverters;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Xunit;

namespace DeviceForExchnage.Test.ConverterTests
{
    public class ReplaseCharStringConverterTest
    {
        private ReplaseCharStringConverterOption Option { get; }

        public ReplaseCharStringConverterTest()
        {
            Option = new ReplaseCharStringConverterOption()
            {
                Mapping = new Dictionary<char, string>
                {
                    {' ', "00 00 00 00 00"},
                    {'-', "08 08 08 08 08"},
                    {'А', "7C 12 11 11 7F"},
                    {'Б', "7F 49 49 49 30"},
                    {'В', "7F 49 49 49 36"},

                    {'0', "3E 41 41 41 3E"},
                    {'1', "04 02 7F 00 00"},
                    {'2', "42 61 51 49 46"},
                    {'3', "22 41 49 49 36"},
                }
            };
        }



        [Fact]
        public void NormalUse()
        {
            //Arrage
            var converter = new ReplaseCharStringConverter(Option);
            var str = "АБВ 123";

            //Act
            var res = converter.Convert(str, 0);

            //Asert
            res.Should().Be("7C 12 11 11 7F7F 49 49 49 307F 49 49 49 3600 00 00 00 0004 02 7F 00 0042 61 51 49 4622 41 49 49 36");
        }


        [Fact]
        public void Not_Found_Char_Key_Test()
        {
            //Arrage
            var converer = new ReplaseCharStringConverter(Option);
            var str = "АБЮ";

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().Be("7C 12 11 11 7F7F 49 49 49 30Ю");
        }


        [Fact]
        public void NullString()
        {
            //Arrage
            var converer = new ReplaseCharStringConverter(Option);
            string str = null;

            //Act
            var res = converer.Convert(str, 0);

            //Asert
            res.Should().BeNull();
        }


        [Fact]
        public void ToLowerInvariant_True_Test()
        {
            //Arrage
            var option = new ReplaseCharStringConverterOption()
            {
                ToLowerInvariant = true,
                Mapping = new Dictionary<char, string>
                {
                    {' ', "00 00 00 00 00"},
                    {'-', "08 08 08 08 08"},
                    {'а', "7C 12 11 11 7F"},
                    {'б', "7F 49 49 49 30"},
                    {'в', "7F 49 49 49 36"},

                    {'0', "3E 41 41 41 3E"},
                    {'1', "04 02 7F 00 00"},
                    {'2', "42 61 51 49 46"},
                    {'3', "22 41 49 49 36"},
                }
            };

            var converter = new ReplaseCharStringConverter(option);
            var str = "АБВ 123";

            //Act
            var res = converter.Convert(str, 0);

            //Asert
            res.Should().Be("7C 12 11 11 7F7F 49 49 49 307F 49 49 49 3600 00 00 00 0004 02 7F 00 0042 61 51 49 4622 41 49 49 36");
        }

        [Fact]
        public void ToLowerInvariant_False_Test()
        {
            //Arrage
            var option = new ReplaseCharStringConverterOption()
            {
                ToLowerInvariant = false,
                Mapping = new Dictionary<char, string>
                {
                    {' ', "00 00 00 00 00"},
                    {'-', "08 08 08 08 08"},
                    {'а', "7C 12 11 11 7F"},
                    {'б', "7F 49 49 49 30"},
                    {'в', "7F 49 49 49 36"},

                    {'0', "3E 41 41 41 3E"},
                    {'1', "04 02 7F 00 00"},
                    {'2', "42 61 51 49 46"},
                    {'3', "22 41 49 49 36"},
                }
            };

            var converter = new ReplaseCharStringConverter(option);
            var str = "АБВ 123";

            //Act
            var res = converter.Convert(str, 0);

            //Asert
            res.Should().Be("АБВ00 00 00 00 0004 02 7F 00 0042 61 51 49 4622 41 49 49 36");
        }


    }
}