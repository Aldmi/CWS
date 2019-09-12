using AbstractProduser.AbstractProduser;
using Autofac;
using Domain.Device.Produser;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Autodictor.ProvidersSpecial;
using Domain.InputDataModel.Autodictor.StronglyTypedResponse;
using Domain.InputDataModel.Base;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders;
using Domain.InputDataModel.Base.Response;
using KafkaProduser;
using KafkaProduser.Options;
using WebClientProduser;
using WebClientProduser.Options;

namespace WebApiSwc.AutofacModules
{
    /// <summary>
    /// Регистрируем КОНКРЕТНЫЕ провайдеры по типу.
    /// </summary>
    public class DataProviderExchangeAutofacModule<TIn> : Module
    {
        protected override void Load(ContainerBuilder builder)
        {          
            switch (typeof(TIn).Name)
            {
                case "AdInputType":
                    builder.RegisterType<AdStronglyTypedResponseFactory>().As<IStronglyTypedResponseFactory>().SingleInstance();

                    builder.RegisterType<VidorBinaryDataProvider>().Named<IDataProvider<AdInputType, ResponseInfo>>("VidorBinary").InstancePerDependency();
                    builder.RegisterType<ByRulesDataProvider<AdInputType>>().Named<IDataProvider<AdInputType, ResponseInfo>>("ByRules").InstancePerDependency();

                    builder.RegisterType<AdInputTypeIndependentInsertsService>().As<IIndependentInsertsService>().SingleInstance();
                    break;

                case "OtherType": 
                    //builder.RegisterType<OtherTypeStronglyTypedResponseFactory>().As<IStronglyTypedResponseFactory>().SingleInstance();
                    //builder.RegisterType<OtherDataProvider>().As<IExchangeDataProvider<TIn, TransportResponse>>().InstancePerDependency();
                    break;
            }
        }
    }
}