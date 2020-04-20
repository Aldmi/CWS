using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.Repository.Abstract;
using Domain.InputDataModel.Shared.StringInseartService.Model;

namespace App.Services.Mediators
{
    public class MediatorForStringInseartModelExt
    {
        #region fields
        private readonly IStringInsertModelExtRepository _stringInsertModelExtRepository;
        #endregion



        #region ctor
        public MediatorForStringInseartModelExt(IStringInsertModelExtRepository stringInsertModelExtRepository)
        {
            _stringInsertModelExtRepository = stringInsertModelExtRepository;
        }
        #endregion



        #region Methode
        /// <summary>
        /// Вернуть продюсер по ключу.
        /// </summary>
        /// <returns></returns>
        public async Task<StringInsertModelExt> GetAsync(int id)
        {
            return await _stringInsertModelExtRepository.GetSingleAsync(m => m.Id == id);
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
        public async Task<bool> IsExistByIdAsync(int id)
        {
            return await _stringInsertModelExtRepository.IsExistAsync(m => m.Id == id);
        }


        /// <summary>
        /// Добавить или Обновить Продюсер в репозитории
        /// </summary>
        public async Task<Result> AddOrUpdateAsync(StringInsertModelExt model)
        {
            if (model == null)
                return Result.Failure("model == null");

            if (await IsExistByIdAsync(model.Id))
            {
                await _stringInsertModelExtRepository.EditAsync(model);
            }
            else
            {
                //проверка уникальности ключа при добавлении.
                if (await _stringInsertModelExtRepository.IsExistAsync(prod => prod.VarName == model.VarName))
                {
                    return Result.Failure($"Уже существует в репозитории VarName= {model.VarName}");
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

        #endregion
    }
}