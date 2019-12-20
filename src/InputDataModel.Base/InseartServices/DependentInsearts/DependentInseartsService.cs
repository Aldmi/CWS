using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts;
using Shared.Helpers;
using Shared.Services.StringInseartService;

namespace Domain.InputDataModel.Base.InseartServices.DependentInsearts
{
    public class DependentInseartsService
    {
        #region fields
        private readonly Func<string, string, Result<string>>[] _replacementHandlers;
        #endregion


        #region ctor
        private DependentInseartsService(params Func<string, string, Result<string>>[] replacementHandlers)
        {
            if(replacementHandlers == null || !replacementHandlers.Any())
                throw new ArgumentException("replacementHandlers не может быть пустым");

            _replacementHandlers = replacementHandlers;
        }
        #endregion


        #region Methode
        public static DependentInseartsService DependentInseartsServiceFactory(string str)
        {
            var replacementHandlers = new List<Func<string, string, Result<string>>>();

            StringInsertModel inseart = null; //TODO:
            if (inseart.VarName == "NumberOfCharacters")
            {
                replacementHandlers.Add(DependentInseartHandlers.NumberOfCharactersInseartHandler);
            }


            if (Regex.Match(str, "{NumberOfCharacters:(.*)}").Success)
                replacementHandlers.Add(DependentInseartHandlers.NumberOfCharactersInseartHandler);

            if (Regex.Match(str, "{NbyteFull:(.*)}").Success)
                replacementHandlers.Add(DependentInseartHandlers.NByteFullInseartHandler);

            if (Regex.Match(str, "{Nbyte:(.*)}").Success)  //TODO: изменить выраждение на строгое равнство Nbyte
                replacementHandlers.Add(DependentInseartHandlers.NByteInseartHandler);

            if (Regex.Match(str, "{CRC(.*)}").Success)
                replacementHandlers.Add(DependentInseartHandlers.CrcInseartHandler);

            return replacementHandlers.Count == 0 ? null : new DependentInseartsService(replacementHandlers.ToArray());
        }


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