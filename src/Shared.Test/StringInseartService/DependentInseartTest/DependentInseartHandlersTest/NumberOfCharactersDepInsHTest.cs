using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}