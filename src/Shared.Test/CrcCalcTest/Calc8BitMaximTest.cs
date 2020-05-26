﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Shared.CrcCalculate;
using Xunit;

namespace Shared.Test.CrcCalcTest
{
    public class Calc8BitMaximTest
    {

        public static IEnumerable<object[]> Datas => new[]
        {
            new object[]
            {
               new byte[] { 0x01, 0x04, 0x4A, 0x10 },
               0xF0
            },
            new object[]
            {
                new byte[] {
                    0x01, 0x81, 0x10, 0x02, 0x01, 0x01, 0x00, 0x02, 0x80, 0x00, 0x73, 0x04, 0x04, 0x7c, 0x04,
                    0x04, 0x7c, 0x54, 0x54, 0x54, 0x44, 0x38, 0x44, 0x44, 0x44, 0x44, 0x04, 0x04, 0x7c, 0x04, 0x04,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03,
                    0x4f, 0xa6},
                0x3A
            }


        };
        [Theory]
        [MemberData(nameof(Datas))]
        public void Check(byte[] arr, byte expectedCrc)
        {
            var crc= CrcCalc.Calc8BitMaxim(arr).First();
            crc.Should().Be(expectedCrc);
        }
    }
}