using Domain.InputDataModel.Base.Response.ResponseInfos;

namespace Domain.InputDataModel.Base.Response.ResponseValidators
{

    public class LenghtResponseValidator : BaseResponseValidator
    {
        private readonly int _expectedLenght;                            //Ожидаемая длинна массива
        public LenghtResponseValidator(int expectedLenght)
        {
            _expectedLenght = expectedLenght;
        }

        public override BaseResponseInfo Validate(byte[] arr)
        {
            var realLenght = arr.Length;
            return new LenghtResponseInfo(realLenght, _expectedLenght);
        }
        public override BaseResponseInfo Validate(string str)
        {
            var realLenght = str.Length;
            return new LenghtResponseInfo(realLenght, _expectedLenght);
        }
    }
}