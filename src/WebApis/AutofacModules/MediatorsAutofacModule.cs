using App.Services.Mediators;
using Autofac;
using Domain.InputDataModel.Base.InData;
using Module = Autofac.Module;

namespace WebApiSwc.AutofacModules
{
    public class MediatorsAutofacModule<TIn> : Module where TIn : InputTypeBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MediatorForStorages<TIn>>().InstancePerDependency();           //SingleInstance - т.к. в его Scope находится _dataProviderFactory, который 
            builder.RegisterType<MediatorForDeviceOptions>().InstancePerDependency();
            builder.RegisterType<MediatorForProduserUnionOptions>().InstancePerDependency();
        }
    }
}