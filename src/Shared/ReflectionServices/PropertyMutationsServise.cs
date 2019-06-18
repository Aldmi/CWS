using System;
using System.Reflection;
using CSharpFunctionalExtensions;

namespace Shared.ReflectionServices
{
    public class PropertyMutationsServise<T>
    {
        #region Methods

        public Result<(PropertyInfo propInfo, object objParent, T val)> GetPropValue(object inData, string propName)
        {
            if (inData == null)
                throw new ArgumentNullException(nameof(inData));

            var props = propName.Split('.');
            if (props.Length == 0)
                return Result.Fail<(PropertyInfo, object, T)>("propName Не задан");

            //Поиск свойства (выставление состояния сервиса)------------------------------------
            PropertyInfo propInfo = null;           //свойство для изменения (например NameRu)
            var obj = inData;                       //Значение свойства для изменения (например "Имя1")
            var objParent = obj;                    //Значение Родительского свойства для изменения (например Note)
            for (var i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                if (obj == null)
                {
                    return Result.Fail<(PropertyInfo, object, T)>($"Родительский объект == Null. {propName}. Невозможно обратится к свойству {prop}");
                }
                propInfo = GetPropertyInfo(obj, prop);
                if (propInfo == null)
                {
                    return Result.Fail<(PropertyInfo, object, T)>($"метаданные для {prop} не найдены");
                }

                obj = propInfo.GetValue(obj);
                if (i < props.Length - 1)
                {
                    objParent = obj;
                }
            }

            //Валидация--------------------------------------------------------------------------------
            if (propInfo.PropertyType != typeof(T))
            {
                return Result.Fail<(PropertyInfo, object, T)>($"Тип свойства {propName} не соответвует типу обработчика handler {typeof(T)}");
            }
            //if ((obj != null) && !(obj is T))
            //{
            //    return Result.Fail<(PropertyInfo, object, T)>($"Тип свойства {propName} не соответвует типу обработчика handler {typeof(T)}");
            //}

            //Возврат значениtе в кортеже----------------------------------------------------------------
            var value = (T)obj;
            var tuple = (propInfo: propInfo, objParent:objParent, val: value);
            return Result.Ok(tuple);
        }




        public Result SetPropValue((PropertyInfo propInfo, object objParent, T val) tupleNewValue)
        {
            var (propInfo, objParent, val) = tupleNewValue;

            if (propInfo == null || objParent == null)
                return Result.Fail("Свойство объекта не переданно в КОРТЕЖЕ");

            try
            {
                propInfo.SetValue(objParent, val);
                return Result.Ok();
            }
            catch(Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }


        private static PropertyInfo GetPropertyInfo(object inData, string name)
        {
            var type = inData.GetType();
            var propInfo = type.GetProperty(name);
            return propInfo;
        }

        #endregion

    }
}