using System;


namespace Domain.InputDataModel.Shared.StringInseartService.Model
{
    /// <summary>
    /// Модель вставки значения в строку.
    /// Replacement - весь блок замены в строке
    /// VarName - Имя переменной значение которой будет вставленно вместо Replacement.
    /// Ext - Расширение, которое принимает занчение на вход и преобразует его к строке. Значение на вход переменной находит внешний сервис.
    /// </summary>
    public class StringInsertModel
    {
        private readonly StringInsertModelExtFactory _extFactory;


        #region prop
        public string Replacement { get; }
        public string VarName { get; }
        public string Option { get; }
        public StringInsertModelExt Ext => _extFactory.GetExt();
        #endregion


        #region ctor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="replacement">весь блок замены в строке</param>
        /// <param name="varName">имя переменной выделенной из replacement</param>
        /// <param name="option"></param>
        /// <param name="extFactory"></param>
        public StringInsertModel(string replacement, string varName, string option, StringInsertModelExtFactory extFactory)
        {
            Replacement = replacement;
            VarName = varName;
            _extFactory = extFactory ?? throw new ArgumentNullException(nameof(extFactory));
            Option = option;
        }
        #endregion
    }
}