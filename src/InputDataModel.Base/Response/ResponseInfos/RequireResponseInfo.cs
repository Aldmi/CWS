using System.Collections.Generic;
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
            IsOutDataValid= realData != null;
        }

        public override string ToString()
        {
            return $"{IsOutDataValid}   RequireResponseInfo= {RealData}";
        }
    }
}