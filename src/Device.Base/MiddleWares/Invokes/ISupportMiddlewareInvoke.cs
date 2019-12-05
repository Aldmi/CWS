using CSharpFunctionalExtensions;
using Domain.InputDataModel.Base.InData;

namespace Domain.Device.MiddleWares.Invokes
{
    public interface IMiddlewareInData<TIn>
    {
        Result<InputData<TIn>, ErrorResultMiddleWareInData> HandleInvoke(InputData<TIn> inData);
    }
}