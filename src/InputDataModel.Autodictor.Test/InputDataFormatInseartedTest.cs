using System.Collections.Generic;
using Xunit;

namespace InputDataModel.Autodictor.Test
{
    /// <summary>
    /// Тест подстановки данных по формату для ViewRule
    /// </summary>
    public class InputDataFormatInseartedTest
    {


        public static IEnumerable<object[]> GetData4Note => new[]
        {
            new object[]
            {
                "Со всеми остановками кроме: Узуново, Ожерелье", //note
                "{Note:[3^0x09]  [10^0x09]}", //noteFormat     
            },
        };









        /// <summary>
        /// Тест подст
        /// </summary>
        [Theory]
        [MemberData(nameof(GetData4Note))]
        public void GetDataRequestStringTest(string note, string noteFormat)
        {





        }
    }
}