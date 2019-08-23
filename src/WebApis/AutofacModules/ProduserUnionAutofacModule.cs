using AbstractProduser.AbstractProduser;
using Autofac;
using BL.Services.Produser;
using InputDataModel.Autodictor.Model;
using KafkaProduser;
using KafkaProduser.Options;
using WebClientProduser;
using WebClientProduser.Options;

namespace WebApiSwc.AutofacModules
{
    /// <summary>
    /// Регистрируем Продюссеры для системы ответов.
    /// </summary>
    public class ProduserUnionAutofacModule<TIn> : Module
    {
        protected override void Load(ContainerBuilder builder)
        {          
            switch (typeof(TIn).Name)
            {
                case "AdInputType":
                    builder.RegisterType<ProdusersUnionFactory<AdInputType>>().InstancePerDependency();
                    builder.RegisterType<ProdusersUnion<AdInputType>>().InstancePerDependency();

                    builder.RegisterType<KafkaProduserWrapper>().As<IProduser<KafkaProduserOption>>().InstancePerDependency();
                    builder.RegisterType<WebClientProduserWrapper>().As<IProduser<WebClientProduserOption>>().InstancePerDependency();
                    break;

                case "OtherType":
                    break;
            }
        }
    }
}