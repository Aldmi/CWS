using Autofac;
using Domain.Device;
using Domain.Device.Produser;
using Domain.Exchange;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Domain.InputDataModel.Shared.StringInseartService.Model.InlineStringInsert;
using Infrastructure.Background;
using Infrastructure.Transport;

namespace WebApiSwc.AutofacModules
{
    /// <summary>
    /// Регистрируем сервисы хранения бизнесс логики. (оперативные данные, хранятся в памяти (CuncurrentDictionary))
    /// </summary>
    public class BlStorageAutofacModule<TIn> : Module where TIn : InputTypeBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TransportStorage>().SingleInstance(); 
            builder.RegisterType<BackgroundStorage>().SingleInstance();
            builder.RegisterType<ExchangeStorage<TIn>>().SingleInstance();
            builder.RegisterType<DeviceStorage<TIn>>().SingleInstance();
            builder.RegisterType<ProduserUnionStorage<TIn>>().SingleInstance();
            builder.RegisterType<StringInsertModelExtStorage>().SingleInstance();
            builder.RegisterType<InlineStringInsertModelStorage>().SingleInstance();
        }
    }
}