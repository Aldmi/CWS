using System;
using Domain.InputDataModel.Base.Response.ResponseValidators;
using FluentAssertions;
using Shared.Helpers;
using Shared.Services.StringInseartService;
using Shared.Types;
using Xunit;

namespace InputDataModel.Autodictor.Test
{
    public class ResponseValidatorsTest
    {
        private readonly byte[] _inArray;
        public ResponseValidatorsTest()
        {
            _inArray = new byte[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        }


        [Fact]
        public void ArrayLenghtResponseValidator_SuccsesfullValidate_Test()
        {
            //Arrange
            var v = new LenghtResponseValidator(10);
            
            //Act
            var respInfo = v.Validate(_inArray);
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeTrue();
            logStr.Should().Be("True   ArrayLenght=  10/10");
        }

        [Fact]
        public void ArrayLenghtResponseValidator_String_SuccsesfullValidate_Test()
        {
            //Arrange
            var v = new LenghtResponseValidator(10);

            //Act
            var respInfo = v.Validate("0123456789");
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeTrue();
            logStr.Should().Be("True   ArrayLenght=  10/10");
        }

        [Fact]
        public void ArrayLenghtResponseValidator_ErroValidate_Test()
        {
            //Arrange
            var v = new LenghtResponseValidator(5);

            //Act
            var respInfo = v.Validate(_inArray);
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeFalse();
            logStr.Should().Be("False   ArrayLenght=  10/5");
        }


        [Fact]
        public void EqualResponseValidator_SuccsesfullValidate_Test()
        {
            //Arrange
            var v = new EqualResponseValidator(new StringRepresentation("0102030405060708090A", "HEX"));

            //Act
            var respInfo = v.Validate(_inArray);
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeTrue();
            logStr.Should().Be("True   EqualResponse= [0102030405060708090A]:HEX/[0102030405060708090A]:HEX");
        }


        [Fact]
        public void EqualResponseValidator_ErroValidate_Test()
        {
            //Arrange
            var v = new EqualResponseValidator(new StringRepresentation("FFFFFF", "HEX"));

            //Act
            var respInfo = v.Validate(_inArray);
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeFalse();
            logStr.Should().Be("False   EqualResponse= [0102030405060708090A]:HEX/[FFFFFF]:HEX");
        }


        [Fact]
        public void EqualResponseValidator_Windows1251_Encoding_Test()
        {
            //Arrange
            var inArray = new byte[] { 0x41, 0x42};
            var expectedFormat = "Windows-1251";
            var v = new EqualResponseValidator(new StringRepresentation("AB", expectedFormat));

            //Act
            var respInfo = v.Validate(inArray);
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeTrue();
            logStr.Should().Be("True   EqualResponse= [AB]:Windows-1251/[AB]:Windows-1251");
        }


        [Fact]
        public void EqualResponseValidator_String_InData_Test()
        {
            //Arrange
            var inData = "Data1";
            var v = new EqualResponseValidator(new StringRepresentation(inData, ""));

            //Act
            var respInfo = v.Validate(inData);
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeTrue();
            logStr.Should().Be("True   EqualResponse= [Data1]:/[Data1]:");
        }
    }
}