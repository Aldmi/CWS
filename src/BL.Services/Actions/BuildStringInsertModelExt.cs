using Domain.InputDataModel.Shared.StringInseartService.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Services.Mediators;
using CSharpFunctionalExtensions;
using Shared.Enums;

namespace App.Services.Actions
{
    public class BuildStringInsertModelExt
    {
        #region field
        private readonly MediatorForStringInseartModelExt _mediatorForStringInseartModelExt;
        private readonly StringInsertModelExtStorage _storage;
        #endregion


        #region ctor
        public BuildStringInsertModelExt(MediatorForStringInseartModelExt mediatorForStringInseartModelExt, StringInsertModelExtStorage storage)
        {
            _mediatorForStringInseartModelExt = mediatorForStringInseartModelExt;
            _storage = storage;
        }
        #endregion



        #region Methode
        /// <summary>
        /// Создать на базе опций ProdusersUnion и добавить в Storage.
        /// </summary>
        public async Task<IReadOnlyList<StringInsertModelExt>> BuildAll()
        {
            // 1. _mediatorForOptions вытаскивает из базы список StringInseartModelExt
            var models = await _mediatorForStringInseartModelExt.GetListAsync();

            // 2. _storage записывает полученный ProduserUnion
            foreach (var m in models)
            {
                AddOrUpdateInStorage(m.VarName, m);
            }

            return GetValuesByStorage();
        }


        /// <summary>
        /// Сохранить или обновить ProdusersUnion в репозитории и сразу сделать билд новго продюссера в Storage
        /// </summary>
        public async Task<Result<StringInsertModelExt>> AddOrUpdateAndBuildProduserAsync(StringInsertModelExt model)
        {
            //Обновить или добавить в Репозиторий model
            var addOrUpdateOptionResult = await _mediatorForStringInseartModelExt.AddOrUpdateAsync(model);
            if (addOrUpdateOptionResult.IsFailure)
            {
                return Result.Failure<StringInsertModelExt>($"{addOrUpdateOptionResult.Error}");
            }

            //Успешно добавили или обновили данные в репозитории.
            //Обновить или добавить в Storage
            var addOrUpdateResult = AddOrUpdateInStorage(model.VarName, model);
            if (addOrUpdateResult == DictionaryCrudResult.Added || addOrUpdateResult == DictionaryCrudResult.Updated)
                return Result.Ok(model);

            return Result.Failure<StringInsertModelExt>($"ошибка добавления в Storage StringInsertModelExt res= {addOrUpdateResult}");
        }


        /// <summary>
        /// удалить из Репозитория и из Хранилища StringInsertModelExt.
        /// </summary>
        public async Task<StringInsertModelExt> RemoveProduserAsync(StringInsertModelExt model)
        {
            var removed = await _mediatorForStringInseartModelExt.RemoveAsync(model);
            var res = RemoveInStorage(removed.VarName);
            return res != DictionaryCrudResult.KeyNotExist ? removed : null;
        }


        /// <summary>
        /// Вернуть всех StringInsertModelExt из Storage
        /// </summary>
        private IReadOnlyList<StringInsertModelExt> GetValuesByStorage()
        {
            return _storage.Values.ToList();
        }


        /// <summary>
        /// Добавить или обновить StringInsertModelExt в Storage
        /// </summary>
        private DictionaryCrudResult AddOrUpdateInStorage(string key, StringInsertModelExt value)
        {
            return _storage.IsExist(key) ?
                _storage.Update(key, value) :
                _storage.AddNew(key, value);
        }


        /// <summary>
        /// Добавить или обновить StringInsertModelExt в Storage
        /// </summary>
        private DictionaryCrudResult RemoveInStorage(string key)
        {
            return _storage.IsExist(key) ? _storage.Remove(key) : DictionaryCrudResult.KeyNotExist;
        }
        #endregion
    }
}