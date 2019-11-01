using Autofac;
using Domain.Device;
using Domain.Device.MiddleWares;
using Domain.Device.MiddleWares.Invokes;
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

            builder.RegisterType<MiddleWareInData<T>>().As<IMiddlewareInData<T>>().InstancePerDependency();
            
        }
    }
}