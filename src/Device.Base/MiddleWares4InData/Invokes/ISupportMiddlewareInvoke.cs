using CSharpFunctionalExtensions;
using Domain.InputDataModel.Base.InData;
using Shared.MiddleWares.Converters;

namespace Domain.Device.MiddleWares4InData.Invokes
{
    public interface ISupportMiddlewareInvoke<TIn>
    {
        Result<InputData<TIn>, ErrorResultMiddleWareInData> HandleInvoke(InputData<TIn> inData);
        void SendCommand4MemConverters(MemConverterCommand command);
    }
}