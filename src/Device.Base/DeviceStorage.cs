using Domain.InputDataModel.Base.InData;
using Infrastructure.Storages;

namespace Domain.Device
{
    public class DeviceStorage<TIn> : BaseStorage<string, Device<TIn>> where TIn : InputTypeBase
    {
        
    }
}