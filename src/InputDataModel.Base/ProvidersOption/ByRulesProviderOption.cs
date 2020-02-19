﻿using System.Collections.Generic;
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
        public string Body { get; set; }                    
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
        public int TimeRespone { get; set; }                 // Время ответа
        public string StronglyTypedName  { get; set; }       // Название типа в которое преобразуется Body. (Типизированный ответ)
    }



    //TEST----------------------------------------------------------------------------------
    public class ResponseOption2
    {
        public int TimeRespone { get; set; }                                         //Время ответа
        public string ValidatorName { get; set; }                                    //Если имя валидатора не указанно, то по умолчанию ставится RequireResponseValidator  
        public LenghtResponseValidatorOption LenghtValidator { get; set; }
        public EqualResponseValidatorOption EqualValidator { get; set; }


        public Result<BaseResponseValidator> CreateValidator()
        {
            return ValidatorName switch
            {
                "LenghtValidator" when LenghtValidator != null => Result.Ok<BaseResponseValidator>(new LenghtResponseValidator(LenghtValidator.ExpectedLenght)),
                "EqualValidator" when EqualValidator != null => Result.Ok<BaseResponseValidator>(new EqualResponseValidator(new StringRepresentation(EqualValidator.Body, EqualValidator.Format))),
                "ManualEkrimValidator" when LenghtValidator != null => Result.Ok<BaseResponseValidator>(new ManualEkrimResponseValidator()),
                _ => Result.Ok<BaseResponseValidator>(new RequireResponseValidator())
            };
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
}