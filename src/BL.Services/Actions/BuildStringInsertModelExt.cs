using Domain.InputDataModel.Shared.Repository.Abstract;
using Domain.InputDataModel.Shared.StringInseartService.Model;
using System;
using System.Threading.Tasks;

namespace App.Services.Actions
{
    public class BuildStringInsertModelExt
    {
        #region field
        private readonly IStringInsertModelExtRepository _repository;
        private readonly StringInsertModelExtStorage _storage;
        #endregion


        #region ctor
        public BuildStringInsertModelExt(IStringInsertModelExtRepository repository, StringInsertModelExtStorage storage)
        {
            _repository = repository;
            _storage = storage;
        }

        public async Task BuildAll()
        {
            await Task.CompletedTask;
        }
        #endregion
    }
}