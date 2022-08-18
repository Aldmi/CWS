using KellermanSoftware.CompareNetObjects;
using Shared.Extensions;
using Shared.Helpers;

namespace Shared.Types
{
    /// <summary>
    /// Строка в формате представления
    /// </summary>
    public class StringRepresentation
    {
        public string Str { get; }
        public string Format { get; }
        public bool IsNull => Str.Equals("NULL") && Format.Equals("_");

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

        public static StringRepresentation CreateNull()
        {
            var strRes = "NULL";
            var format = "_";
            return new StringRepresentation(strRes, format);
        }


        public byte[] Convert2ByteArray()
        {
            return Str.ConvertString2ByteArray(Format); //Преобразовываем КОНЕЧНУЮ строку в массив байт
        }
        #endregion


        #region EqualsOperator
        protected bool Equals(StringRepresentation other)
        {
            var compareLogic = new CompareLogic();
            var result = compareLogic.Compare(other, this);
            return result.AreEqual;
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
        #endregion


        public override string ToString()
        {
            var viewInfo = IsNull ?
                $"[{Str}]:{Format}" :
                $"[{Str}]:{Format} L='{Str.Length}'";
            return viewInfo;
        }
    }
}