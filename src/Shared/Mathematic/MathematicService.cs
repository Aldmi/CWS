using CSharpFunctionalExtensions;

namespace Shared.Mathematic
{
    public class MathematicService
    {
        private const string VarName = "x";

        #region prop
        /// <summary>
        /// Математическое выражение
        /// </summary>
        public string Expr { get; private set; } //private set нужен для AutoMapper
        #endregion


        #region ctor
        public MathematicService(string expr)
        {
            Expr = expr;
        }
        #endregion



        public int Calc(int var)
        {
            return MathematicFormat.CalculateMathematicFormat(Expr, VarName, var);
        }
    }
}