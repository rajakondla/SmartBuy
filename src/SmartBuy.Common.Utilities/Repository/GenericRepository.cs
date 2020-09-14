using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartBuy.Common.Utilities.Repository
{
    public interface IGenericReadRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> All();
        Task<IEnumerable<TEntity>> AllAsync();
        IEnumerable<TEntity> AllInclude
        (params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> AllIncludeAsync
        (params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> FindByInclude
          (Expression<Func<TEntity, bool>> predicate,
          params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> FindByIncludeAsync
          (Expression<Func<TEntity, bool>> predicate,
          params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity FindByKey(int id);
        Task<TEntity> FindByKeyAsync(int id);
    }

    public class GenericReadRepository<TEntity> : IGenericReadRepository<TEntity>
        where TEntity : class
    {
        private DbContext _context;
        private DbSet<TEntity> _dbSet;

        public GenericReadRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> All()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public IEnumerable<TEntity> AllInclude
        (params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetAllIncluding(includeProperties).ToList();
        }

        public async Task<IEnumerable<TEntity>> AllIncludeAsync
        (params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await GetAllIncluding(includeProperties).ToListAsync();
        }

        public IEnumerable<TEntity> FindByInclude
          (Expression<Func<TEntity, bool>> predicate,
          params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            IEnumerable<TEntity> results = query.Where(predicate).ToList();
            return results;
        }

        public async Task<IEnumerable<TEntity>> FindByIncludeAsync
          (Expression<Func<TEntity, bool>> predicate,
          params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            return await query.Where(predicate).ToListAsync();
        }

        private IQueryable<TEntity> GetAllIncluding
        (params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = _dbSet.AsNoTracking();

            return includeProperties.Aggregate
              (queryable, (current, includeProperty) => current.Include(includeProperty));
        }

        public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> results = _dbSet.AsNoTracking()
              .Where(predicate).ToList();

            return results;
        }

        public async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public TEntity FindByKey(int id)
        {
            Expression<Func<TEntity, bool>> lambda = Helper.BuildLambdaForFindByKey<TEntity>(id);
            return _dbSet.AsNoTracking().SingleOrDefault(lambda);
        }

        public Task<TEntity> FindByKeyAsync(int id)
        {
            Expression<Func<TEntity, bool>> lambda = Helper.BuildLambdaForFindByKey<TEntity>(id);
            return _dbSet.AsNoTracking().SingleOrDefaultAsync(lambda);
        }
    }

    public class GenericReadWriteRepository<TEntity>
        : GenericReadRepository<TEntity> where TEntity : class
    {
        private DbContext _context;
        private DbSet<TEntity> _dbSet;

        public GenericReadWriteRepository(DbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public async Task InsertAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var entity = FindByKey(id);
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = FindByKey(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
