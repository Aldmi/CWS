using System;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

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
        #region prop
        public string Replacement { get; }
        public string VarName { get; }
        public StringInsertModelExt Ext { get; }
        #endregion


        #region ctor
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="replacement">весь блок замены в строке</param>
        /// <param name="varName">имя переменной выделенной из replacement</param>
        /// <param name="ext">расширение. НЕ МОЖЕТ быть NULL</param>
        public StringInsertModel(string replacement, string varName, StringInsertModelExt ext)
        {
            Replacement = replacement;
            VarName = varName;
            Ext = ext ?? throw new ArgumentNullException(nameof(ext));
        }
        #endregion


        #region StaticMethode

        public static Result<string> CalcSubStringBeetween2Models(string str, StringInsertModel startModel, StringInsertModel endModel)
        {
            var pattern = $"{startModel.Replacement}(.*){endModel.Replacement}";
            var match = Regex.Match(str, pattern);
            if (match.Success)
            {
                var res = match.Groups[1].Value;
                return Result.Ok(res);
            }
            return Result.Failure<string>($"Невозможно выделить подстроку из строки {str} используя паттерн {pattern}");
        }

        #endregion
    }
}