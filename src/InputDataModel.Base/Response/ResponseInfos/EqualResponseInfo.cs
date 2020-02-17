using Shared.Types;

namespace Domain.InputDataModel.Base.Response.ResponseInfos
{
    /// <summary>
    /// Интерпретатор ответа.
    /// IsOutDataValid выставляется при realData == expectedData
    /// </summary>
    public class EqualResponseInfo : BaseResponseInfo
    {
        public readonly StringRepresentation RealData;
        private readonly StringRepresentation _expectedData;                //Ожидаемые (верные) данные
        public EqualResponseInfo(StringRepresentation realData, StringRepresentation expectedData)
        {
            RealData = realData;
            _expectedData = expectedData;
            IsOutDataValid = RealData == _expectedData;
        }
        public override string ToString()
        {
            return $"{IsOutDataValid}   EqualResponse= {RealData}/{_expectedData}";
        }
    }
}