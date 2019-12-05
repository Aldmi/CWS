
namespace Domain.InputDataModel.Base.InseartServices.IndependentInsearts
{
    public class IndependentInsertModel
    {
        public string Replacement { get;  }
        public string VarName { get; }
        public string Format { get; }

        public IndependentInsertModel(string replacement, string varName, string format)
        {
            Replacement = replacement;
            VarName = varName;
            Format = format;
        }
    }
}