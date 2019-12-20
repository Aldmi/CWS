namespace Shared.Services.StringInseartService
{
    public class StringInsertModel
    {
        public string Replacement { get;  }
        public string VarName { get; }
        public string Format { get; }

        public StringInsertModel(string replacement, string varName, string format)
        {
            Replacement = replacement;
            VarName = varName;
            Format = format;
        }
    }
}