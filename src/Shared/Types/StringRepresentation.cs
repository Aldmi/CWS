namespace Shared.Types
{
    /// <summary>
    /// Строка в формате представления
    /// </summary>
    public class StringRepresentation
    {
        public string Str { get; }
        public string Format { get; }

        public StringRepresentation(string str, string format)
        {
            Str = str;
            Format = format;
        }
    }
}