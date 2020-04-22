using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.Repository.Abstract;
using Domain.InputDataModel.Shared.StringInseartService.Model;

namespace App.Services.Facade
{
    /// <summary>
    /// Фасад на слоем доступа к БД через репозиторий для StringInsertModelExt
    /// </summary>
    public class StringInsertModelExtRepositoryFacade
    {
        #region fields
        private readonly IStringInsertModelExtRepository _stringInsertModelExtRepository;
        #endregion



        #region ctor
        public StringInsertModelExtRepositoryFacade(IStringInsertModelExtRepository stringInsertModelExtRepository)
        {
            _stringInsertModelExtRepository = stringInsertModelExtRepository;
        }
        #endregion



        #region Methode
        /// <summary>
        /// Вернуть продюсер по ключу.
        /// </summary>
        /// <returns></returns>
        public async Task<StringInsertModelExt> GetAsync(string key)
        {
            return await _stringInsertModelExtRepository.GetSingleAsync(m => m.Key == key);
        }


        /// <summary>
        /// Вернуть список продюсеров.
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<StringInsertModelExt>> GetListAsync()
        {
            return await _stringInsertModelExtRepository.ListAsync();
        }


        /// <summary>
        /// Проверка наличия продюссера по ключу и по Id.
        /// </summary>
        public async Task<bool> IsExistByVarNameAsync(string key)
        {
            return await _stringInsertModelExtRepository.IsExistAsync(m => m.Key == key);
        }


        /// <summary>
        /// Добавить или Обновить Продюсер в репозитории
        /// </summary>
        public async Task<Result> AddOrUpdateAsync(StringInsertModelExt model)
        {
            if (model == null)
                return Result.Failure("model == null");

            if (await IsExistByVarNameAsync(model.Key))
            {
                await _stringInsertModelExtRepository.EditAsync(model);
            }
            else
            {
                //проверка уникальности ключа при добавлении.
                if (await _stringInsertModelExtRepository.IsExistAsync(prod => prod.Key == model.Key))
                {
                    return Result.Failure($"Уже существует в репозитории Key= {model.Key}");
                }
                await _stringInsertModelExtRepository.AddAsync(model);
            }
            return Result.Ok();
        }


        /// <summary>
        /// Удалить produserUnionOption
        /// </summary>
        public async Task<StringInsertModelExt> RemoveAsync(StringInsertModelExt model)
        {
            await _stringInsertModelExtRepository.DeleteAsync(model);
            return model;
        }


        /// <summary>
        /// удалить все StringInsertModelExt из хранилища и БД
        /// </summary>
        public async Task<Result> EraseAsync()
        {
            try
            {
                await _stringInsertModelExtRepository.DeleteAsync(option => true);
            }
            catch (Exception ex)
            {
                return Result.Failure<string>($"ИСКЛЮЧЕНИЕ В StringInsertModelExt EraseAsync: {ex}");
            }
            return Result.Ok();
        }

        #endregion
    }
}