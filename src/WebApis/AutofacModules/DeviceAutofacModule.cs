using Autofac;
using Domain.Device;
using Domain.Device.MiddleWares4InData;
using Domain.Device.MiddleWares4InData.Invokes;
using Domain.Device.Services;
using Domain.InputDataModel.Base.InData;

namespace WebApiSwc.AutofacModules
{
    public class DeviceAutofacModule<TIn> : Module where TIn : InputTypeBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Device<TIn>>().AsSelf().InstancePerDependency();
            builder.RegisterType<AllExchangesResponseAnaliticService>().AsSelf().InstancePerDependency();
            builder.RegisterType<MiddlewareInvokeService<TIn>>().AsSelf().InstancePerDependency();

            builder.RegisterType<MiddleWareMediator<TIn>>().As<ISupportMiddlewareInvoke<TIn>>().InstancePerDependency();

            builder.RegisterType<ProduserAdapter<TIn>>().AsSelf().InstancePerDependency();
        }
    }
}