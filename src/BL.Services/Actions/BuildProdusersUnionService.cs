using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Services.Mediators;
using BL.Services.Produser;
using InputDataModel.Base.InData;

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

        public async Task<IReadOnlyList<ProdusersUnion<TIn>>> BuildAllProdusers()
        {
            // 1. _mediatorForOptions вытаскивает из базы список ProduserUnionOption
            var produsersUnionOptions = await _mediatorForOptions.GetProduserUnionOptions();

            // 2. _factory создает по 1 на базе ProduserUnionOption ProduserUnion
            //   _mediatorForStorages записывает в storage полученный ProduserUnion
            foreach (var option in produsersUnionOptions)
            {
                var prodUnion= _factory.CreateProduserUnion(option);
                _mediatorForStorages.AddProduserUnion(prodUnion.GetKey, prodUnion);
            }

            return _mediatorForStorages.GetProduserUnions();
        }

        #endregion
    }
}