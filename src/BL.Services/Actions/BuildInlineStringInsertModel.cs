using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Services.Facade;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.StringInseartService.Model.InlineStringInsert;
using Shared.Enums;

namespace App.Services.Actions
{
    public class BuildInlineStringInsertModel
    {
        #region field
        private readonly InlineStringInsertModeRepositoryFacade _inlineStrRep;
        private readonly InlineStringInsertModelStorage _storage;
        #endregion


        #region ctor
        public BuildInlineStringInsertModel(InlineStringInsertModeRepositoryFacade inlineStrRep, InlineStringInsertModelStorage storage)
        {
            _inlineStrRep = inlineStrRep;
            _storage = storage;
        }
        #endregion



        #region Methode
        /// <summary>
        /// Вернуть все InlineStringInsertModel из Storage
        /// </summary>
        public IReadOnlyList<InlineStringInsertModel> GetValuesFromStorage()
        {
            return _storage.Values.ToList();
        }


        /// <summary>
        /// Вернуть все StringInsertModelExt из Storage
        /// </summary>
        public InlineStringInsertModel GetValuesFromStorageByVarName(string key)
        {
            return _storage.Get(key);
        }


        /// <summary>
        /// Создать на базе опций ProdusersUnion и добавить в Storage.
        /// </summary>
        public async Task<IReadOnlyList<InlineStringInsertModel>> BuildAll()
        {
            var models = await _inlineStrRep.GetListAsync();

            foreach (var m in models)
            {
                AddOrUpdateInStorage(m.Key, m);
            }

            return GetValuesFromStorage();
        }


        /// <summary>
        /// Сохранить или обновить StringInsertModelExt в репозитории и сразу сделать билд нового продюссера в Storage
        /// </summary>
        public async Task<Result> AddOrUpdateAndBuildListAsync(IReadOnlyList<InlineStringInsertModel> models)
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

            var listResults = new List<Result<InlineStringInsertModel>>();
            foreach (var m in models)
            {
                var res = await AddOrUpdateAndBuildAsync(m);
                listResults.Add(res);
            }
            var combineRes = Result.Combine(listResults, "  ");
            return combineRes;
        }



        /// <summary>
        /// Сохранить или обновить StringInsertModelExt в репозитории и сразу сделать билд нового продюссера в Storage
        /// </summary>
        public async Task<Result<InlineStringInsertModel>> AddOrUpdateAndBuildAsync(InlineStringInsertModel model)
        {
            //Обновить или добавить в Репозиторий model
            var result = await _inlineStrRep.AddOrUpdateAsync(model);
            if (result.IsFailure)
            {
                return Result.Failure<InlineStringInsertModel>($"{result.Error}");
            }

            //Успешно добавили или обновили данные в репозитории.
            //Обновить или добавить в Storage
            var addOrUpdateResult = AddOrUpdateInStorage(model.Key, model);
            if (addOrUpdateResult == DictionaryCrudResult.Added || addOrUpdateResult == DictionaryCrudResult.Updated)
                return Result.Ok(model);

            return Result.Failure<InlineStringInsertModel>($"ошибка добавления в Storage StringInsertModelExt res= {addOrUpdateResult}");
        }


        /// <summary>
        /// удалить из Репозитория и из Хранилища StringInsertModelExt.
        /// </summary>
        public async Task<InlineStringInsertModel> RemoveAsync(InlineStringInsertModel model)
        {
            var removed = await _inlineStrRep.RemoveAsync(model);
            var res = RemoveInStorage(removed.Key);
            return res != DictionaryCrudResult.KeyNotExist ? removed : null;
        }


        /// <summary>
        /// удалить все StringInsertModelExt из хранилища и БД
        /// </summary>
        public async Task<Result> EraseAsync()
        {
            var (_, isFailure, error) = await _inlineStrRep.EraseAsync();
            if (isFailure)
                return Result.Failure($"{error}");

            _storage.EraseAll();
            return Result.Ok();
        }


        /// <summary>
        /// Добавить или обновить StringInsertModelExt в Storage
        /// </summary>
        private DictionaryCrudResult AddOrUpdateInStorage(string key, InlineStringInsertModel value)
        {
            return _storage.ContainsKey(key) ?
                _storage.Update(key, value) :
                _storage.AddNew(key, value);
        }

        /// <summary>
        /// Добавить или обновить StringInsertModelExt в Storage
        /// </summary>
        private DictionaryCrudResult RemoveInStorage(string key)
        {
            return _storage.ContainsKey(key) ? _storage.Remove(key) : DictionaryCrudResult.KeyNotExist;
        }
        #endregion
    }
}