using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Base.Response.ResponseValidators;
using Shared.Types;

namespace Domain.InputDataModel.Base.ProvidersOption
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
        public AgregateFilter AgregateFilter { get; set; }  //Список фильтров, результат которых, объединяется в 1 список
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
        public string Body { get; set; }                    
    }



    public class RequestOption : RequestResonseOption
    {  
        public int MaxBodyLenght { get; set; }               // Максимальная длина тела запроса
        public string Header { get; set; }                   // НАЧАЛО запроса (ТОЛЬКО ЗАВИСИМЫЕ ДАННЫЕ).
        public string Footer { get; set; }                   // КОНЕЦ ЗАПРОСА (ТОЛЬКО ЗАВИСИМЫЕ ДАННЫЕ).
    }


    public class ResponseOption
    {
        public int TimeRespone { get; set; }                                         // Время ответа
        public string ValidatorName { get; set; }                                    // Если имя валидатора не указанно, то по умолчанию ставится RequireResponseValidator  
        public LenghtResponseValidatorOption LenghtValidator { get; set; }           // ВАЛИДАТОР проверки длинны ответа     
        public EqualResponseValidatorOption EqualValidator { get; set; }             // ВАЛИДАТОР точного сравнения самого ответа с заданным. 
        public ManualEkrimValidatorOption ManualEkrimValidator { get; set; }         // ВАЛИДАТОР протокола экрим

        public Result<BaseResponseValidator> CreateValidator()
        {
            try
            {
                return ValidatorName switch
                {
                    "LenghtValidator" when LenghtValidator != null => Result.Ok<BaseResponseValidator>(new LenghtResponseValidator(LenghtValidator.ExpectedLenght)),
                    "EqualValidator" when EqualValidator != null => Result.Ok<BaseResponseValidator>(new EqualResponseValidator(new StringRepresentation(EqualValidator.Body, EqualValidator.Format))),
                    "ManualEkrimValidator"  => Result.Ok<BaseResponseValidator>(new ManualEkrimResponseValidator()),
                    _ => Result.Ok<BaseResponseValidator>(new RequireResponseValidator())
                };
            }
            catch (Exception ex)
            {
                return Result.Failure<BaseResponseValidator>($"Исключение при создании ВАЛИДАТОРА ответа {ex.Message}");
            }
        }
    }

    public class LenghtResponseValidatorOption
    {
        public int ExpectedLenght { get; set; }
    }

    public class EqualResponseValidatorOption
    {
        public string Format { get; set; }
        public string Body { get; set; }
    }

    public class ManualEkrimValidatorOption { }
}