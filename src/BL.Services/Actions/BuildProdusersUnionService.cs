using System.Collections.Generic;
using System.Threading.Tasks;
using App.Services.Mediators;
using CSharpFunctionalExtensions;
using DAL.Abstract.Entities.Options.ResponseProduser;
using Domain.Device.Produser;
using Domain.InputDataModel.Base.InData;
using Shared.Enums;

namespace App.Services.Actions
{
    /// <summary>
    /// Создать коллекцию ProdusersUnion на базе опций и записать их в Storage.
    /// </summary>
    public class BuildProdusersUnionService<TIn> where TIn : InputTypeBase
    {
        #region field

        private readonly MediatorForProduserUnionOptions _mediatorForProduserUnionOptions;
        private readonly MediatorForStorages<TIn> _mediatorForStorages;
        private readonly ProdusersUnionFactory<TIn> _factory;

        #endregion



        #region ctor

        public BuildProdusersUnionService(MediatorForProduserUnionOptions mediatorForProduserUnionOptions, MediatorForStorages<TIn> mediatorForStorages, ProdusersUnionFactory<TIn> factory)
        {
            _mediatorForProduserUnionOptions = mediatorForProduserUnionOptions;
            _mediatorForStorages = mediatorForStorages;
            _factory = factory;
        }

        #endregion



        #region Methode

        /// <summary>
        /// Создать на базе опций ProdusersUnion и добавить в Storage.
        /// </summary>
        public async Task<IReadOnlyList<ProdusersUnion<TIn>>> BuildAllProdusers()
        {
            // 1. _mediatorForOptions вытаскивает из базы список ProduserUnionOption
            var produsersUnionOptions = await _mediatorForProduserUnionOptions.GetProduserUnionOptionsAsync();

            // 2. _factory создает по 1 на базе ProduserUnionOption ProduserUnion
            //   _mediatorForStorages записывает в storage полученный ProduserUnion
            foreach (var option in produsersUnionOptions)
            {
                var (_, _, value) = _factory.CreateProduserUnion(option);
                _mediatorForStorages.AddOrUpdateProduserUnion(value.GetKey, value);
            }

            return _mediatorForStorages.GetProduserUnions();
        }


        /// <summary>
        /// Сохранить или обновить ProdusersUnion в репозитории и сразу сделать билд новго продюссера в Storage
        /// </summary>
        public async Task<Result<ProdusersUnion<TIn>>> AddOrUpdateAndBuildProduserAsync(ProduserUnionOption produsersUnionOption)
        {
            //Обновить или добавить в Репозиторий produsersUnionOption
            var addOrUpdateOptionResult = await _mediatorForProduserUnionOptions.AddOrUpdateUnionOptionAsync(produsersUnionOption);
            if (addOrUpdateOptionResult.IsFailure)
            {
                return Result.Fail<ProdusersUnion<TIn>>($"{addOrUpdateOptionResult.Error}");
            }

            //Если успешно обновили или добавили ProduserUnionOption в репозиторий, сбилдить ProduserUnion
            var createResult = _factory.CreateProduserUnion(produsersUnionOption);
            if (createResult.IsFailure)
                return createResult;

            // Обновить или добавить в Storage
            var prodUnion = createResult.Value;
            var addOrUpdateProduserUnionResult = _mediatorForStorages.AddOrUpdateProduserUnion(prodUnion.GetKey, prodUnion);
            if (addOrUpdateProduserUnionResult == DictionaryCrudResult.Added || addOrUpdateProduserUnionResult == DictionaryCrudResult.Updated)
                return Result.Ok(prodUnion);

            return Result.Fail<ProdusersUnion<TIn>>($"ошибка добавления в Storage ProduserUnion res= {addOrUpdateProduserUnionResult}");
        }


        /// <summary>
        /// удалить из Репозитория и из Хранилища produserUnion.
        /// </summary>
        public async Task<ProduserUnionOption> RemoveProduserAsync(ProduserUnionOption produserUnionOption)
        {
            var removed = await _mediatorForProduserUnionOptions.RemoveProduserUnionOptionAsync(produserUnionOption);
            var res = _mediatorForStorages.RemoveProduserUnion(removed.Key);
            return res != DictionaryCrudResult.KeyNotExist ? removed : null;
        }

        #endregion
    }
}