using Domain.InputDataModel.Base.Response.ResponseValidators;
using FluentAssertions;
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

        #region LenghtResponseValidator
        [Fact]
        public void LenghtResponseValidator_ByteArray_InData_Test()
        {
            //Arrange
            var v = new LenghtResponseValidator(10);

            //Act
            var respInfo = v.Validate(_inArray);
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeTrue();
            logStr.Should().Be("Valid: True  Type: LenghtResponseInfo  Info= 10 <---> 10");
        }

        [Fact]
        public void LenghtResponseValidator_ByteArray_ErroValidate_Test()
        {
            //Arrange
            var v = new LenghtResponseValidator(5);

            //Act
            var respInfo = v.Validate(_inArray);
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeFalse();
            logStr.Should().Be("Valid: False  Type: LenghtResponseInfo  Info= 10 <---> 5");
        }


        [Fact]
        public void LenghtResponseValidator_String_InData_Test()
        {
            //Arrange
            var v = new LenghtResponseValidator(10);

            //Act
            var respInfo = v.Validate("0123456789");      //Если ответ на массив байт, а строка.
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeTrue();
            logStr.Should().Be("Valid: True  Type: LenghtResponseInfo  Info= 10 <---> 10");
        }


        [Fact]
        public void LenghtResponseValidator_String_ErroValidate_Test()
        {
            //Arrange
            var v = new LenghtResponseValidator(5);

            //Act
            var respInfo = v.Validate("as");
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeFalse();
            logStr.Should().Be("Valid: False  Type: LenghtResponseInfo  Info= 2 <---> 5");
        }
        #endregion



        #region EqualResponseValidator
        [Fact]
        public void EqualResponseValidator_ByteArray_Test()
        {
            //Arrange
            var v = new EqualResponseValidator(new StringRepresentation("0102030405060708090A", "HEX"));

            //Act
            var respInfo = v.Validate(_inArray);
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeTrue();
            logStr.Should().Be("Valid: True  Type: EqualResponseInfo  Info= [0102030405060708090A]:HEX <---> [0102030405060708090A]:HEX");
        }


        [Fact]
        public void EqualResponseValidator_ByteArray_ErroValidate_Test()
        {
            //Arrange
            var v = new EqualResponseValidator(new StringRepresentation("FFFFFF", "HEX"));

            //Act
            var respInfo = v.Validate(_inArray);
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeFalse();
            logStr.Should().Be("Valid: False  Type: EqualResponseInfo  Info= [0102030405060708090A]:HEX <---> [FFFFFF]:HEX");
        }


        [Fact]
        public void EqualResponseValidator_Windows1251_Encoding_Test()
        {
            //Arrange
            var inArray = new byte[] { 0x41, 0x42 };
            var expectedFormat = "Windows-1251";
            var v = new EqualResponseValidator(new StringRepresentation("AB", expectedFormat));

            //Act
            var respInfo = v.Validate(inArray);
            var logStr = respInfo.ToString();

            //Asert
            respInfo.IsOutDataValid.Should().BeTrue();
            logStr.Should().Be("Valid: True  Type: EqualResponseInfo  Info= [AB]:Windows-1251 <---> [AB]:Windows-1251");
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
            logStr.Should().Be("Valid: True  Type: EqualResponseInfo  Info= [Data1]: <---> [Data1]:");
        }
        #endregion
    }
}