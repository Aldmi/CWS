//using System;
//using System.Text;
//using CSharpFunctionalExtensions;
//using Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers;
//using Domain.InputDataModel.Shared.StringInseartService.Model;
//using FluentAssertions;
//using Xunit;

//namespace Shared.Test.DependentInseartTest.DependentInseartHandlersTest
//{
//    public class NcharDepInsHTest
//    {
//        private readonly NcharDepInsH _handler;
//        public NcharDepInsHTest()
//        {
//            var requiredModel = new StringInsertModel("{Nchar:X2}", "Nchar","", ":D2");
//            var crcModel = new StringInsertModel("{CRCXorInverse:X2}", "CRCXorInverse", "", ":X2");
//            _handler = new NcharDepInsH(requiredModel, crcModel);
//        }
        
        
//        [Fact]
//        public void Calc_Normal_Test()
//        {
//            //Arrange
//            var sb = new StringBuilder("\u000201{Nchar:X2}%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003" );
//            //Act
//            var (isSuccess, _, value) = _handler.CalcInsert(sb);
//            //Assert
//            isSuccess.Should().BeTrue();
//            value.ToString().Should().Be( "\u00020127%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003");  //27
//        }
        
        
//        [Fact]
//        public void Calc_Zero_Nchar_Test()
//        {
//            //Arrange
//            var sb = new StringBuilder("\u000201{Nchar:X2}{CRCXorInverse:X2}\u0003" );
//            //Act
//            var (isSuccess, _, value) = _handler.CalcInsert(sb);
//            //Assert
//            isSuccess.Should().BeTrue();
//            value.ToString().Should().Be( "\u00020100{CRCXorInverse:X2}\u0003");  //27
//        }
        
        
//        [Fact]
//        public void Not_Nchar_Marker_In_Str_Test()
//        {
//            //Arrange
//            var sb = new StringBuilder("\u000201%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003" );
//            //Act
//            var (isSuccess, _, _, error)= _handler.CalcInsert(sb);
//            //Assert
//            isSuccess.Should().BeFalse();
//            error.Should().Be( "Обработчик Dependency Inseart не может найти Replacement переменную {Nchar:X2} в строке \u000201%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003");
//        }
        
        
//        [Fact]
//        public void Not_Crc_Marker_in_str_Test()
//        {
//            //Arrange
//            var sb = new StringBuilder("\u000201{Nchar:X2}%010C60EF03B0470000001E%110\u0003");
//            //Act
//            var (isSuccess, _, _, error) = _handler.CalcInsert(sb);
//            //Assert
//            isSuccess.Should().BeFalse();
//            error.Should().Be( "Обработка NcharDepInsH не может быт выполненна. Не найдена замена в строке {CRCXorInverse:X2}"); 
//        }
        
        
//        [Fact]
//        public void Not_Crc_optionalModel_Test()
//        {
//            //Arrange
//            var requiredModel = new StringInsertModel("{Nchar:X2}", "Nchar", "", ":D2");
//            StringInsertModel optionalModel = null;

//            //Act & Asert
//            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new NcharDepInsH(requiredModel, optionalModel));
//            exception.Should().BeOfType<ArgumentNullException>();
//            exception.Message.Should().Contain("Value cannot be null. (Parameter 'crcModel')");
//        }
//    }
//}