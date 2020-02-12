using Domain.InputDataModel.Base.Response.ResponseInfos;

namespace Domain.InputDataModel.Base.Response.ResponseValidators
{
    public abstract class BaseResponseValidator
    {
        public abstract BaseResponseInfo Validate(byte[] arr);
        public abstract BaseResponseInfo Validate(string str);
    }
}