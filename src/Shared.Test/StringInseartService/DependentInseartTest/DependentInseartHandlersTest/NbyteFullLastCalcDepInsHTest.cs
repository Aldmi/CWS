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
    public class NbyteFullLastCalcDepInsHTest
    {
        private readonly NbyteFullLastCalcDepInsH _handler;
        private readonly IReadOnlyDictionary<string, StringInsertModelExt> _extDict;
        public NbyteFullLastCalcDepInsHTest()
        {
            _extDict = GetStringInsertModelExtDict.SimpleDictionary;
            var requiredModel = StringInsertModelFactory.CreateList("{NbyteFullLastCalc:X2}", _extDict).First();
            _handler = new NbyteFullLastCalcDepInsH(requiredModel);
        }


        [Fact]
        public void Calc_With_Format_Win1251_In_Hex_String()
        {
            //Arrange
            //16 48 cf 01 85 5a 12 03 00 00 00 00 00 01 02 00 18 80 20 31 32 35
            var sb = new StringBuilder("0x{NbyteFullLastCalc:X2}0x480xcf0x010x850x5a0x120x030x000x000x000x000x000x010x020x000x180x800x200x310x320x35");

            //Act
            var format = "Windows-1251";
            var res = _handler.CalcInsert(sb, format);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.ToString().Should().Be("0x160x480xcf0x010x850x5a0x120x030x000x000x000x000x000x010x020x000x180x800x200x310x320x35");
        }



        [Fact]
        public void Calc_With_Format_Win1251_In_String()
        {
            //Arrange
            var sb = new StringBuilder("0x{NbyteFullLastCalc:X2}0x480xcf Москва-Владивосток 0x02");

            //Act
            var format = "Windows-1251";
            var res = _handler.CalcInsert(sb, format);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.ToString().Should().Be("0x180x480xcf Москва-Владивосток 0x02");
        }


        [Fact]
        public void Calc_With_Format_HEX()
        {
            //Arrange
            var sb = new StringBuilder("{NbyteFullLastCalc:X2}48cf01855a12030000000000010200188020313235");

            //Act
            var format = "HEX";
            var res = _handler.CalcInsert(sb, format);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.ToString().Should().Be("1648cf01855a12030000000000010200188020313235");
        }


        [Fact]
        public void Empty_String_Test()
        {
            //Arrange
            var sb = new StringBuilder("0x{NbyteFullLastCalc:X2}");

            //Act
            var format = "Windows-1251";
            var res = _handler.CalcInsert(sb, format);

            //Assert
            res.IsSuccess.Should().BeTrue();
            res.Value.ToString().Should().Be("0x01");
        }


        [Fact]
        public void Not_string_for_calc_Test()
        {
            //Arrange
            var sb = new StringBuilder("0x22");

            //Act
            var format = "Windows-1251";
            var res = _handler.CalcInsert(sb, format);

            //Assert
            res.IsSuccess.Should().BeFalse();
            res.Error.Should().Be("Обработчик Dependency Inseart не может найти Replacement переменную {NbyteFullLastCalc:X2} в строке 0x22");
        }
    }
}