using Autofac;
using Domain.InputDataModel.Autodictor.IndependentInsearts.Factory;
using Domain.InputDataModel.Autodictor.ProvidersSpecial;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders;
using Domain.InputDataModel.Base.Response;
using Domain.InputDataModel.Base.Response.ResponseInfos;
using Shared.Services.StringInseartService.IndependentInseart;

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

                    builder.RegisterType<VidorBinaryDataProvider>().Named<IDataProvider<TIn, BaseResponseInfo>>("VidorBinary").InstancePerDependency();
                    builder.RegisterType<ByRulesDataProvider<TIn>>().Named<IDataProvider<TIn, BaseResponseInfo>>("ByRules").InstancePerDependency();
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