using Autofac;
using Domain.Device.Paged4InData;
using Domain.InputDataModel.Base.InData;
using Shared.Paged;

namespace WebApiSwc.AutofacModules
{
    public class PagedAutofacModule<TIn>: Module where TIn : InputTypeBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PagingInvokeService<TIn>>().InstancePerDependency();
        }
    }
}