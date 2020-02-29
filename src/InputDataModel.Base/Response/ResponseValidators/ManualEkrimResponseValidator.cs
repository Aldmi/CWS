using Domain.InputDataModel.Base.Response.ResponseInfos;

namespace Domain.InputDataModel.Base.Response.ResponseValidators
{
    /// <summary>
    /// Создает Валидатор, для протокола Ekrim.
    /// </summary>
    public class ManualEkrimResponseValidator : BaseResponseValidator
    {
        public override BaseResponseInfo Validate(byte[] arr)
        {
            return ManualEkrimResponseInfo.CreateInstanse(arr);
        }
        public override BaseResponseInfo Validate(string str)
        {
            throw new System.NotImplementedException();
        }
    }
}