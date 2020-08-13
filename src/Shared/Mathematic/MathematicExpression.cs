using NCalc;

namespace Shared.Mathematic
{
    public static class MathematicExpression
    {
        #region fields
        private static readonly object LockerNCalc = new object();
        #endregion



        /// <summary>
        /// Математическое вычисление формулы с участием переменной rowNumber
        /// Возможно CuncurrencyException при многопоточной работе с Expression.
        /// </summary>
        /// <param name="expression">математическое выражение где содержится переменная 'varName', вместо нее подставится значение var</param>
        /// <param name="varName">имя переменной</param>
        /// <param name="var">значение переменной</param>
        public static int CalculateExpression(string expression, string varName, int var)
        {
            lock (LockerNCalc)
            {
                var expr = new Expression(expression)
                {
                    Parameters = { [varName] = var }
                };
                var func = expr.ToLambda<int>();
                var arithmeticResult = func();
                return arithmeticResult;
            }
        }
    }
}