using System.Collections.Generic;

namespace DAL.Abstract.Entities.Options.Exchange.ProvidersOption
{
    public class ByRulesProviderOption
    {
        public string RuleName4DefaultHandle { get; set; }
        public List<RuleOption> Rules { get; set; }
    }


    public class RuleOption
    {
        public string Name { get; set; }                    //Имя правила, или название команды вида Command_On, Command_Off, Command_Restart, Command_Clear       
        public string AddressDevice { get; set; }           // Адресс ус-ва.
        public string WhereFilter { get; set; }             //Булевое выражение для фильтрации (например "(ArrivalTime > DateTime.Now.AddMinute(-100) && ArrivalTime < DateTime.Now.AddMinute(100)) || ((EventTrain == \"Transit\") && (ArrivalTime > DateTime.Now))")
        public string OrderBy { get; set; }                 //Имя св-ва для фильтрации (например "ArrivalTime").
        public int TakeItems { get; set; }                  //N первых элементов. Если элементов меньше, то ДОПОЛНИТЬ список пустыми элементами.
        public string DefaultItemJson { get; set; }         //Элемент по умолчанию (заменяет null на указанный тип в JSON). "{}" - дефолтный конструктор типа
        public List<ViewRuleOption> ViewRules { get; set; }  //Правила отображения. TakeItems элементов распределяются между правилами для отображения. Например первые 3 элемента отображаются первым правилом, остальные вторым правилом.
    }


    /// <summary>
    /// Правило отображения
    /// </summary>
    public class ViewRuleOption
    {
        public int Id { get; set; }
        public int StartPosition { get; set; }               //Начальная позиция элемента из списка
        public int Count { get; set; }                      //Конечная позиция элемента из списка
        public int BatchSize { get; set; }                   //Разбить отправку на порции по BatchSize.
        public RequestOption RequestOption { get; set; }     //Запрос
        public ResponseOption ResponseOption { get; set; }   //Ответ
    }


    public abstract class RequestResonseOption
    {
        public string Format { get; set; }
        public string Body { get; set; }                     // ТЕЛО (ТОЛЬКО НЕЗАВИСИМЫЕ ДАННЫЕ). Каждый элемент батча подставляет свои данные в Body, затем все элементы Конкатенируются.
    }



    public class RequestOption : RequestResonseOption
    {  
        public int MaxBodyLenght { get; set; }               // Максимальная длина тела запроса
        public string Header { get; set; }                   // НАЧАЛО запроса (ТОЛЬКО ЗАВИСИМЫЕ ДАННЫЕ).
        public string Footer { get; set; }                   // КОНЕЦ ЗАПРОСА (ТОЛЬКО ЗАВИСИМЫЕ ДАННЫЕ).

    }


    public class ResponseOption : RequestResonseOption
    {
        public int Lenght { get; set; }                      // Ожидаемое кол-во байт ОТВЕТА
        public int TimeRespone { get; set; }
    }
}