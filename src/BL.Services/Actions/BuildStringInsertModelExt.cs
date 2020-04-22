using System;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Services.Facade;
using CSharpFunctionalExtensions;
using Shared.Enums;

namespace App.Services.Actions
{
    public class BuildStringInsertModelExt
    {
        #region field
        private readonly StringInsertModelExtRepositoryFacade _stringInsertModelExtRepositoryFacade;
        private readonly StringInsertModelExtStorage _storage;
        #endregion


        #region ctor
        public BuildStringInsertModelExt(StringInsertModelExtRepositoryFacade stringInsertModelExtRepositoryFacade, StringInsertModelExtStorage storage)
        {
            _stringInsertModelExtRepositoryFacade = stringInsertModelExtRepositoryFacade;
            _storage = storage;
        }
        #endregion



        #region Methode

        /// <summary>
        /// Вернуть все StringInsertModelExt из Storage
        /// </summary>
        public IReadOnlyList<StringInsertModelExt> GetValuesFromStorage()
        {
            return _storage.Values.ToList();
        }


        /// <summary>
        /// Вернуть все StringInsertModelExt из Storage
        /// </summary>
        public StringInsertModelExt GetValuesFromStorageByVarName(string key)
        {
            return _storage.Get(key);
        }


        /// <summary>
        /// Создать на базе опций ProdusersUnion и добавить в Storage.
        /// </summary>
        public async Task<IReadOnlyList<StringInsertModelExt>> BuildAll()
        {
            // 1. _mediatorForOptions вытаскивает из базы список StringInseartModelExt
            var models = await _stringInsertModelExtRepositoryFacade.GetListAsync();

            // 2. _storage записывает полученный ProduserUnion
            foreach (var m in models)
            {
                AddOrUpdateInStorage(m.Key, m);
            }

            return GetValuesFromStorage();
        }


        /// <summary>
        /// Сохранить или обновить StringInsertModelExt в репозитории и сразу сделать билд нового продюссера в Storage
        /// </summary>
        public async Task<Result> AddOrUpdateAndBuildListAsync(IReadOnlyList<StringInsertModelExt> models)
        {
            //Проверить на дублирование Key
            var duplicateVarNames = models
                .GroupBy(g => g.Key)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToArray();

            if (duplicateVarNames.Any())
            {
                var agr = duplicateVarNames.Aggregate((current, next) => current + ", " + next);
                return Result.Failure($"Key Не может повторятся {agr}");
            }

            var listResults = new List<Result<StringInsertModelExt>>();
            foreach (var m in models)
            {
                var res = await AddOrUpdateAndBuildAsync(m);
                listResults.Add(res);
            }
            var combineRes= Result.Combine(listResults, "  ");
            return combineRes;
        }


        /// <summary>
        /// Сохранить или обновить StringInsertModelExt в репозитории и сразу сделать билд нового продюссера в Storage
        /// </summary>
        public async Task<Result<StringInsertModelExt>> AddOrUpdateAndBuildAsync(StringInsertModelExt model)
        {
            //Обновить или добавить в Репозиторий model
            var addOrUpdateOptionResult = await _stringInsertModelExtRepositoryFacade.AddOrUpdateAsync(model);
            if (addOrUpdateOptionResult.IsFailure)
            {
                return Result.Failure<StringInsertModelExt>($"{addOrUpdateOptionResult.Error}");
            }

            //Успешно добавили или обновили данные в репозитории.
            //Обновить или добавить в Storage
            var addOrUpdateResult = AddOrUpdateInStorage(model.Key, model);
            if (addOrUpdateResult == DictionaryCrudResult.Added || addOrUpdateResult == DictionaryCrudResult.Updated)
                return Result.Ok(model);

            return Result.Failure<StringInsertModelExt>($"ошибка добавления в Storage StringInsertModelExt res= {addOrUpdateResult}");
        }


        /// <summary>
        /// удалить из Репозитория и из Хранилища StringInsertModelExt.
        /// </summary>
        public async Task<StringInsertModelExt> RemoveAsync(StringInsertModelExt model)
        {
            var removed = await _stringInsertModelExtRepositoryFacade.RemoveAsync(model);
            var res = RemoveInStorage(removed.Key);
            return res != DictionaryCrudResult.KeyNotExist ? removed : null;
        }


        /// <summary>
        /// удалить все StringInsertModelExt из хранилища и БД
        /// </summary>
        public async Task<Result> EraseAsync()
        {
           var (_, isFailure, error) = await _stringInsertModelExtRepositoryFacade.EraseAsync();
           if(isFailure)
               return Result.Failure($"{error}");

           _storage.EraseAll();
           return Result.Ok();
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