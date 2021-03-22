using System;
using Domain.InputDataModel.Base.Response.ResponseInfos;
using Shared.Types;

namespace Domain.InputDataModel.Base.Response.ResponseValidators
{
    /// <summary>
    /// Создает Валидатор, Который вадает true, даже при отсутствии ответа.
    /// </summary>
    public class DisabledResponseValidator : BaseResponseValidator
    {
        /// <summary>
        /// arr - может быть равен null
        /// </summary>
        public override BaseResponseInfo Validate(byte[] arr)
        {
            if (arr == null)
            {
                var nullData = StringRepresentation.CreateNull();
                return new DisabledResponseInfo(nullData);
            }
            var realData = StringRepresentation.Create(arr, "HEX");
            return new DisabledResponseInfo(realData);
        }

        public override BaseResponseInfo Validate(string str)
        {
            throw new NotImplementedException("DisabledResponseValidator не реалзизован для строкового ответа");
            //var realData = new StringRepresentation(str, "");
            //return new RequireResponseInfo(realData);
        }
    }
}