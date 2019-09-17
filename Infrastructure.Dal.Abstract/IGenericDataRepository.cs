using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Dal.Abstract
{
    public interface IGenericDataRepository<T>
    {
        T GetById(int id);
        Task<T> GetByIdAsync(int id);

        T GetSingle(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
        IReadOnlyList<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties); //?????

        IReadOnlyList<T> List();
        IReadOnlyList<T> List(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> ListAsync();
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate);

        int Count(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        void Add(T entity);
        Task AddAsync(T entity);

        void AddRange(IReadOnlyList<T> entitys); 
        Task AddRangeAsync(IReadOnlyList<T> entitys); 

        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
        Task DeleteAsync(T entity);
        Task DeleteAsync(Expression<Func<T, bool>> predicate);

        void Edit(T entity);
        Task EditAsync(T entity);

        bool IsExist(Expression<Func<T, bool>> predicate);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate);

        Task CreateDb(HowCreateDb howCreateDb);
    }
}