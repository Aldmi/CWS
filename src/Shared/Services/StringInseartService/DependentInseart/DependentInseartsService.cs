using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace Shared.Services.StringInseartService.DependentInseart
{
    public class DependentInseartsService
    {
        #region fields
        private readonly Func<string, string, Result<string>>[] _replacementHandlers;
        #endregion


        #region ctor
        public DependentInseartsService(params Func<string, string, Result<string>>[] replacementHandlers)
        {
            if(replacementHandlers == null || !replacementHandlers.Any())
                throw new ArgumentException("replacementHandlers не может быть пустым");

            _replacementHandlers = replacementHandlers;
        }
        #endregion


        #region Methode
        public Result<string> ExecuteInseart(string str, string format)
        {
            foreach (var handler in _replacementHandlers)
            {
               var res= handler(str, format);
               if (res.IsFailure)
                   return res;
               str = res.Value;
            }
            return Result.Ok(str);
        }
        #endregion
    }
}