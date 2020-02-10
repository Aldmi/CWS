using System;
using System.Text;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Shared.Services.StringInseartService;
using Shared.Services.StringInseartService.DependentInseart.DependentInseartHandlers;
using Xunit;

namespace Shared.Test.DependentInseartTest.DependentInseartHandlersTest
{
    public class NbyteFullDepInsHTest
    {
        private readonly NbyteFullDepInsH _handler;
        public NbyteFullDepInsHTest()
        {
            var requiredModel = new StringInsertModel("{NbyteFull:X2}", "NbyteFull", "", ":X2");
            var crcModel = new StringInsertModel("{CRCMod256:X2}", "CRCMod256", "", ":X2");
            _handler = new NbyteFullDepInsH(requiredModel, crcModel);
        }
        
        
        [Fact]
        public void Calc_Normal_Test()
        {
            //Arrange
            var sb = new StringBuilder("0x050x{NbyteFull:X2}0x03^52^ 10:250x{CRCMod256:X2}");
            var format = "cp866";
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be( "0x050x0E0x03^52^ 10:250x{CRCMod256:X2}");
        }
        
        
        [Fact]
        public void Calc_Zero_NbyteFull_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201{NbyteFull:X2}{CRCMod256:X2}\u0003" );
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be( "\u00020103{CRCMod256:X2}\u0003");  //27
        }
        
        
        [Fact]
        public void Not_NbyteFull_Marker_In_Str_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201%010C60EF03B0470000001E%110{CRCMod256:X2}\u0003" );
            //Act
            var (isSuccess, _, _, error)= _handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeFalse();
            error.Should().Be( "Обработчик Dependency Inseart не может найти Replacement переменную {NbyteFull:X2} в строке \u000201%010C60EF03B0470000001E%110{CRCMod256:X2}\u0003");
        }
        
        
        [Fact]
        public void Not_Crc_Marker_in_str_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201{NbyteFull:X2}%010C60EF03B0470000001E%110\u0003");
            //Act
            var (isSuccess, _, _, error) = _handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeFalse();
            error.Should().Be( "Невозможно выделить подстроку использую паттерн {NbyteFull:X2}(.*){CRCMod256:X2}"); 
        }
        
        
        [Fact]
        public void Not_Crc_optionalModel_Test()
        {
            //Arrange
            var requiredModel = new StringInsertModel("{NbyteFull:X2}", "NbyteFull", "", ":X2");
            StringInsertModel optionalModel = null;
            
            //Act & Asert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(()=> new NbyteFullDepInsH(requiredModel, optionalModel));
            exception.Should().BeOfType<ArgumentNullException>();
            exception.Message.Should().Contain("Value cannot be null.\r\nParameter name: crcModel");
        }
    }
}