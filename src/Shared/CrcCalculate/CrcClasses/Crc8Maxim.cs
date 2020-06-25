using Crc;

namespace Shared.CrcCalculate.CrcClasses
{
    class Crc8Maxim : Crc8Base
    {
        public Crc8Maxim() : base(0x31, 0x00, 0x00, true, true, 0xA1)
        {
        }
    }
}