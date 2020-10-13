using System;

namespace Domain.InputDataModel.Shared.StringInseartService.Model.InlineStringInsert
{
    public class InlineStringInsertModel : IDisposable
    {
        public string Key { get; private set; }    //private set нужен для AutoMapper
        public string InlineStr { get; }
        public string Description { get; }


        public InlineStringInsertModel(string key, string inlineStr, string description)
        {
            Key = string.IsNullOrEmpty(key) ? throw new ArgumentNullException(nameof(key)) : key;
            InlineStr = string.IsNullOrEmpty(inlineStr) ? throw new ArgumentNullException(nameof(inlineStr)) : inlineStr;
            Description = description;
        }


        #region Disposable
        public void Dispose() { }
        #endregion
    }
}