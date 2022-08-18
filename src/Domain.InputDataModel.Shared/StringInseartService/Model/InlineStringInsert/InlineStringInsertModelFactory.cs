using System.Collections.Generic;

namespace Domain.InputDataModel.Shared.StringInseartService.Model.InlineStringInsert
{
    public class InlineStringInsertModelFactory
    {
        #region fields
        private readonly IReadOnlyDictionary<string, InlineStringInsertModel> _extDict;
        private readonly InlineStringInsertModel _defaultInlineModel;
        private readonly InlineStringInsertModel _keyNotFoundInlineModel;
        #endregion


        #region ctor
        public InlineStringInsertModelFactory(IReadOnlyDictionary<string, InlineStringInsertModel> extDict)
        {
            _extDict = extDict;
            _defaultInlineModel= new InlineStringInsertModel("default", " ", "Значение по умолчанию");
            _keyNotFoundInlineModel = new InlineStringInsertModel("keyNotFound", "!!!InlineStrKeyNotFound!!!", "Значение Если ключ в словаре не найденн");
        }
        #endregion


        public InlineStringInsertModel GetInlineModel(string key)
        {
            //1. Ключа не указан, вернуть расширение по умолчанию.
            if (string.IsNullOrEmpty(key))
            {
                return _defaultInlineModel;
            }
            //2. Ключ ЕСТЬ в словаре, вернуть указанное по клучу значение. Ключ НЕТ в словаре, вернуть расширение _keyNotFoundInlineModel (заменяющее вставку на !!!InlineStrKeyNotFound!!!)
            return _extDict.TryGetValue(key, out var ext) ? ext : _keyNotFoundInlineModel;
        }


        public string GetInlineStr(string key)
        {
            return GetInlineModel(key).InlineStr;
        }
    }
}