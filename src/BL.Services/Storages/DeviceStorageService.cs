
using DeviceForExchange;
using InputDataModel.Base;

namespace BL.Services.Storages
{
    public class DeviceStorageService<TIn> : StorageServiceBase<string, Device<TIn>> where TIn : InputTypeBase
    {
        
    }
}