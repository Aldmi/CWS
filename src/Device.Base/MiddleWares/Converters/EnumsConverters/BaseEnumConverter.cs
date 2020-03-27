using System;
using Domain.Device.Repository.Entities.MiddleWareOption.ConvertersOption;

namespace Domain.Device.MiddleWares.Converters.EnumsConverters
{
    public abstract class BaseEnumConverter : IConverterMiddleWare<Enum>
    {
        public int Priority { get; }
        protected Type objectType { get; }
        protected BaseEnumConverter(BaseConverterOption baseOption)
        {
            Priority = baseOption.Priority;

            //Извлекли тип объекта из сборки
            const string objectToInstantiate = "Domain.InputDataModel.Autodictor.Entities.Lang, Domain.InputDataModel.Autodictor";
            objectType = Type.GetType(objectToInstantiate);
        }

        public abstract Enum Convert(Enum inProp, int dataId);
    }
}