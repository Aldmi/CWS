using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using Domain.InputDataModel.Shared.StringInseartService.InlineInseart;

namespace WebApiSwc.AutofacModules
{
    public class ServicesAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Регистрация InlineInseartService для ByRules провайдера, со специальным replacePattern
            const string replacePattern = @"\{\$[^{}:$]+\}";
            builder.RegisterType<InlineInseartService>()
                .AsSelf()
                .Named<InlineInseartService>("ByRules")
                .WithParameters(new List<Parameter>
                {
                    new NamedParameter("replacePattern", replacePattern),
                })
                .InstancePerDependency();
        }
    }
}