using Allegory.Entities.Abstract;
using Allegory.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Allegory.EntityRepository
{
    public abstract class EntityRepositoryBase
    {
        public static PagedResult<T> GetPaged<T>(IQueryable<T> query, int page, int pageSize) where T : class, new()
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }
    public abstract class EntityRepositoryBase<TEntity> : IEntityRepository<TEntity> where TEntity : class, IEntity, new()
    {
        protected bool IsAssignableFromICreatedDate, IsAssignableFromIModifiedDate;
        public EntityRepositoryBase()
        {
            IsAssignableFromICreatedDate = typeof(ICreatedDate).IsAssignableFrom(typeof(TEntity));
            IsAssignableFromIModifiedDate = typeof(IModifiedDate).IsAssignableFrom(typeof(TEntity));
        }

        public abstract TEntity Get(Expression<Func<TEntity, bool>> filter);
        public abstract List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null);

        protected abstract TEntity AddOrm(TEntity entity);
        protected abstract TEntity UpdateOrm(TEntity entity);
        public abstract void Delete(TEntity entity);

        protected abstract List<TEntity> AddOrm(List<TEntity> entities);
        protected abstract List<TEntity> UpdateOrm(List<TEntity> entities);
        protected abstract void DeleteOrm(List<TEntity> entities);

        public abstract PagedResult<TEntity> GetPagedList<TKey>(Expression<Func<TEntity, TKey>> order, int page = 1, int pageSize = 20, Expression<Func<TEntity, bool>> filter = null, bool desc = false);

        public TEntity Add(TEntity entity)
        {
            SetForAdd(entity);
            return AddOrm(entity);
        }
        public TEntity Update(TEntity entity)
        {
            SetForUpdate(entity);
            return UpdateOrm(entity);
        }

        public List<TEntity> Add(List<TEntity> entities)
        {
            if (entities == null || entities.Count == 0)
                return entities;
            SetForAdd(entities);
            return AddOrm(entities);
        }
        public List<TEntity> Update(List<TEntity> entities)
        {
            if (entities == null || entities.Count == 0)
                return entities;
            SetForUpdate(entities);
            return UpdateOrm(entities);
        }
        public void Delete(List<TEntity> entities)
        {
            if (entities == null || entities.Count == 0)
                return;
            DeleteOrm(entities);
        }

       
        protected void SetForAdd(TEntity entity)
        {
            if (IsAssignableFromICreatedDate)
                ((ICreatedDate)entity).CreatedDate = DateTime.Now;
            if (IsAssignableFromIModifiedDate)
                ((IModifiedDate)entity).ModifiedDate = null;
        }
        protected void SetForAdd(List<TEntity> entities)
        {
            if (IsAssignableFromICreatedDate)
                entities.ForEach(X => { (X as ICreatedDate).CreatedDate = DateTime.Now; });
            if (IsAssignableFromIModifiedDate)
                entities.ForEach(X => { (X as IModifiedDate).ModifiedDate = null; });
        }
        protected void SetForUpdate(TEntity entity)
        {
            if (IsAssignableFromIModifiedDate)
                ((IModifiedDate)entity).ModifiedDate = DateTime.Now;
        }
        protected void SetForUpdate(List<TEntity> entities)
        {
            if (IsAssignableFromIModifiedDate)
                entities.ForEach(X => { (X as IModifiedDate).ModifiedDate = DateTime.Now; });
        }
    }
}
