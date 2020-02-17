using Domain.InputDataModel.Base.Response.ResponseInfos;
using Shared.Types;

namespace Domain.InputDataModel.Base.Response.ResponseValidators
{
    /// <summary>
    /// Создает Валидатор, которому важно ТОЛЬКО само наличие данных.
    /// </summary>
    public class RequireResponseValidator : BaseResponseValidator
    {
        public override BaseResponseInfo Validate(byte[] arr)
        {
            var realData = StringRepresentation.Create(arr, "HEX");
            return new RequireResponseInfo(realData);
        }

        public override BaseResponseInfo Validate(string str)
        {
            var realData = new StringRepresentation(str, "");
            return new RequireResponseInfo(realData);
        }
    }
}