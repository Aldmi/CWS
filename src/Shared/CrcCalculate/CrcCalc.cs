using System;
using System.Collections.Generic;
using System.Linq;
using Crc;

namespace Shared.CrcCalculate
{
    /// <summary>
    /// Алгоритмы вычисление CRC. 
    /// </summary>
    public static class CrcCalc
    {
        public static byte[] CalcXor(IReadOnlyList<byte> arr)
        {
            var xor = arr[0];
            for (var i = 1; i < arr.Count; i++)
            {
                xor ^= arr[i];
            }
            return new[] { xor };
        }


        public static byte[] CalcXorInverse(IReadOnlyList<byte> arr)
        {
            var xor = arr[0];
            for (var i = 1; i < arr.Count; i++)
            {
                xor ^= arr[i];
            }
            xor ^= 0xFF;
            return new[] { xor };
        }


        public static byte[] CalcMod256(IReadOnlyList<byte> arr)
        {
            var sum = arr.Aggregate(0, (current, a) => current + a);
            var mod256 = (byte)(sum % 256);
            return new[] { mod256 };
        }


        /// <summary>
        /// Специфический алгоритм вычисления CRC для AlphaTime протокола.
        /// </summary>
        public static byte[] CalcMod256AlphaTime(IReadOnlyList<byte> arr)
        {
            var sum = arr.Aggregate(0, (current, a) => current + a);
            var mod256 = (sum % 256);
            if (mod256 < 128)
                mod256 += 128;

            return new[] { (byte)mod256 };
        }


        /// <summary>
        /// Посчитать сумму и взять младший байт.
        /// </summary>
        public static byte[] Calc8Bit(IReadOnlyList<byte> arr)
        {
            byte sum = 0;
            foreach (var b in arr)
            {
                sum += b;
            }
            return new[] { sum };
        }

        /// <summary>
        /// Посчитать СRC по алгоритму Crc8Maxim
        /// </summary>
        public static byte[] CalcCrc8Maxim(IReadOnlyList<byte> arr)
        {
            var crc8Maxim = new Crc8Maxim();
            var crc = crc8Maxim.ComputeHash(arr.ToArray());
            return new[] { crc[0]};
        }



        public class Crc16Ccitt
        {
            private readonly ushort _init;
            private readonly int _poly;

            public Crc16Ccitt(ushort init, int poly)
            {
                _init = init;
                _poly = poly;
            }


            public byte[] Calc(IReadOnlyList<byte> arr)
            {
                ushort crc = _init;
                for (int i = 0; i < arr.Count; i++)
                {
                    crc ^= (ushort)(arr[i] << 8);
                    for (int j = 0; j < 8; j++)
                    {
                        if ((crc & 0x8000) > 0)
                            crc = (ushort)((crc << 1) ^ _poly);
                        else
                            crc <<= 1;
                    }
                }
                var res = BitConverter.GetBytes(crc);
                return res;
            }
        }


        class Crc8Maxim : Crc8Base
        {
            public Crc8Maxim() : base(0x31, 0x00, 0x00, true, true, 0xA1)
            {
            }
        }
    }
}