using System;
using System.Collections.Generic;

namespace Shared.CrcCalculate.CrcClasses
{
    /// <summary>
    /// CRC по алгоритму Артема
    /// </summary>
    public class Crc8FullPoly
    {
        /**
         * Набор данных, сформированный опытным путём
         */
        private static readonly byte[][] Source = {
            new byte[] { 0x01, 0xfd, 0x05, 0xf5, 0xf5, 0x35, 0xb5, 0xb5, 0x5a },
            new byte[] { 0x80, 0x7f, 0x83, 0x7b, 0x7b, 0x9b, 0x5b, 0xdb, 0x2d },
            new byte[] { 0x40, 0x40, 0x41, 0x3d, 0x3d, 0x4d, 0x2d, 0x6d, 0x96 },
            new byte[] { 0x20, 0xa0, 0xa0, 0x9f, 0x9f, 0xa7, 0x97, 0xb7, 0x4b },
            new byte[] { 0x10, 0xd0, 0x50, 0x50, 0x4f, 0x53, 0x4b, 0x5b, 0xa5 },
            new byte[] { 0x08, 0xe8, 0x28, 0xa8, 0xa8, 0xa9, 0xa5, 0xad, 0xd2 },
            new byte[] { 0x04, 0xf4, 0x14, 0xd4, 0xd4, 0xd4, 0xd3, 0xd7, 0x69 },
            new byte[] { 0x02, 0xfa, 0x0a, 0xea, 0xea, 0x6a, 0x6a, 0x6b, 0xb4 }
        };

        /**
         * Матрица смещения значения функции относительно предыдущего значения. Нужна для получения матрицы значений функции.
         * Строки - N % n, где N - порядковый номер байта в пакете, n - количество строк.
         * Колонки - (f(x) - f(x-1)) % 256, где x - индекс колонки (0x00 - 0xFF)
         */
        private static readonly byte[][] Offsets = InitializeOffsets(Source);

        /**
         * Матрица значений функции для байта. 
         * Строки - N % n, где N - порядковый номер байта в пакете, n - количество строк.
         * Колонки - f(x) % 256, где x - индекс колонки (0x00 - 0xFF)
         */
        private static readonly byte[][] Values = InitializeValues(Offsets);

        /**
         * Проинициализировать массив заданными в массиве-источнике значениям по определённым позициям.
         */
        private static byte[][] InitializeOffsets(byte[][] source)
        {
            // Создаем массив, размером с источник
            var result = new byte[source.Length][];
            for (var i = 0; i < result.Length; i++)
            {
                // Создаем подмассив для каждой строки
                var arr = new byte[256];

                // Перебираем байты от 0x00 до 0xFF
                for (var j = 0; j < arr.Length; j++)
                {
                    for (var k = 0; k < source[i].Length; k++)
                    {
                        // Находим степень двойки, которой не кратно это число.
                        // Если условие не соблюдено, записываем элемент из последнего столбца
                        if (j % (2 << k) != 0 || k == source[i].Length - 1)
                        {
                            arr[j] = source[i][k];
                            break;
                        }
                    }
                }
                result[i] = arr;
            }

            return result;
        }

        private static byte[][] InitializeValues(byte[][] offsets)
        {
            // Создаем массив, размером с источник
            var result = new byte[offsets.Length][];
            for (var i = 0; i < result.Length; i++)
            {
                // Создаем подмассив для каждой строки
                var arr = new byte[offsets[i].Length];

                // Суммируем f(x-1) и смещение в заданной точке, т.е. находим f(x), где x - индекс массива
                var res = 0;
                for (var j = 0; j < arr.Length; j++)
                {
                    res = (res + offsets[i][j]) % 256;
                    arr[j] = (byte) res;
                }
                result[i] = arr;
            }

            return result;
        }

        /**
         * Функция получения контрольной суммы
         */
        public static byte[] Calc(IReadOnlyList<byte> arr)
        {
            var sum = 0;
            for (var i = 0; i < arr.Count; i++)
            {
                // Определяем строку, в которой будем искать значение байта
                var pos = i % Values.Length;

                // Определяем значение байта для поиска (x), гарантированно не превышающее размер массива
                var num = arr[i] % Values[pos].Length;

                // Прибавляем к итоговой сумме полученное из матрицы значений число
                sum = (sum + Values[pos][num]) % 256;
            }
            return new[]{(byte) sum};
        }
    }
}
