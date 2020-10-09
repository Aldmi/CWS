using System;
using System.Collections.Generic;
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
                return Result.Failure<(PropertyInfo, object, T)>("propName Не задан");

            //Поиск свойства (выставление состояния сервиса)------------------------------------
            PropertyInfo propInfo = null;           //свойство для изменения (например NameRu)
            var obj = inData;                       //Значение свойства для изменения (например "Имя1")
            var objParent = obj;                    //Значение Родительского свойства для изменения (например Note)
            for (var i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                if (obj == null)
                {
                    return Result.Failure<(PropertyInfo, object, T)>($"Родительский объект == Null. {propName}. Невозможно обратится к свойству {prop}");
                }
                propInfo = GetPropertyInfo(obj, prop);
                if (propInfo == null)
                {
                    return Result.Failure<(PropertyInfo, object, T)>($"метаданные для {prop} не найдены");
                }

                obj = propInfo.GetValue(obj);
                if (i < props.Length - 1)
                {
                    objParent = obj;
                }
            }

            //Валидация--------------------------------------------------------------------------------
            var strictTypeEquality = propInfo.PropertyType == typeof(T);   //Строгое равенство типов
            var hierarchyEquality = obj is T;                              //Если типы из одной иерархии и возможно приведение к тип T;
            var notMatchType = !(strictTypeEquality || hierarchyEquality); //Если 2 условиея не выполнились, то переменная НЕ верного типа.
            if (notMatchType)
            {
                return Result.Failure<(PropertyInfo, object, T)>($"Тип свойства {propName} не соответвует типу обработчика handler {typeof(T)}");
            }

            //Возврат значениtе в кортеже----------------------------------------------------------------
            var value = (T)obj;
            var tuple = (propInfo: propInfo, objParent:objParent, val: value);
            return Result.Ok(tuple);
        }


        /// <summary>
        /// Возвращает ЗНАЧЕНИЕ переданного свойства, под типом dynamic.
        /// Вызываемый код должен сам выполнить cast к нужному типу.
        /// </summary>
        public Result<dynamic> GetValue(object inData, string propName)
        {
            if (inData == null)
                throw new ArgumentNullException(nameof(inData));

            var props = propName.Split('.');
            if (props.Length == 0)
                return Result.Failure<dynamic>("propName Не задан");

            //Поиск свойства (выставление состояния сервиса)------------------------------------
            var obj = inData;                       //Значение свойства для изменения (например "Имя1")
            foreach (var prop in props)
            {
                if (obj == null)
                {
                    return Result.Failure<object>($"Родительский объект == Null. {propName}. Невозможно обратится к свойству {prop}");
                }
                var propInfo = GetPropertyInfo(obj, prop);                 //свойство для изменения (например NameRu)
                if (propInfo == null)
                {
                    return Result.Failure<object>($"метаданные для {prop} не найдены");
                }
                obj = propInfo.GetValue(obj);
            }
            return Result.Ok(obj);
        }


        public Result<Dictionary<string, dynamic>> GetValuesDict(object inData, params string[] propNames)
        {
            Dictionary<string, dynamic> resDict= new Dictionary<string, dynamic>();
            foreach (var propName in propNames)
            {
              var (_, isFailure, value, error) = GetValue(inData, propName);
              if(isFailure)
                  return Result.Failure<Dictionary<string, dynamic>>(error);

              resDict.Add(propName, value);
            }
            return Result.Ok(resDict);
        }



        public Result SetPropValue((PropertyInfo propInfo, object objParent, T val) tupleNewValue)
        {
            var (propInfo, objParent, val) = tupleNewValue;

            if (propInfo == null || objParent == null)
                return Result.Failure("Свойство объекта не переданно в КОРТЕЖЕ");

            try
            {
                propInfo.SetValue(objParent, val);
                return Result.Ok();
            }
            catch(Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }


        private static PropertyInfo GetPropertyInfo(object inData, string name)
        {
            var type = inData.GetType();
            var propInfo = type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return propInfo;
        }

        #endregion

    }
}