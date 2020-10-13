using App.Services.Actions;
using Autofac;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Shared.StringInseartService.InlineInseart;

namespace WebApiSwc.AutofacModules
{
    public class ServicesAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InlineInseartService>().AsSelf().InstancePerDependency();
        }
    }
}