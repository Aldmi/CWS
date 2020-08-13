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


        /// <summary>
        ///Конвертировать в JSON. Beauty -для людей, с форрматированием  
        /// </summary>
        public static string Serialize2BeautyJson(object response)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,                 //Отступы дочерних элементов ДА
                NullValueHandling = NullValueHandling.Ignore      //Игнорировать пустые теги
            };

            var jsonResp = JsonConvert.SerializeObject(response, settings);
            return jsonResp;
        }
    }
}