using System;
using System.Linq;
using Shared.Helpers;

namespace Domain.InputDataModel.Base.Response.ResponseInfos
{
    /// <summary>
    /// Интерпретатор ответа.
    /// IsOutDataValid выставляется при точном соответвие протоколу Ekrim
    /// </summary>
    public class ManualEkrimResponseInfo : BaseResponseInfo
    {
        public readonly int Address;
        public readonly byte[] Data;
        public readonly byte Crc;
        private ManualEkrimResponseInfo(int address, byte[] data, byte crc)
        {
            Address = address;
            Data = data;
            Crc = crc;
            IsOutDataValid = true;
        }
        private ManualEkrimResponseInfo()
        {
            IsOutDataValid = false;
        }

        public static ManualEkrimResponseInfo CreateInstanse(byte[] arr)
        {
            if (arr == null)
                return new ManualEkrimResponseInfo();                    //НЕ Валидные данные 

            try
            {
                int address = arr[0];
                var data = arr.Skip(3).Take(5).ToArray();
                var crc = arr.Last();
                return new ManualEkrimResponseInfo(address, data, crc);  //Валидные данные 
            }
            catch (Exception)
            {
                return new ManualEkrimResponseInfo();                    //НЕ Валидные данные 
            }
        }
        public override string ToString()
        {
            var json = HelpersJson.Serialize2RawJson(this);
            return $"{IsOutDataValid}  ManualEkrimResponseInfo= {json}";
        }
    }
}