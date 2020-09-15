using Autofac;
using Domain.InputDataModel.Autodictor.ForProviderImpl.IndependentInseartsImpl.Factory;
using Domain.InputDataModel.Autodictor.ProvidersSpecial;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders;
using Domain.InputDataModel.Shared.StringInseartService.IndependentInseart.IndependentInseartHandlers;

namespace WebApiSwc.AutofacModules
{
    /// <summary>
    /// Регистрируем КОНКРЕТНЫЕ провайдеры по типу.
    /// </summary>
    public class DataProviderAutofacModule<TIn> : Module where TIn : InputTypeBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            switch (typeof(TIn).Name)
            {
                case "AdInputType":
                    builder.RegisterType<AdInputTypeIndependentInseartsHandlersFactory>().As<IIndependentInseartsHandlersFactory>().SingleInstance();

                    builder.RegisterType<VidorBinaryDataProvider>().Named<IDataProvider<TIn>>("VidorBinary").InstancePerDependency();
                    builder.RegisterType<ByRulesDataProvider<TIn>>().Named<IDataProvider<TIn>>("ByRules").InstancePerDependency();
                    break;

                case "OtherType":
                    //builder.RegisterType<OtherTypeStronglyTypedResponseFactory>().As<IStronglyTypedResponseFactory>().SingleInstance();
                    //builder.RegisterType<OtherDataProvider>().As<IExchangeDataProvider<TIn, TransportResponse>>().InstancePerDependency();
                    break;
            }

            builder.RegisterType<ProviderResult<TIn>>().InstancePerDependency();
        }
    }
}