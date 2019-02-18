using System.Collections.Generic;
using System.Linq;

namespace Shared.CrcCalculate
{
    public static class CrcCalc
    {
        public static byte CalcXor(IReadOnlyList<byte> arr)
        {
            var xor = arr[0];
            for (var i = 1; i < arr.Count; i++)
            {
                xor ^= arr[i];
            }
            xor ^= 0xFF;

            return xor;
        }


        public static byte CalcMod256(IReadOnlyList<byte> arr)
        {
            var sum = arr.Aggregate(0, (current, a) => current + a);
            return (byte)(sum % 256);
        }
    }
}