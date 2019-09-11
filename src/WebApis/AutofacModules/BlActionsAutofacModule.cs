using App.Services.Actions;
using Autofac;
using Domain.InputDataModel.Base.InData;

namespace WebApiSwc.AutofacModules
{
    public class BlActionsAutofacModule<TIn> : Module where TIn : InputTypeBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DeviceActionService<TIn>>().InstancePerDependency();    
            builder.RegisterType<BuildDeviceService<TIn>>().InstancePerDependency();
            builder.RegisterType<BuildProdusersUnionService<TIn>>().InstancePerDependency();
            
        }
    }
}