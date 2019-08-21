using Autofac;
using BL.Services.Mediators;
using InputDataModel.Base;
using InputDataModel.Base.InData;
using Module = Autofac.Module;

namespace WebApiSwc.AutofacModules
{
    public class MediatorsAutofacModule<TIn> : Module where TIn : InputTypeBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MediatorForStorages<TIn>>().InstancePerDependency();
            builder.RegisterType<MediatorForOptions>().InstancePerDependency();
        }
    }
}