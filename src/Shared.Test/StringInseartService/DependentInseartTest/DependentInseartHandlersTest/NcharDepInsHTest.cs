using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using FluentAssertions;
using Shared.Test.StringInseartService.Datas;
using Xunit;

namespace Shared.Test.StringInseartService.DependentInseartTest.DependentInseartHandlersTest
{
    public class NcharDepInsHTest
    {
        private readonly NcharDepInsH _handler;
        private readonly IReadOnlyDictionary<string, StringInsertModelExt> _extDict;
        public NcharDepInsHTest()
        {
            _extDict = GetStringInsertModelExtDict.SimpleDictionary;
            var requiredModel = StringInsertModelFactory.CreateList("{Nchar:D2_NcharBorder}", _extDict).First();
            _handler = new NcharDepInsH(requiredModel);
        }


        [Fact]
        public void Calc_Normal_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201{Nchar:D2_NcharBorder}%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003");
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("\u00020127%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003");  //27
        }


        [Fact]
        public void Error_Format_Not_Found_Test()
        {
            //Arrange
            var requiredModel = StringInsertModelFactory.CreateList("{Nchar:D99}", _extDict).First();
            var handler = new NcharDepInsH(requiredModel);
            var sb = new StringBuilder("\u000201{Nchar:D99}%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003");
            //Act
            var (isSuccess, _, value, error) = handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("\u000201!!!ExtKeyNotFound!!!%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003");
        }


        [Fact]
        public void Calc_Zero_Nchar_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201{Nchar:D2_NcharBorder}{CRCXorInverse:X2}\u0003");
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("\u00020100{CRCXorInverse:X2}\u0003");  //27
        }


        [Fact]
        public void Not_Nchar_Marker_In_Str_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003");
            //Act
            var (isSuccess, _, _, error) = _handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeFalse();
            error.Should().Be("Обработчик Dependency Inseart не может найти Replacement переменную {Nchar:D2_NcharBorder} в строке \u000201%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003");
        }


        [Fact]
        public void Not_Crc_Marker_in_str_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201{Nchar:D2_NcharBorder}%010C60EF03B0470000001E%110\u0003");
            //Act
            var (isSuccess, _, _, error) = _handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeFalse();
            error.Should().Be("Not Found endCh= {CRC");
        }


        //[Fact]
        //public void Not_Crc_optionalModel_Test()
        //{
        //    //Arrange
        //    var requiredModel = StringInsertModelFactory.CreateList("{Nchar:D2}", _extDict).First();
        //    StringInsertModel optionalModel = null;

        //    //Act & Asert
        //    ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new NcharDepInsH(requiredModel, optionalModel));
        //    exception.Should().BeOfType<ArgumentNullException>();
        //    exception.Message.Should().Contain("Value cannot be null. (Parameter 'crcModel')");
        //}
    }
}