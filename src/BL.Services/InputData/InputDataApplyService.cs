using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using App.Services.Mediators;
using CSharpFunctionalExtensions;
using Domain.Exchange.Models;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Newtonsoft.Json;
using Serilog;

namespace App.Services.InputData
{
    public class InputDataApplyService<TIn> where TIn : InputTypeBase
    {
        private readonly MediatorForStorages<TIn> _mediatorForStorages;
        private readonly ILogger _logger;


        #region ctor

        public InputDataApplyService(MediatorForStorages<TIn> mediatorForStorages, ILogger logger)
        {
            _mediatorForStorages = mediatorForStorages;
            _logger = logger;
        }

        #endregion




        #region Methode
        /// <summary>
        /// Найти все ус-ва по имени и передать им данные.
        /// </summary>
        /// <param name="inputDatas">Данные для нескольких ус-в</param>
        /// <returns>Словарь состояний обменов для устройств или ошибку</returns>
        public async Task<Result<Dictionary<string, IReadOnlyList<ExchangeInfoModel>>>> ApplyInputData(IReadOnlyList<InputData<TIn>> inputDatas)
        {
            //найти Device по имени и передать ему данные 
            var errorsSb= new StringBuilder();
            var tasks = new List<Task>();
            var dictResult= new Dictionary<string, IReadOnlyList<ExchangeInfoModel>>();
            foreach (var inData in inputDatas)
            {
                var device = _mediatorForStorages.GetDevice(inData.DeviceName);
                if (device == null)
                {
                    errorsSb.AppendLine($"устройство не найдено: {inData.DeviceName}");
                    continue;
                }

                if(!string.IsNullOrEmpty(inData.ExchangeName) && (_mediatorForStorages.GetExchange(inData.ExchangeName) == null))
                { 
                   errorsSb.AppendLine($"Обмен не найден: {inData.ExchangeName}");
                  continue;
                }

                tasks.Add(device.Resive(inData));
                dictResult.Add(inData.DeviceName, device.GetExchnagesInfo(inData.ExchangeName));
                LogingInData(inData);
            }
            await Task.WhenAll(tasks);
            return errorsSb.Length > 0 
                ? Result.Failure<Dictionary<string, IReadOnlyList<ExchangeInfoModel>>>(errorsSb.ToString()) 
                : Result.Ok(dictResult);
        }


        /// <summary>
        /// Логирование входных данных, Прошедших валидацию
        /// </summary>
        /// <param name="inData"></param>
        private void LogingInData(InputData<TIn> inData)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,             //Отступы дочерних элементов 
                NullValueHandling = NullValueHandling.Ignore  //Игнорировать пустые теги
            };
   
            var infoObj = new
            {
                inData.DeviceName,
                inData.ExchangeName,
                DataAction= inData.DataAction.ToString(),
                Command = inData.Command.ToString(),
                inData.DirectHandlerName,   
                DataCount = inData.Data?.Count ?? 0
            };
            var infoObjsJson = JsonConvert.SerializeObject(infoObj, settings);
            _logger.Information($"ПРИНЯТЫ ДАННЫЕ ДЛЯ: {infoObjsJson}");
            if (inData.Command == Command4Device.None)
            {
                var debugObjsJson = JsonConvert.SerializeObject(inData, settings);
                _logger.Debug($"ПРИНЯТЫ ДАННЫЕ ПОДРОБНО: {debugObjsJson}"); 
            }
        }

        #endregion
    }
}