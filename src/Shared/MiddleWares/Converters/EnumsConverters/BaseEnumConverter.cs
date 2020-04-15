using System;

namespace Shared.MiddleWares.Converters.EnumsConverters
{
    public abstract class BaseEnumConverter : IConverterMiddleWare<Enum>
    {
        protected Type ObjectType { get; }
        protected BaseEnumConverter(string path2Type)
        {
            ObjectType = Type.GetType(path2Type);      //Извлекли тип объекта из сборки
        }

        public abstract Enum Convert(Enum inProp, int dataId);
    }
}