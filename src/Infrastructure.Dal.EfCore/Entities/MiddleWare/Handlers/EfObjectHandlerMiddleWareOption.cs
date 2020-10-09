using System.Collections.Generic;
using Infrastructure.Dal.EfCore.Entities.MiddleWare.Converters.ObjectConvertersOption;


namespace Infrastructure.Dal.EfCore.Entities.MiddleWare.Handlers
{
    public class EfObjectHandlerMiddleWareOption
    {
        public List<EfUnitObjectConverterOption> Converters { get; set; }
    }

    public class EfUnitObjectConverterOption
    {
        public EfCreepingLineRunningConvertertOption CreepingLineRunningConvertertOption { get; set; }
    }
}