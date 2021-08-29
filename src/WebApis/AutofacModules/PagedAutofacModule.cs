using Autofac;
using Domain.InputDataModel.Base.InData;
using Shared.Paged;

namespace WebApiSwc.AutofacModules
{
    public class PagedAutofacModule<TIn>: Module where TIn : InputTypeBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PagedService<TIn>>().InstancePerDependency();
        }
    }
}