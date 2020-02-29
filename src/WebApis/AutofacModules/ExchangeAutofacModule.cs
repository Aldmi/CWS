using Autofac;
using Domain.Exchange;
using Domain.Exchange.Behaviors;
using Domain.Exchange.Services;
using Domain.InputDataModel.Base.InData;

namespace WebApiSwc.AutofacModules
{
    public class ExchangeAutofacModule<TIn> : Module where TIn : InputTypeBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Exchange<TIn>>().As<IExchange<TIn>>().AsSelf().InstancePerDependency();

            builder.RegisterType<CycleBehavior<TIn>>().InstancePerDependency();
            builder.RegisterType<OnceBehavior<TIn>>().InstancePerDependency();
            builder.RegisterType<CommandBehavior<TIn>>().InstancePerDependency();

            builder.RegisterType<InputCycleDataEntryCheker>().InstancePerDependency();
            builder.RegisterType<SkippingPeriodChecker>().InstancePerDependency();
        }
    }
}