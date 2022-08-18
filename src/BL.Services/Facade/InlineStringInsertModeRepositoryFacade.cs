using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.InputDataModel.Shared.Repository.Abstract;
using Domain.InputDataModel.Shared.StringInseartService.Model.InlineStringInsert;

namespace App.Services.Facade
{
    public class InlineStringInsertModeRepositoryFacade
    {
        #region fields
        private readonly IInlineStringInsertModelRepository _inlineStrRep;
        #endregion



        #region ctor
        public InlineStringInsertModeRepositoryFacade(IInlineStringInsertModelRepository inlineStrRep)
        {
            _inlineStrRep = inlineStrRep;
        }
        #endregion



        #region Methode
        public async Task<InlineStringInsertModel> GetAsync(string key)
        {
            return await _inlineStrRep.GetSingleAsync(m => m.Key == key);
        }


        public async Task<IReadOnlyList<InlineStringInsertModel>> GetListAsync()
        {
            return await _inlineStrRep.ListAsync();
        }


        public async Task<bool> IsExistByKeyAsync(string key)
        {
            return await _inlineStrRep.IsExistAsync(m => m.Key == key);
        }



        public async Task<Result> AddOrUpdateAsync(InlineStringInsertModel model)
        {
            if (model == null)
                return Result.Failure("model == null");

            if (await IsExistByKeyAsync(model.Key))
            {
                await _inlineStrRep.EditAsync(model);
            }
            else
            {
                //проверка уникальности ключа при добавлении.
                if (await _inlineStrRep.IsExistAsync(prod => prod.Key == model.Key))
                {
                    return Result.Failure($"Уже существует в репозитории Key= {model.Key}");
                }
                await _inlineStrRep.AddAsync(model);
            }
            return Result.Ok();
        }


        public async Task<InlineStringInsertModel> RemoveAsync(InlineStringInsertModel model)
        {
            await _inlineStrRep.DeleteAsync(model);
            return model;
        }


        public async Task<Result> EraseAsync()
        {
            try
            {
                await _inlineStrRep.DeleteAsync(option => true);
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