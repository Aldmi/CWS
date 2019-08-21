using Autofac;
using BL.Services.Storages;
using InputDataModel.Base;
using InputDataModel.Base.InData;

namespace WebApiSwc.AutofacModules
{
    /// <summary>
    /// Регистрируем сервисы хранения бизнесс логики. (оперативные данные, хранятся в памяти (CuncurrentDictionary))
    /// </summary>
    public class BlStorageAutofacModule<TIn> : Module where TIn : InputTypeBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TransportStorageService>().SingleInstance(); 
            builder.RegisterType<BackgroundStorageService>().SingleInstance();
            builder.RegisterType<ExchangeStorageService<TIn>>().SingleInstance();
            builder.RegisterType<DeviceStorageService<TIn>>().SingleInstance();
        }
    }
}