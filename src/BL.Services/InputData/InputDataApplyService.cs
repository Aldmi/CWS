using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Services.Mediators;
using InputDataModel.Base;
using Newtonsoft.Json;
using Serilog;

namespace BL.Services.InputData
{
    public class InputDataApplyService<TIn>
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
        public async Task<IEnumerable<string>> ApplyInputData(IEnumerable<InputData<TIn>> inputDatas)
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

                if (string.IsNullOrEmpty(inData.ExchangeName))
                {
                    tasks.Add(device.Send2AllExchanges(inData.DataAction, inData.Data, inData.Command));
                }
                else
                {
                    tasks.Add(device.Send2ConcreteExchanges(inData.ExchangeName, inData.DataAction, inData.Data, inData.Command, inData.DirectHandlerName));
                }
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
                inData.DataAction,
                inData.Command,
                inData.DirectHandlerName,
                DataCount = inData.Data.Count
            };
            var infoObjsJson = JsonConvert.SerializeObject(infoObj, settings);
            _logger.Information($"ПРИНЯТЫ ДАННЫЕ ДЛЯ: {infoObjsJson}");
            var debugObjsJson = JsonConvert.SerializeObject(inData, settings);
            _logger.Debug($"ПРИНЯТЫ ДАННЫЕ ПОДРОБНО: {debugObjsJson}");
        }


        #endregion
    }
}