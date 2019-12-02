using NCalc;

namespace Shared.Mathematic
{
    public static class MathematicFormat
    {
        #region fields
        private static readonly object LockerNCalc = new object();
        #endregion



        /// <summary>
        /// Математическое вычисление формулы с участием переменной rowNumber
        /// Возможно CuncurrencyException при многопоточной работе с Expression.
        /// </summary>
        public static int CalculateMathematicFormat(string str, int row)
        {
            lock (LockerNCalc)
            {
                var expr = new Expression(str)
                {
                    Parameters = { ["rowNumber"] = row }
                };
                var func = expr.ToLambda<int>();
                var arithmeticResult = func();
                return arithmeticResult;
            }
        }
    }
}