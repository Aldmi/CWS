using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Shared.Extensions;
using Xunit;

namespace Shared.Test.ExtensionsTests
{
    public class StringBuilderExtensionsTest
    {
        public static IEnumerable<object[]> ReplaceBracketsDatas => new[]
        {
             new object[]
             {
                 new StringBuilder("%%%\\\"123456\\\"%%% \\\"gggg\\\""), 
                 "\\\"",
                 "\\\"",
                 "",
                 "%%%123456%%% \\\"gggg\\\"",
                 6
             },
              new object[]
             {
                 new StringBuilder("%%%g123456ggg%%% gkjjkjkjggg"), 
                 "g",
                 "ggg",
                 "",
                 "%%%123456%%% gkjjkjkjggg",
                 6
             },
              new object[]
             {
                 new StringBuilder("%%%[]%%% [gggg]"),
                 "[",
                 "]",
                 "+",
                 "%%%++%%% [gggg]",
                 0
             },
             //StartIndex "fff" не найден
              new object[]
              {
                  new StringBuilder("%%%\\\"123456\\\"%%% \\\"gggg\\\""),
                  "fff",
                  "\\\"",
                  "",
                  "%%%\\\"123456\\\"%%% \\\"gggg\\\"",
                  0
              },
              //StartIndex и EndIndex пуст
              new object[]
              {
                  new StringBuilder("%%%\\\"123456\\\"%%% \\\"gggg\\\""),
                  "",
                  "",
                  "",
                  "%%%\\\"123456\\\"%%% \\\"gggg\\\"",
                  0
              },
              // Входная строка пуста
              new object[]
              {
                  new StringBuilder(""),
                  "\\\"",
                  "\\\"",
                  "",
                  "",
                  0
              }
        };
        [Theory]
        [MemberData(nameof(ReplaceBracketsDatas))]
        public void ReplaceBrackets_Test(StringBuilder sb, string startBracket, string endBracket, string newValue, string expectedStr, int expectedNumberOfCharactersBetweenBrackets)
        {
            //Act
            var (res, numberOfCharactersBetweenBrackets) = sb.ReplaceBrackets(startBracket, endBracket, newValue);
            
            //Assert
           res.ToString().Should().Be(expectedStr);
           numberOfCharactersBetweenBrackets.Should().Be(expectedNumberOfCharactersBetweenBrackets);
        }

        
        [Fact]
        public void ReplaceBrackets_NewValue_null_Test()
        {
            //Arrange
            StringBuilder sb = new StringBuilder("%%%\\\"123456\\\"%%% \\\"gggg\\\"");
            
            //Act & Asert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => sb.ReplaceBrackets("\\\"", "\\\"", null));
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Contain("newValue НЕ может быть null");
        }
        
                
        [Fact]
        public void Test()
        {
            var ts = TimeSpan.FromMinutes(5);
            var min=ts.ToString("%m");
        }
        
    }
}