using System.Text;
using FluentAssertions;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.DependentInseart.DependentInseartHandlers;
using Xunit;

namespace Shared.Test.DependentInseartTest.DependentInseartHandlersTest
{
    public class NumberOfCharactersDepInsHTest
    {
        private readonly NumberOfCharactersDepInsH handler;
        public NumberOfCharactersDepInsHTest()
        {
            var m = new StringInsertModel("{NumberOfCharacters:X2}", "NumberOfCharacters", "", ":X2");
            handler = new NumberOfCharactersDepInsH(m);
        }
        
        
        [Fact]
        public void FirstOccurencyReplace_Test()
        {
            //Arrange
            var sb = new StringBuilder("{NumberOfCharacters:X2}\\\"42\\\"{NumberOfCharacters:X2}\\\"12:25\\\"{NumberOfCharacters:X2}\\\"5\\\"");
            //Act
            var res= handler.CalcInsert(sb);
            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.ToString().Should().Be( "0242{NumberOfCharacters:X2}\\\"12:25\\\"{NumberOfCharacters:X2}\\\"5\\\"");
        }
        
        
        [Fact]
        public void String_betveen_Test()
        {
            //Arrange
            var sb = new StringBuilder("{NumberOfCharacters:X2}01\\\"1234565))\\\"");
            
            //Act
            var res= handler.CalcInsert(sb);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.ToString().Should().Be( "09011234565))");
        }
        
        
        [Fact]
        public void Empty_String_Test()
        {
            //Arrange
            var sb = new StringBuilder("{NumberOfCharacters:X2}ggggggg\\\"\\\"");
            
            //Act
            var res= handler.CalcInsert(sb);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.ToString().Should().Be( "00ggggggg");
        }
        
        
        [Fact]
        public void Not_NumberOfCharacters_Replacement_Test()
        {
            //Arrange
            var sb = new StringBuilder("ggggggg\\\"\\\"");
            
            //Act
            var res= handler.CalcInsert(sb);

            //Assert
            res.IsSuccess.Should().BeFalse();
        }
        
        
        [Fact]
        public void Not_string_for_calc_Test()
        {
            //Arrange
            var sb = new StringBuilder("{NumberOfCharacters:X2}ggggggg");
            
            //Act
            var res= handler.CalcInsert(sb);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.ToString().Should().Be( "00ggggggg");
        }
    }
}