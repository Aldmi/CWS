using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Shared.Extensions;

namespace Shared.Types
{
    public class BorderSubString
    {
        public string StartCh { get; set; }
        public string EndCh { get; set; }
        public bool IncludeBorder { get; set; }



        /// <summary>
        /// Вернуть подстроку обозначенную границами BorderSubString
        /// </summary>
        public Result<string> Calc(string str)
        {
            var (_, isFailure, value, error) = str.SubstringBetweenCharacters(StartCh, EndCh, IncludeBorder);
            return isFailure ? Result.Failure<string>(error) : Result.Ok(value);
        }
    }
}