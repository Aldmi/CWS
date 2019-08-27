using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Services.Mediators;
using BL.Services.Produser;
using DAL.Abstract.Entities.Options.ResponseProduser;
using InputDataModel.Base.InData;
using Shared.Enums;

namespace BL.Services.Actions
{
    /// <summary>
    /// Создать коллекцию ProdusersUnion на базе опций и записать их в Storage.
    /// </summary>
    public class BuildProdusersUnionService<TIn> where TIn : InputTypeBase
    {
        #region field

        private readonly MediatorForOptions _mediatorForOptions;
        private readonly MediatorForStorages<TIn> _mediatorForStorages;
        private readonly ProdusersUnionFactory<TIn> _factory;

        #endregion



        #region ctor

        public BuildProdusersUnionService(MediatorForOptions mediatorForOptions, MediatorForStorages<TIn> mediatorForStorages, ProdusersUnionFactory<TIn> factory)
        {
            _mediatorForOptions = mediatorForOptions;
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
            var produsersUnionOptions = await _mediatorForOptions.GetProduserUnionOptionsAsync();

            // 2. _factory создает по 1 на базе ProduserUnionOption ProduserUnion
            //   _mediatorForStorages записывает в storage полученный ProduserUnion
            foreach (var option in produsersUnionOptions)
            {
                var prodUnion = _factory.CreateProduserUnion(option);
                _mediatorForStorages.AddOrUpdateProduserUnion(prodUnion.GetKey, prodUnion);
            }

            return _mediatorForStorages.GetProduserUnions();
        }


        /// <summary>
        /// Сохранить или обновить ProdusersUnion в репозитории и сразу сделать билд новго продюссера в Storage
        /// </summary>
        public async Task<ProdusersUnion<TIn>> SaveOrUpdateAndBuildProduserAsync(ProduserUnionOption produsersUnionOption)
        {
            //Обновить или добавить в Репозиторий produsersUnionOption
            var succsses = await _mediatorForOptions.AddOrUpdateUnionOptionAsync(produsersUnionOption); //TODO: вызов Exception заменить на возврат Result<T,T>

            //Если успешно обновили или добавили в репозиторий.
            if (succsses)
            {
                // сбилдить ProduserUnion
                var prodUnion = _factory.CreateProduserUnion(produsersUnionOption);
                // Обновить или добавить в Storage
                var res = _mediatorForStorages.AddOrUpdateProduserUnion(prodUnion.GetKey, prodUnion);
                if (res == DictionaryCrudResult.Added || res == DictionaryCrudResult.Updated)
                    return prodUnion;
            }
            return null;
        }


        /// <summary>
        /// удалить из Репозитория и из Хранилища produserUnion.
        /// </summary>
        public async Task<ProduserUnionOption> RemoveProduserAsync(ProduserUnionOption produserUnionOption)
        {
            var removed = await _mediatorForOptions.RemoveProduserUnionOptionAsync(produserUnionOption);
            var res= _mediatorForStorages.RemoveProduserUnion(removed.Key);
            return res != DictionaryCrudResult.KeyNotExist ? removed : null;
        }

        #endregion
    }
}