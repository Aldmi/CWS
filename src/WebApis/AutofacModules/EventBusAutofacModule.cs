using Autofac;
using Infrastructure.EventBus.Abstract;
using Infrastructure.EventBus.Concrete;

namespace WebApiSwc.AutofacModules
{
    public class EventBusAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MemEventBus>().As<IEventBus>().SingleInstance();
        }
    }
}