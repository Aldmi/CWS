using Autofac;
using Domain.Device;
using Domain.Device.MiddleWares4InData;
using Domain.Device.MiddleWares4InData.Invokes;
using Domain.Device.Services;
using Domain.InputDataModel.Base.InData;

namespace WebApiSwc.AutofacModules
{
    public class DeviceAutofacModule<T> : Module where T : InputTypeBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Device<T>>().AsSelf().InstancePerDependency();
            builder.RegisterType<AllExchangesResponseAnaliticService>().AsSelf().InstancePerDependency();
            builder.RegisterType<MiddlewareInvokeService<T>>().AsSelf().InstancePerDependency();

            builder.RegisterType<MiddleWareMediator<T>>().As<ISupportMiddlewareInvoke<T>>().InstancePerDependency();
            
        }
    }
}