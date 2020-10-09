using Domain.InputDataModel.Autodictor.Entities;
using Shared.MiddleWares.Converters;

namespace Domain.InputDataModel.Autodictor.MIddleWare.ObjectConverters
{
    public class CreepingLineRunningConvertert : IConverterMiddleWare<object>
    {
        private readonly CreepingLineRunningConvertertOption _option;


        public CreepingLineRunningConvertert(CreepingLineRunningConvertertOption option)
        {
            _option = option;
        }


        public object Convert(object inProp, int dataId)
        {
            var cl = (CreepingLine) inProp;
            cl.NameRu = "New11111";
            return cl;
        }
    }


    public class CreepingLineRunningConvertertOption
    {

    }
}