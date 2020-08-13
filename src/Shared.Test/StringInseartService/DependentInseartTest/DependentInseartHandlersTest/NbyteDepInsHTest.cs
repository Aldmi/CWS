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
    public class NbyteDepInsHTest
    {
        private readonly NbyteDepInsH _handler;
        private readonly IReadOnlyDictionary<string, StringInsertModelExt> _extDict;

        public NbyteDepInsHTest()
        {
            _extDict = GetStringInsertModelExtDict.SimpleDictionary;
            var requiredModel = StringInsertModelFactory.CreateList("{Nbyte:X2_BorderRightBeforeCrc_Math}", _extDict).First();
            _handler = new NbyteDepInsH(requiredModel);
        }


        [Fact]
        public void Calc_Nchar_Manual_Border_Test()
        {
            //Arrange
            var requiredModel = StringInsertModelFactory.CreateList("{Nbyte:X2_NbyteFull<%01-%1>_Math}", _extDict).First(); 
            var handler = new NbyteDepInsH(requiredModel);
            var sb = new StringBuilder("\u000201{Nbyte:X2_NbyteFull<%01-%1>_Math}0C60E%01F03B04700000010E%110{CRCXorInverse:X2}\u0003");
            var format = "cp866";
            //Act
            var (isSuccess, _, value, error) = handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("\u000201130C60E%01F03B04700000010E%110{CRCXorInverse:X2}\u0003");
        }



        [Fact]
        public void Calc_Normal_Test()
        {
            //Arrange
            var sb = new StringBuilder("0x050x{Nbyte:X2_BorderRightBeforeCrc_Math}0x03^52^ 10:250x{CRCMod256:X2}");
            var format = "cp866";
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb, format);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("0x050x0E0x03^52^ 10:250x{CRCMod256:X2}");
        }


        [Fact]
        public void Calc_Zero_NbyteFull_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201{Nbyte:X2_BorderRightBeforeCrc_Math}{CRCMod256:X2}\u0003");
            //Act
            var (isSuccess, _, value) = _handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeTrue();
            value.ToString().Should().Be("\u00020103{CRCMod256:X2}\u0003");  //27
        }


        [Fact]
        public void CheckError_Not_NbyteFull_Marker_In_Str_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201%010C60EF03B0470000001E%110{CRCMod256:X2}\u0003");
            //Act
            var (isSuccess, _, _, error) = _handler.CalcInsert(sb);
            //Assert
            isSuccess.Should().BeFalse();
            error.Should().Be("Обработчик Dependency Inseart не может найти Replacement переменную {Nbyte:X2_BorderRightBeforeCrc_Math} в строке \u000201%010C60EF03B0470000001E%110{CRCMod256:X2}\u0003");
        }


        [Fact]
        public void CheckError_Not_Crc_Marker_In_Str_Test()
        {
            //Arrange
            var sb = new StringBuilder("\u000201{Nbyte:X2_BorderRightBeforeCrc_Math}%010C60EF03B0470000001E%110\u0003");
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
        //    var requiredModel = StringInsertModelFactory.CreateList("{NbyteFull:X2}", _extDict).First();
        //    StringInsertModel optionalModel = null;

        //    //Act & Asert
        //    ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new NbyteDepInsH(requiredModel, optionalModel));
        //    exception.Should().BeOfType<ArgumentNullException>();
        //    exception.Message.Should().Contain("Value cannot be null. (Parameter 'crcModel')");
        //}
    }
}