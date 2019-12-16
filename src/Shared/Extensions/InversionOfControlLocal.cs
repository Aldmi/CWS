using System.Linq;

namespace Shared.Extensions
{
    /// <summary>
    /// Инверсия локального контролля по Дмитрий Нестерук
    /// </summary>
    public static class InversionOfControlLocal
    {
        /// <summary>
        /// Содержится ли данный элемент в коллекции
        /// </summary>
        /// <param name="self">элемент</param>
        /// <param name="variants">коллекция</param>
        public static bool IsOneOf<T>(this T self, params T[] variants)
        {
            return variants.Contains(self);
        }
    }
}