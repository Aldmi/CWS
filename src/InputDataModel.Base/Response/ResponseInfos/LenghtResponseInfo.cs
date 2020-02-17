namespace Domain.InputDataModel.Base.Response.ResponseInfos
{
    /// <summary>
    /// Интерпретатор ответа.
    /// IsOutDataValid выставляется при realLenght == expectedLenght
    /// </summary>
    public class LenghtResponseInfo : BaseResponseInfo
    {
        public readonly int RealLenght;
        private readonly int _expectedLenght;                //Ожидаемые (верные) данные
        public LenghtResponseInfo(int realLenght, int expectedLenght)
        {
            RealLenght = realLenght;
            _expectedLenght = expectedLenght;
            IsOutDataValid = RealLenght == _expectedLenght;
        }
        public override string ToString()
        {
            return $"{IsOutDataValid}   ArrayLenght=  {RealLenght}/{_expectedLenght}";
        }
    }
}