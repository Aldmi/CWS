using App.Services.Actions;
using Autofac;
using Domain.Exchange.Behaviors;
using Domain.InputDataModel.Base.InData;

namespace WebApiSwc.AutofacModules
{
    public class BehaviorExchangeAutofacModule<TIn> : Module where TIn : InputTypeBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CycleBehavior<TIn>>().InstancePerDependency();
            builder.RegisterType<OnceBehavior<TIn>>().InstancePerDependency();
            builder.RegisterType<CommandBehavior<TIn>>().InstancePerDependency();
        }
    }
}