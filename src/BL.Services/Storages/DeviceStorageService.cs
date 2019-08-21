
using DeviceForExchange;
using InputDataModel.Base;
using InputDataModel.Base.InData;

namespace BL.Services.Storages
{
    public class DeviceStorageService<TIn> : StorageServiceBase<string, Device<TIn>> where TIn : InputTypeBase
    {
        
    }
}