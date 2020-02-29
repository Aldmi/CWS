using Domain.InputDataModel.Base.Response.ResponseInfos;
using Shared.Types;

namespace Domain.InputDataModel.Base.Response.ResponseValidators
{
    /// <summary>
    /// Создает Валидатор, проверяющий входные данные на точное соответствие Ожидаемым данным
    /// </summary>
    public class EqualResponseValidator : BaseResponseValidator
    {
        public readonly StringRepresentation ExpectedData;                  //Ожидаемые (верные) данные
        public EqualResponseValidator(StringRepresentation expectedData)
        {
            ExpectedData = expectedData;
        }

        public override BaseResponseInfo Validate(byte[] arr)
        {
            var realData = StringRepresentation.Create(arr, ExpectedData.Format);
            return new EqualResponseInfo(realData, ExpectedData);
        }

        public override BaseResponseInfo Validate(string str)
        {
            var realData = new StringRepresentation(str, "");
            return new EqualResponseInfo(realData, ExpectedData);
        }
    }
}