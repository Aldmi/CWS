namespace Domain.InputDataModel.Base.Response.ResponseInfos
{
    public class LenghtResponseInfo : BaseResponseInfo
    {
        public readonly int RealLenght;
        public readonly int ExpectedLenght;                //Ожидаемые (верные) данные
        public LenghtResponseInfo(int realLenght, int expectedLenght)
        {
            RealLenght = realLenght;
            ExpectedLenght = expectedLenght;
            IsOutDataValid = RealLenght == ExpectedLenght;
        }
        public override string ToString()
        {
            return $"{IsOutDataValid}   ArrayLenght=  {RealLenght}/{ExpectedLenght}";
        }
    }
}