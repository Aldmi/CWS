namespace Shared.Mathematic
{
    public class MathematicFormula
    {
        private const string VarName = "x";

        #region prop
        /// <summary>
        /// Математическое выражение
        /// </summary>
        public string Expr { get; private set; } //private set нужен для AutoMapper
        #endregion


        #region ctor
        public MathematicFormula(string expr)
        {
            Expr = expr;
        }
        #endregion



        public int Calc(int var)
        {
            return MathematicExpression.CalculateExpression(Expr, VarName, var);
        }
    }
}