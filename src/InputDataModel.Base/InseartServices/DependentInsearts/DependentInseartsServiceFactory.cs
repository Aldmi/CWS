using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Shared.Services.StringInseartService.DependentInseart;

namespace Domain.InputDataModel.Base.InseartServices.DependentInsearts
{
    public static class DependentInseartsServiceFactory
    {
        /// <summary>
        /// Порядок выполнения обработчиков СТРОГО заданн и не определяется порядком нахождения replacement в строке.
        /// Например Nbyte и NbyteFull должно вычислятся строго после NumberOfCharacters
        /// </summary>
        public static DependentInseartsService Create(string str)
        {
            var replacementHandlers = new List<Func<string, string, Result<string>>>();
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
    }
}