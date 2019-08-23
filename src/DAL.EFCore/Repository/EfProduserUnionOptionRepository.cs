using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Abstract.Concrete;
using DAL.Abstract.Entities.Options.ResponseProduser;
using DAL.EFCore.Entities.Produser;

namespace DAL.EFCore.Repository
{
    public class EfProduserUnionOptionRepository : EfBaseRepository<EfProduserUnionOption, ProduserUnionOption>, IProduserUnionOptionRepository
    {
        #region ctor

        public EfProduserUnionOptionRepository(string connectionString) : base(connectionString)
        {
        }

        #endregion



        #region CRUD

        public new ProduserUnionOption GetById(int id)
        {
            return base.GetById(id);
        }


        public new async Task<ProduserUnionOption> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }


        public new ProduserUnionOption GetSingle(Expression<Func<ProduserUnionOption, bool>> predicate)
        {
            return base.GetSingle(predicate);
        }


        public new async Task<ProduserUnionOption> GetSingleAsync(Expression<Func<ProduserUnionOption, bool>> predicate)
        {
            return await base.GetSingleAsync(predicate);
        }


        public new IReadOnlyList<ProduserUnionOption> GetWithInclude(params Expression<Func<ProduserUnionOption, object>>[] includeProperties)
        {
            return base.GetWithInclude(includeProperties);
        }


        public new IReadOnlyList<ProduserUnionOption> List()
        {
            return base.List();
        }


        public new IReadOnlyList<ProduserUnionOption> List(Expression<Func<ProduserUnionOption, bool>> predicate)
        {
            return base.List(predicate);
        }


        public new async Task<IReadOnlyList<ProduserUnionOption>> ListAsync()
        {
            return await base.ListAsync();
        }


        public new async Task<IReadOnlyList<ProduserUnionOption>> ListAsync(Expression<Func<ProduserUnionOption, bool>> predicate)
        {
            return await base.ListAsync(predicate);
        }


        public new int Count(Expression<Func<ProduserUnionOption, bool>> predicate)
        {
            return base.Count(predicate);
        }


        public new async Task<int> CountAsync(Expression<Func<ProduserUnionOption, bool>> predicate)
        {
            return await base.CountAsync(predicate);
        }


        public new void Add(ProduserUnionOption entity)
        {
            base.Add(entity);
        }


        public new async Task AddAsync(ProduserUnionOption entity)
        {
            await base.AddAsync(entity);
        }


        public new void AddRange(IReadOnlyList<ProduserUnionOption> entitys)
        {
            base.AddRange(entitys);
        }


        public new async Task AddRangeAsync(IReadOnlyList<ProduserUnionOption> entitys)
        {
            await base.AddRangeAsync(entitys);
        }


        public new void Delete(ProduserUnionOption entity)
        {
            base.Delete(entity);
        }


        public new void Delete(Expression<Func<ProduserUnionOption, bool>> predicate)
        {
            base.Delete(predicate);
        }


        public new async Task DeleteAsync(ProduserUnionOption entity)
        {
            await base.DeleteAsync(entity);
        }


        public new async Task DeleteAsync(Expression<Func<ProduserUnionOption, bool>> predicate)
        {
            await base.DeleteAsync(predicate);
        }


        public new void Edit(ProduserUnionOption entity)
        {
            base.Edit(entity);
        }


        public new async Task EditAsync(ProduserUnionOption entity)
        {
            await base.EditAsync(entity);
        }


        public new bool IsExist(Expression<Func<ProduserUnionOption, bool>> predicate)
        {
            return base.IsExist(predicate);
        }


        public new async Task<bool> IsExistAsync(Expression<Func<ProduserUnionOption, bool>> predicate)
        {
            return await base.IsExistAsync(predicate);
        }

        #endregion
    }
}