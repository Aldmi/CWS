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
    public class Crc16CcittDepInsHTest
    {
        private readonly Crc16CcittDepInsH _handler;
        private readonly IReadOnlyDictionary<string, StringInsertModelExt> _extDict;
        public Crc16CcittDepInsHTest()
        {
            _extDict = GetStringInsertModelExtDict.SimpleDictionary;
            var requiredModel = StringInsertModelFactory.CreateList("{CRCCcitt:X4_BorderLeft}", _extDict).First();
            _handler = new Crc16CcittDepInsH(requiredModel);
        }


        [Fact]
        public void Calc_Normal_Test()
        {
            //Arrange
            var sb = new StringBuilder("0x020000S202002171307090x03{CRCCcitt:X4_BorderLeft}0x04");
            var format = "Windows-1251";
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            //value.ToString().Should().Be("0x020000S202002171307090x038A7D0x04");
            value.ToString().Should().Be("0x020000S202002171307090x03070D0x04");
        }


        [Fact]
        public void Response_Test()
        {
            //Arrange
            var sb = new StringBuilder("0xFF0xFF0xFF0x0208000x060x03{CRCCcitt:X4_BorderLeft}0x04");
            var format = "Windows-1251";
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            //value.ToString().Should().Be("0xFF0xFF0xFF0x0208000x060x03143E0x04");
            value.ToString().Should().Be("0xFF0xFF0xFF0x0208000x060x03A66A0x04");
        }


        [Fact]
        public void Empty_String_Test()
        {
            //Arrange
            var sb = new StringBuilder("0x{CRCCcitt:X4_BorderLeft}");

            //Act
            var format = "Windows-1251";
            var (isSuccess, _, value, error) = _handler.CalcInsert(sb, format);

            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("0xFFFF");
        }


        [Fact]
        public void CheckError_Format_WithOut_Border_Test()
        {
            //Arrange
            var requiredModel = StringInsertModelFactory.CreateList("{CRCCcitt:X4}", _extDict).First();
            var handler = new Crc16CcittDepInsH(requiredModel);
            var sb = new StringBuilder("0xFF0xFF0xFF0x0208000x060x03{CRCCcitt:X4}0x04");
            var format = "Windows-1251";
            //Act
            var (isSuccess, _, value, error) = handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeFalse();
            error.Should().Be("Для любого алгоритма CRC подстрока определяется BorderSubString. ОБЯЗАТЕЛЬНО ЗАДАЙТЕ BorderSubString.");
        }
    }
}