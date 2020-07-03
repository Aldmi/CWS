using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.DependentInseart.DependentInseartHandlers.Crc;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using FluentAssertions;
using Shared.Test.StringInseartService.Datas;
using Xunit;

namespace Shared.Test.StringInseartService.DependentInseartTest.DependentInseartHandlersTest
{
    public class Crc8FullPolyDepInsHTest
    {
        private readonly Crc8FullPolyDepInsH _handler;
        private readonly IReadOnlyDictionary<string, StringInsertModelExt> _extDict;
        public Crc8FullPolyDepInsHTest()
        {
            _extDict = GetStringInsertModelExtDict.SimpleDictionary;
            var requiredModel = StringInsertModelFactory.CreateList("{CRC8FullPoly:X2_BorderRight}", _extDict).First();
            _handler = new Crc8FullPolyDepInsH(requiredModel);
        }


        [Fact]
        public void Crc8FullPoly_Calc_Test()
        {
            //Arrange
            var sb = new StringBuilder("0x160x480x{CRC8FullPoly:X2_BorderRight}0x010x850x5a0x120x030x000x000x000x000x000x010x020x000x180x800x200x310x320x35");
            var format = "Windows-1251";
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("0x160x480xCF0x010x850x5a0x120x030x000x000x000x000x000x010x020x000x180x800x200x310x320x35");
        }


        /// <summary>
        /// подстрока для подсчета CRC равна 0x160x480x
        /// </summary>
        [Fact] 
        public void Crc8FullPoly_Calc_BorderLeft_Test()
        {
            //Arrange
            var requiredModel = StringInsertModelFactory.CreateList("{CRC8FullPoly:X2_BorderLeft}", _extDict).First();
            var handler = new Crc8FullPolyDepInsH(requiredModel);
            var sb = new StringBuilder("0x160x480x{CRC8FullPoly:X2_BorderLeft}0x010x850x5a0x120x030x000x000x000x000x000x010x020x000x180x800x200x310x320x35");
            var format = "Windows-1251";
            //Act
            var (isSuccess, _, value) = handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("0x160x480x550x010x850x5a0x120x030x000x000x000x000x000x010x020x000x180x800x200x310x320x35");
        }


        [Fact]
        public void Empty_String_Test()
        {
            //Arrange
            var sb = new StringBuilder("0x{CRC8FullPoly:X2_BorderRight}");

            //Act
            var format = "Windows-1251";
            var (isSuccess, _, value, error) = _handler.CalcInsert(sb, format);

            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("0x00");
        }


        [Fact]
        public void CheckError_Format_WithOut_Border_Test()
        {
            //Arrange
            var requiredModel = StringInsertModelFactory.CreateList("{CRC8FullPoly:X4}", _extDict).First();
            var handler = new Crc16CcittDepInsH(requiredModel);
            var sb = new StringBuilder("0xFF0xFF0xFF0x0208000x060x03{CRC8FullPoly:X4}0x04");
            var format = "Windows-1251";
            //Act
            var (isSuccess, _, _, error) = handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeFalse();
            error.Should().Be("Для любого алгоритма CRC подстрока определяется BorderSubString. ОБЯЗАТЕЛЬНО ЗАДАЙТЕ BorderSubString.");
        }
    }
}