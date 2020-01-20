using System;
using Newtonsoft.Json;



namespace Shared.Helpers
{
    public static class HelpersJson
    {
        /// <summary>
        ///Конвертировать в JSON. Raw -для машин, без отступов  
        /// </summary>
        public static string Serialize2RawJson(object response)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,                     //Отступы дочерних элементов НЕТ
                NullValueHandling = NullValueHandling.Ignore      //Игнорировать пустые теги
            };
          
            var jsonResp = JsonConvert.SerializeObject(response, settings);
            return jsonResp;
        }
    }
}