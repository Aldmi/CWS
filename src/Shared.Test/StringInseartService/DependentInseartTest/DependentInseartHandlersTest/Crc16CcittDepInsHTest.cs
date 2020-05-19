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
            var requiredModel = StringInsertModelFactory.CreateList("{CRCCcitt:X4_Border}", _extDict).First();
            _handler = new Crc16CcittDepInsH(requiredModel);
        }


        [Fact]
        public void Calc_Normal_Test()
        {
            //Arrange
            var sb = new StringBuilder("0x020000S202002171307090x03{CRCCcitt:X4_Border}0x04");
            var format = "Windows-1251";
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("0x020000S202002171307090x038A7D0x04");
        }


        [Fact]
        public void Response_Test()
        {
            //Arrange
            var sb = new StringBuilder("0xFF0xFF0xFF0x0208000x060x03{CRCCcitt:X4_Border}0x04");
            var format = "Windows-1251";
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("0xFF0xFF0xFF0x0208000x060x03143E0x04");
        }
    }
}