using Shared.Types;

namespace Domain.InputDataModel.Base.Response.ResponseInfos
{
    /// <summary>
    /// Интерпретатор ответа.
    /// IsOutDataValid выставляется даже при отсутсвии данных.
    /// </summary>
    public class DisabledResponseInfo : BaseResponseInfo
    {
        public readonly StringRepresentation RealData;

        public DisabledResponseInfo(StringRepresentation realData)
        {
            RealData = realData;
            IsOutDataValid = true;
        }

        public override string ToString()
        {
            return $"{base.ToString()}  Info= {RealData}";
        }
    }
}