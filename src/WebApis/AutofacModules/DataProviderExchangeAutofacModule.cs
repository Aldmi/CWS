using AbstractProduser.AbstractProduser;
using Autofac;
using Domain.Device.Produser;
using Domain.Exchange.DataProviderAbstract;
using Domain.InputDataModel.Autodictor.DataProviders.ByRuleDataProviders;
using Domain.InputDataModel.Autodictor.DataProviders.ManualDataProviders;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Autodictor.StronglyTypedResponse;
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

                    builder.RegisterType<VidorBinaryDataProvider>().Named<IExchangeDataProvider<AdInputType, ResponseInfo>>("VidorBinary").InstancePerDependency();
                    builder.RegisterType<ByRulesDataProvider<AdInputType>>().Named<IExchangeDataProvider<AdInputType, ResponseInfo>>("ByRules").InstancePerDependency();
                    break;

                case "OtherType": 
                    //builder.RegisterType<OtherTypeStronglyTypedResponseFactory>().As<IStronglyTypedResponseFactory>().SingleInstance();
                    //builder.RegisterType<OtherDataProvider>().As<IExchangeDataProvider<TIn, TransportResponse>>().InstancePerDependency();
                    break;
            }


            //TODO: вынести в отдельный модуль
            builder.RegisterType<ProdusersUnionFactory<AdInputType>>().InstancePerDependency();
            builder.RegisterType<ProdusersUnion<AdInputType>>().InstancePerDependency();

            builder.RegisterType<KafkaProduserWrapper>().As<IProduser<KafkaProduserOption>>().InstancePerDependency();
            builder.RegisterType<WebClientProduserWrapper>().As<IProduser<WebClientProduserOption>>().InstancePerDependency();
        }
    }
}