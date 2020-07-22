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
        #region prop
        public int Address { get; }
        public byte[] Data { get; }
        public byte Crc { get; }
        #endregion


        #region ctor
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
        #endregion


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
            var data = GetData();
            var json = HelpersJson.Serialize2RawJson(data);
            return $"{base.ToString()}  Info= '{json}'";
        }


        protected override object GetData()
        {
            return new
            {
                Address,
                Data = Data.Select(d=>d.ToString("X")).ToArray(),
                Crc
            };
        }
    }
}