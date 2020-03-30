using CSharpFunctionalExtensions;
using Domain.Device.MiddleWares.Converters;
using Domain.InputDataModel.Base.InData;

namespace Domain.Device.MiddleWares.Invokes
{
    public interface ISupportMiddlewareInvoke<TIn>
    {
        Result<InputData<TIn>, ErrorResultMiddleWareInData> HandleInvoke(InputData<TIn> inData);
        void SendCommand4MemConverters(MemConverterCommand command);
    }
}