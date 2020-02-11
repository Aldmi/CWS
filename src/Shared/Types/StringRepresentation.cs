using System;
using Shared.Extensions;

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


        #region Methode
        public static StringRepresentation Create(byte[] arr, string dataFormat)
        {
           var strRes= arr.ArrayByteToString(dataFormat);
           return new StringRepresentation(strRes, dataFormat);
        }
        #endregion


        #region EqualsOperator
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StringRepresentation)obj);
        }

        protected bool Equals(StringRepresentation other)
        {
            return Str == other.Str && Format == other.Format;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Str != null ? Str.GetHashCode() : 0) * 397) ^ (Format != null ? Format.GetHashCode() : 0);
            }
        }

        // Перегружаем логический оператор ==
        public static bool operator ==(StringRepresentation obj1, StringRepresentation obj2)
        {
            return obj1.Equals(obj2);
        }
        public static bool operator !=(StringRepresentation obj1, StringRepresentation obj2)
        {
            return !(obj1 == obj2);
        }

        public override string ToString()
        {
            return $"[{Str}]:{Format}";
        }
        #endregion
    }
}