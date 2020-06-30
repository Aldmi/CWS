using CSharpFunctionalExtensions;
using Shared.Extensions;

namespace Shared.Types
{
    /// <summary>
    /// Разделитель строки на 2 части.
    /// Выделять подстроку СЛЕВА/СПРАВА от указанного разделителя
    /// </summary>
    public enum DelemiterBorderSubString { None, Left, Right}

    public class BorderSubString
    {
        public string StartCh { get; set; }
        public string EndCh { get; set; }
        public bool StartInclude { get; set; }
        public bool EndInclude { get; set; }


        /// <summary>
        /// Вернуть подстроку обозначенную границами BorderSubString
        /// </summary>
        public Result<string> Calc(string str)
        {
            Result<string> result;
            if (string.IsNullOrEmpty(StartCh))
            {
                result = str.SubstringBetweenCharacters(0, EndCh, EndInclude);
            }
            else
            if (string.IsNullOrEmpty(EndCh))
            {
                result = str.SubstringBetweenCharacters(StartCh, str.Length - 1, StartInclude);
            }
            else
            {
                result = str.SubstringBetweenCharacters(StartCh, EndCh, StartInclude, EndInclude);
            }
            var (_, isFailure, value, error) = result;
            return isFailure ? Result.Failure<string>(error) : Result.Ok(value);
        }
    }
}