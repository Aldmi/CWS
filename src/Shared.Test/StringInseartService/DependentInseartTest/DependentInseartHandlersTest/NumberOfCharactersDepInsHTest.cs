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
    public class NumberOfCharactersDepInsHTest
    {
        private readonly NumberOfCharactersDepInsH _handler;
        private readonly IReadOnlyDictionary<string, StringInsertModelExt> _extDict;
        public NumberOfCharactersDepInsHTest()
        {
            _extDict = GetStringInsertModelExtDict.SimpleDictionary;
            var requiredModel = StringInsertModelFactory.CreateList("{NumberOfCharacters:X2}", _extDict).First();
            _handler = new NumberOfCharactersDepInsH(requiredModel);
        }


        [Fact]
        public void FirstOccurencyReplace_Test()
        {
            //Arrange
            var sb = new StringBuilder("{NumberOfCharacters:X2}\\\"42\\\"{NumberOfCharacters:X2}\\\"12:25\\\"{NumberOfCharacters:X2}\\\"5\\\"");
            //Act
            var res = _handler.CalcInsert(sb);
            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.ToString().Should().Be("0242{NumberOfCharacters:X2}\\\"12:25\\\"{NumberOfCharacters:X2}\\\"5\\\"");
        }


        [Fact]
        public void String_betveen_Test()
        {
            //Arrange
            var sb = new StringBuilder("{NumberOfCharacters:X2}01\\\"1234565))\\\"");

            //Act
            var res = _handler.CalcInsert(sb);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.ToString().Should().Be("09011234565))");
        }


        [Fact]
        public void Empty_String_Test()
        {
            //Arrange
            var sb = new StringBuilder("{NumberOfCharacters:X2}ggggggg\\\"\\\"");

            //Act
            var res = _handler.CalcInsert(sb);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.ToString().Should().Be("00ggggggg");
        }


        [Fact]
        public void Not_NumberOfCharacters_Replacement_Test()
        {
            //Arrange
            var sb = new StringBuilder("ggggggg\\\"\\\"");

            //Act
            var res = _handler.CalcInsert(sb);

            //Assert
            res.IsSuccess.Should().BeFalse();
        }


        [Fact]
        public void Not_string_for_calc_Test()
        {
            //Arrange
            var sb = new StringBuilder("{NumberOfCharacters:X2}ggggggg");

            //Act
            var res = _handler.CalcInsert(sb);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.ToString().Should().Be("00ggggggg");
        }



        [Fact]
        public void CheckError_Format_Not_Found_Test()
        {
            //Arrange
            var requiredModel = StringInsertModelFactory.CreateList("{NumberOfCharacters:D99}", _extDict).First();
            var handler = new NumberOfCharactersDepInsH(requiredModel);
            var sb = new StringBuilder("\u000201{NumberOfCharacters:D99}%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003");

            //Act
            var (isSuccess, _, value, error) = handler.CalcInsert(sb);

            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("\u000201!!!ExtKeyNotFound!!!%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003");
        }


        [Fact]
        public void CheckError_Not_NumberOfCharacters_Marker_In_Str_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003");
            //Act
            var (isSuccess, _, _, error) = _handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeFalse();
            error.Should().Be("Обработчик Dependency Inseart не может найти Replacement переменную {NumberOfCharacters:X2} в строке \u000201%010C60EF03B0470000001E%110{CRCXorInverse:X2}\u0003");
        }
    }
}