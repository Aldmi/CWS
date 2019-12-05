using Autofac;
using Domain.Device.Produser;
using Domain.Device.Repository.Entities.ResponseProduser;
using Domain.InputDataModel.Autodictor.Model;
using Infrastructure.Produser.AbstractProduser.AbstractProduser;
using Infrastructure.Produser.KafkaProduser;
using Infrastructure.Produser.KafkaProduser.Options;
using Infrastructure.Produser.WebClientProduser;
using Infrastructure.Produser.WebClientProduser.Options;
using WebApiSwc.Produsers;
using WebApiSwc.SignalRClients;

namespace WebApiSwc.AutofacModules
{
    /// <summary>
    /// Регистрируем Продюссеры для системы ответов.
    /// </summary>
    public class ProduserUnionAutofacModule<TIn> : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProdusersUnionResponseConverter<TIn>>().SingleInstance();
            builder.RegisterType<ProdusersUnionFactory<TIn>>().InstancePerDependency();
            builder.RegisterType<ProdusersUnion<TIn>>().InstancePerDependency();

            builder.RegisterType<KafkaProduserWrapper>().As<IProduser<KafkaProduserOption>>().InstancePerDependency();
            builder.RegisterType<WebClientProduserWrapper>().As<IProduser<WebClientProduserOption>>().InstancePerDependency();
            builder.RegisterType<SignaRProduserClientsStorage<SignaRProdusserClientsInfo>>().SingleInstance();
            builder.RegisterType<SignalRProduserWrapper>().As<IProduser<SignalRProduserOption>>().InstancePerDependency();
        }
    }
}