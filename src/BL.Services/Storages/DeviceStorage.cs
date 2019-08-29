
using DeviceForExchange;
using Infrastructure.Storages;
using InputDataModel.Base;
using InputDataModel.Base.InData;

namespace BL.Services.Storages
{
    public class DeviceStorage<TIn> : BaseStorage<string, Device<TIn>> where TIn : InputTypeBase
    {
        
    }
}