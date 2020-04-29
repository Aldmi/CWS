﻿using System;
using Domain.InputDataModel.Autodictor.Entities;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using Newtonsoft.Json;
using Shared.Extensions;
using Shared.MiddleWares.HandlersOption;
using Xunit;

namespace Shared.Test
{
   public class JsonSerializeDeserializeTest
    {

        [Fact]
        public void StringMiddleWareOption_Deserialize_Test()
        {
            //Arrage
            var str = "{\"converters\":[{\"PadRightStringConverterOption\":{\"Lenght\": 10}}]}";


            //Act
            try
            {
                var converters = JsonConvert.DeserializeObject<StringHandlerMiddleWareOption>(str);
            }
            catch (Exception ex)
            {

            }

            //Asert
            //res.Should().Be("Строка вот такая то 555 69");
        }


        struct Gg
        {
            
        }


        [Fact]
        public void Test()
        {
            var ext= new StringInsertModelExt("ss", ":t", null, null);

            string res;
            int data = 10;
            //res= ext.CalcFinishValue(data);

            DateTime data2 = DateTime.Now;
            res = ext.CalcFinishValue(data2);

            Lang l = Lang.Eng;
            res = ext.CalcFinishValue(l);
   

            //???
           // ConvertByFormat(null);

           // res=  ConvertByFormat(new Gg());

        }



        public string ConvertByFormat(ValueType data)
        {
            var format = ":t";


           // return Convert2StrByFormat111(data, "");

            return data switch
            {
                int intVal => intVal.Convert2StrByFormat(format),//Убрать
                DateTime dateTimeVal => dateTimeVal.Convert2StrByFormat(format),
                //_ => data.Convert2StrByFormat(format) //Должно быть поведение по умолчанию
            };

            throw new InvalidCastException("Тип переданного значнеия не соответсвует ни одному обработчику");
        }


        //Обрабатывает строки 
        public string Convert(string data)
        {
            return StartMiddleWarePipline(data);
        }

        //Обрабатывает ValueRupe
        public string Convert(ValueType data)
        {
            var formatVal= ConvertByFormat(data);
            return StartMiddleWarePipline(formatVal);
        }

        //Конечный конвеер обработки
        public string StartMiddleWarePipline(string data)
        {
            return data + "newData";
        }



        //public string Convert2StrByFormat111<T>(T val, string formatValue) where T : struct
        //{
        //    var format = "{0" + formatValue + "}";
        //    var formatStr = string.Format(format, val);
        //    return formatStr;
        //}




    }
}
