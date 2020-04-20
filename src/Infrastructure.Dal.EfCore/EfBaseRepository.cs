using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Infrastructure.Dal.EfCore.DbContext;
using Infrastructure.Dal.EfCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.EfCore
{
    /// <summary>
    /// Базовый тип репозитория для EntitiFramework
    /// </summary>
    /// <typeparam name="TDb">Тип в системе хранения</typeparam>
    /// <typeparam name="TMap">Тип в бизнесс логики</typeparam>
    public abstract class EfBaseRepository<TDb, TMap> : IDisposable
                                                        where TDb : class, IEntity
                                                        where TMap : class
    {
        #region field

        private readonly IMapper _mapper;   //TODO: регистрировать зависимости в WebApiSwc 
        protected readonly Context Context;
        protected readonly DbSet<TDb> DbSet;

        #endregion




        #region ctor

        protected EfBaseRepository(string connectionString, IMapper mapper)
        {
            _mapper = mapper;
            Context = new Context(connectionString);
            DbSet = Context.Set<TDb>();
        }

        #endregion



        #region CRUD

        public virtual TMap GetById(int id)
        {
            var efSpOption = DbSet.Find(id);
            var spOptions = _mapper.Map<TMap>(efSpOption);
            return spOptions;
        }


        public virtual async Task<TMap> GetByIdAsync(int id)
        {
            var efSpOption = await DbSet.FindAsync(id);
            var spOptions = _mapper.Map<TMap>(efSpOption);
            return spOptions;
        }


        public virtual TMap GetSingle(Expression<Func<TMap, bool>> predicate)
        {
            var efPredicate = _mapper.MapExpression<Expression<Func<TDb, bool>>>(predicate);
            var efSpOption = DbSet.SingleOrDefault(efPredicate);
            var spOption = _mapper.Map<TMap>(efSpOption);
            return spOption;
        }


        public virtual async Task<TMap> GetSingleAsync(Expression<Func<TMap, bool>> predicate)
        {
            var efPredicate = _mapper.MapExpression<Expression<Func<TDb, bool>>>(predicate);
            var efSpOption = await DbSet.SingleOrDefaultAsync(efPredicate);
            var spOption = _mapper.Map<TMap>(efSpOption);
            return spOption;
        }

        //TODO: Отладить!!!!  using: (IReadOnlyList<Phone> phones = phoneRepo.GetWithInclude(p=>p.Company);)
        public IReadOnlyList<TMap> GetWithInclude(params Expression<Func<TMap, object>>[] includeProperties)
        {
            var list = new List<Expression<Func<TDb, object>>>();
            foreach (var includeProperty in includeProperties)
            {
                var efIncludeProperty = _mapper.MapExpression<Expression<Func<TDb, object>>>(includeProperty);
                list.Add(efIncludeProperty);
            }
            var result = Include(list.ToArray()).ToList();
            return _mapper.Map<IReadOnlyList<TMap>>(result);
        }


        public virtual IReadOnlyList<TMap> List()
        {
            var efOptions = DbSet.ToList();
            var spOptions = _mapper.Map<IReadOnlyList<TMap>>(efOptions);
            return spOptions;
        }


        public virtual IReadOnlyList<TMap> List(Expression<Func<TMap, bool>> predicate)
        {
            var efPredicate = _mapper.MapExpression<Expression<Func<TDb, bool>>>(predicate);
            var efOptions = DbSet.Where(efPredicate).ToList();
            var spOptions = _mapper.Map<IReadOnlyList<TMap>>(efOptions);
            return spOptions;
        }


        public virtual async Task<IReadOnlyList<TMap>> ListAsync()
        {
            var efOptions = await DbSet.ToListAsync();
            var spOptions = _mapper.Map<IReadOnlyList<TMap>>(efOptions);
            return spOptions;
        }


        public virtual async Task<IReadOnlyList<TMap>> ListAsync(Expression<Func<TMap, bool>> predicate)
        {
            var efPredicate = _mapper.MapExpression<Expression<Func<TDb, bool>>>(predicate);
            var efOptions = await DbSet.Where(efPredicate).ToListAsync();
            var spOptions = _mapper.Map<IReadOnlyList<TMap>>(efOptions);
            return spOptions;
        }


        public virtual int Count(Expression<Func<TMap, bool>> predicate)
        {
            var efPredicate = _mapper.MapExpression<Expression<Func<TDb, bool>>>(predicate);
            return DbSet.Count(efPredicate);
        }


        public virtual async Task<int> CountAsync(Expression<Func<TMap, bool>> predicate)
        {
            var efPredicate = _mapper.MapExpression<Expression<Func<TDb, bool>>>(predicate);
            return await DbSet.CountAsync(efPredicate);
        }


        public virtual void Add(TMap entity)
        {
            var efOptions = _mapper.Map<TDb>(entity);
            DbSet.Add(efOptions);
            Context.SaveChanges();
        }


        public virtual async Task AddAsync(TMap entity)
        {
            var efOptions = _mapper.Map<TDb>(entity);
            await DbSet.AddAsync(efOptions);
            await Context.SaveChangesAsync();
        }


        public virtual void AddRange(IReadOnlyList<TMap> entitys)
        {
            var efOptions = _mapper.Map<IReadOnlyList<TDb>>(entitys);
            DbSet.AddRange(efOptions);
            Context.SaveChanges();
        }


        public virtual async Task AddRangeAsync(IReadOnlyList<TMap> entitys)
        {
            try
            {
                var efOptions = _mapper.Map<IReadOnlyList<TDb>>(entitys);
                await DbSet.AddRangeAsync(efOptions);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            await Context.SaveChangesAsync();
        }


        public virtual void Delete(TMap entity)
        {
            var efOptions = _mapper.Map<TDb>(entity);
            DbSet.Remove(efOptions);
            Context.SaveChanges();
        }


        public virtual void Delete(Expression<Func<TMap, bool>> predicate)
        {
            var efPredicate = _mapper.MapExpression<Expression<Func<TDb, bool>>>(predicate);
            var efOptions = DbSet.Where(efPredicate).ToList();
            DbSet.RemoveRange(efOptions);
            Context.SaveChanges();
        }


        public virtual async Task DeleteAsync(TMap entity)
        {
            var efOptions = _mapper.Map<TDb>(entity);
            DbSet.Remove(efOptions);
            await Context.SaveChangesAsync();
        }


        public virtual async Task DeleteAsync(Expression<Func<TMap, bool>> predicate)
        {
            var efPredicate = _mapper.MapExpression<Expression<Func<TDb, bool>>>(predicate);
            var efOptions = await DbSet.Where(efPredicate).ToListAsync();
            DbSet.RemoveRange(efOptions);
            await Context.SaveChangesAsync();
        }


        public virtual void Edit(TMap entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }


        public virtual async Task EditAsync(TMap entity)
        {
            var efOptions = _mapper.Map<TDb>(entity);
            DbSet.Update(efOptions);
            await Context.SaveChangesAsync();
        }


        public virtual bool IsExist(Expression<Func<TMap, bool>> predicate)
        {
            var efPredicate = _mapper.MapExpression<Expression<Func<TDb, bool>>>(predicate);
            return DbSet.Any(efPredicate);
        }


        public virtual async Task<bool> IsExistAsync(Expression<Func<TMap, bool>> predicate)
        {
            var efPredicate = _mapper.MapExpression<Expression<Func<TDb, bool>>>(predicate);
            return await DbSet.AnyAsync(efPredicate);
        }

        #endregion


        #region Methode

        private IQueryable<TDb> Include(params Expression<Func<TDb, object>>[] includeProperties)
        {
            IQueryable<TDb> query = DbSet.AsNoTracking();
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        #endregion




        #region Disposable

        public void Dispose()
        {
            Context?.Dispose();
        }

        #endregion
    }
}