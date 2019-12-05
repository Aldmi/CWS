using Autofac.Features.OwnedInstances;
using Domain.InputDataModel.Base.InData;
using Infrastructure.Storages;

namespace Domain.Device
{
    public class DeviceStorage<TIn> : BaseStorage<string, Owned<Device<TIn>>> where TIn : InputTypeBase
    {
        
    }
}