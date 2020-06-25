using System;
using System.Collections.Generic;

namespace Shared.CrcCalculate.CrcClasses
{
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
}