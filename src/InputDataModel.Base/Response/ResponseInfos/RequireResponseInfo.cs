using Shared.Types;

namespace Domain.InputDataModel.Base.Response.ResponseInfos
{
    /// <summary>
    /// Интерпретатор ответа.
    /// IsOutDataValid выставляется при получении ЛЮБЫХ данных
    /// </summary>
    public class RequireResponseInfo : BaseResponseInfo
    {
        public readonly StringRepresentation RealData;

        public RequireResponseInfo(StringRepresentation realData)
        {
            RealData = realData;
            IsOutDataValid = RealData != null;
        }

        public override string ToString()
        {
            return $"{base.ToString()}  Info= {RealData}";
        }
    }
}