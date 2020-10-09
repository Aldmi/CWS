using System;
using System.Collections.Generic;
using System.Linq;
using Domain.InputDataModel.Autodictor.MIddleWare.ObjectConverters;
using Shared.MiddleWares.Converters;

namespace Domain.Device.MiddleWares4InData.Handlers4InData
{
    public class ObjectHandlerMiddleWare4InData : BaseHandlerMiddleWare4InData<object>
    {
        #region ctor
        public ObjectHandlerMiddleWare4InData(ObjectHandlerMiddleWare4InDataOption option)
        {
            PropName = option.PropName;
            Converters.AddRange(option.CreateConverters());
        }
        #endregion
    }


    public class ObjectHandlerMiddleWare4InDataOption : ObjectHandlerMiddleWareOption
    {
        public string PropName { get; set; }                       //Имя свойства для обработки
    }


    public class ObjectHandlerMiddleWareOption
    {
        public List<UnitObjectConverterOption> Converters { get; set; }

        public IList<IConverterMiddleWare<object>> CreateConverters()
        {
            return Converters.Select(c => c.CreateConverter()).ToList();
        }

    }

    public class UnitObjectConverterOption
    {
        public CreepingLineRunningConvertertOption CreepingLineRunningConvertertOption { get; set; }

        public IConverterMiddleWare<object> CreateConverter()
        {
            if (CreepingLineRunningConvertertOption != null) return new CreepingLineRunningConvertert(CreepingLineRunningConvertertOption);

            throw new NotSupportedException("В UnitObjectConverterOption необходимо указать хотя бы одну опцию");
        }
    }
}