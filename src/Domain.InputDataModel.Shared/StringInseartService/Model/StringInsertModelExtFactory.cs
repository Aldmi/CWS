using System.Collections.Generic;
using Shared.MiddleWares.ConvertersOption.StringConvertersOption;
using Shared.MiddleWares.HandlersOption;

namespace Domain.InputDataModel.Shared.StringInseartService.Model
{
    public class StringInsertModelExtFactory
    {
        #region fields
        private readonly IReadOnlyDictionary<string, StringInsertModelExt> _extDict;
        private readonly string _keyExt;
        private readonly StringInsertModelExt _defaultExt;
        private readonly StringInsertModelExt _keyNotFoundExt;
        #endregion


        #region ctor
        public StringInsertModelExtFactory(IReadOnlyDictionary<string, StringInsertModelExt> extDict, string keyExt)
        {
            _extDict = extDict;
            _keyExt = keyExt;
            _defaultExt = new StringInsertModelExt("default", string.Empty, null, null);
            _keyNotFoundExt = new StringInsertModelExt("keyNotFound", string.Empty, null, 
                new StringHandlerMiddleWareOption
                {
                    Converters = new List<UnitStringConverterOption>
                    {
                        new UnitStringConverterOption {ReplaseStringConverterOption = new ReplaseStringConverterOption
                        {
                            Mapping = new Dictionary<string, string>
                            {
                                {"_", "!!!ExtKeyNotFound!!!" }
                            }
                        }}
                    }
                });
        }
        #endregion


        /// <summary>
        /// Ищет расширение в словаре. Если расширение не найдено, вставляется дефолтное значение. 
        /// </summary>
        public StringInsertModelExt GetExt()
        {
            //1. Ключа не указан, вернуть расширение по умолчанию.
            if (string.IsNullOrEmpty(_keyExt))
            { 
                return _defaultExt;
            }
            //2. Ключ ЕСТЬ в словаре, вернуть указанное Ext. Ключ НЕТ в словаре, вернуть расширение keyNotFound (заменяющее вставку на !!!ExtKeyNotFound!!!)
            return _extDict.TryGetValue(_keyExt, out var ext) ? ext : _keyNotFoundExt;
        }
    }
}