using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Services.Mediators;
using InputDataModel.Base;
using InputDataModel.Base.InData;
using Newtonsoft.Json;
using Serilog;

namespace BL.Services.InputData
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
        /// <returns>Список ОШИБОК</returns>
        public async Task<IReadOnlyList<string>> ApplyInputData(IReadOnlyList<InputData<TIn>> inputDatas)
        {
            //найти Device по имени и передать ему данные 
            var errors= new List<string>();
            var tasks = new List<Task>();
            foreach (var inData in inputDatas)
            {
                var device = _mediatorForStorages.GetDevice(inData.DeviceName);
                if (device == null)
                {
                    errors.Add($"устройство не найденно: {inData.DeviceName}");
                    continue;
                }

                if(!string.IsNullOrEmpty(inData.ExchangeName) && (_mediatorForStorages.GetExchange(inData.ExchangeName) == null))
                {
                  errors.Add($"Обмен не найденн: {inData.ExchangeName}");
                  continue;
                }

                LogingInData(inData);
                tasks.Add(device.Resive(inData));
            }

            await Task.WhenAll(tasks);
            return errors;
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
            if (inData.Command != Command4Device.None)
            {
                var debugObjsJson = JsonConvert.SerializeObject(inData, settings);
                _logger.Information($"ПРИНЯТЫ ДАННЫЕ ПОДРОБНО: {debugObjsJson}"); //TODO: поменять на DEBUG
            }
        }


        #endregion
    }
}