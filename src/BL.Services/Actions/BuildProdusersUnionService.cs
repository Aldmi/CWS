using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Services.Mediators;
using BL.Services.Produser;
using DeviceForExchange;
using InputDataModel.Base.InData;

namespace BL.Services.Actions
{
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




        public async Task<IReadOnlyList<ProdusersUnion<TIn>>> BuildAllProdusers()
        {
            // 1._mediatorForOptions вытаскивает из базы список ProduserUnionOption
            // 2. _factory создает по 1 на базе ProduserUnionOption ProduserUnion
            // 3. _mediatorForStorages записывает в storage полученный ProduserUnion

            return null;
        }

    }
}