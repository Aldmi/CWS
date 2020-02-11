using Shared.Types;

namespace Domain.InputDataModel.Base.Response.ResponseInfos
{
    public class EqualResponseInfo : BaseResponseInfo
    {
        public readonly StringRepresentation RealData;
        public readonly StringRepresentation ExpectedData;                //Ожидаемые (верные) данные
        public EqualResponseInfo(StringRepresentation realData, StringRepresentation expectedData)
        {
            RealData = realData;
            ExpectedData = expectedData;
            IsOutDataValid = RealData == ExpectedData;
        }
        public override string ToString()
        {
            return $"{IsOutDataValid}   EqualResponse= {RealData}/{ExpectedData}";
        }
    }
}