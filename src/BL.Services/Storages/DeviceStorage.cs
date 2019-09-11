using Domain.Device;
using Domain.InputDataModel.Base.InData;
using Infrastructure.Storages;

namespace App.Services.Storages
{
    public class DeviceStorage<TIn> : BaseStorage<string, Device<TIn>> where TIn : InputTypeBase
    {
        
    }
}