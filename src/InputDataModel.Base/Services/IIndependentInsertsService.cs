using System.Collections.Generic;

namespace Domain.InputDataModel.Base.Services
{
    /// <summary>
    /// Интерфейс задает правила создания словаря ВСТАВОК независимых данных (данных самого типа). 
    /// </summary>
    public interface IIndependentInsertsService
    {
        /// <summary>
        /// Создать словарь вставок
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Dictionary<string, object> CreateDictionary(object inData);
    }
}