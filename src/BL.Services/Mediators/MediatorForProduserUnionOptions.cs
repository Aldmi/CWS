using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.Device.Repository.Abstract;
using Domain.Device.Repository.Entities.ResponseProduser;

namespace App.Services.Mediators
{
    /// <summary>
    /// Сервис объединяет работу с репозиотриями опций для устройств.
    /// DeviceOption + ExchangeOption + TransportOption.
    /// </summary>
    public class MediatorForProduserUnionOptions
    {
        #region fields

        private readonly IProduserUnionOptionRepository _produserUnionOptionRep;

        #endregion




        #region ctor

        public MediatorForProduserUnionOptions(IProduserUnionOptionRepository produserUnionOptionRep)
        {
            _produserUnionOptionRep = produserUnionOptionRep;
        }

        #endregion




        #region Methode

        /// <summary>
        /// Вернуть продюсер по ключу.
        /// </summary>
        /// <returns></returns>
        public async Task<ProduserUnionOption> GetProduserUnionOptionAsync(int id)
        {
            return await _produserUnionOptionRep.GetSingleAsync(option => option.Id == id);
        }


        /// <summary>
        /// Вернуть список продюсеров.
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<ProduserUnionOption>> GetProduserUnionOptionsAsync()
        {
            return await _produserUnionOptionRep.ListAsync();
        }


        /// <summary>
        /// Проверка наличия продюссера по ключу и по Id.
        /// </summary>
        public async Task<bool> IsExistProduserUnionAsyncById(int id)
        {
            return await _produserUnionOptionRep.IsExistAsync(prod => prod.Id == id);
        }


        /// <summary>
        /// Добавить или Обновить Продюсер в репозитории
        /// </summary>
        public async Task<Result> AddOrUpdateUnionOptionAsync(ProduserUnionOption produserUnionOption)
        {
            if (produserUnionOption == null)
                return Result.Failure("produserUnionOption == null");

            if (await IsExistProduserUnionAsyncById(produserUnionOption.Id))
            {
                await _produserUnionOptionRep.EditAsync(produserUnionOption);
            }
            else
            {
                //проверка уникальности ключа при добавлении.
                if (await _produserUnionOptionRep.IsExistAsync(prod => prod.Key == produserUnionOption.Key))
                {
                    return Result.Failure($"Уже существует в репозитории Key= {produserUnionOption.Key}");
                }
                await _produserUnionOptionRep.AddAsync(produserUnionOption);
            }
            return Result.Ok();
        }


        /// <summary>
        /// Удалить produserUnionOption
        /// </summary>
        public async Task<ProduserUnionOption> RemoveProduserUnionOptionAsync(ProduserUnionOption produserUnionOption)
        {
            await _produserUnionOptionRep.DeleteAsync(produserUnionOption);
            return produserUnionOption;
        }

        #endregion
    }
}