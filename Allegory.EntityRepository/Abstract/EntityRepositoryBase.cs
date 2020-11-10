using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Allegory.Entities.Abstract;
using Allegory.Entities.Concrete;

namespace Allegory.EntityRepository.Abstract
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
        public static PagedResult<T> GetPaged<T,TKey>(IQueryable<T> query,Expression<Func<T, TKey>> order, int page = 1, int pageSize = 20, Expression<Func<T, bool>> filter = null, bool desc = false) where T : class, new()
        {
            if (filter != null)
                query = query.Where(filter);
            if (desc)
                query = query.OrderByDescending(order);
            else
                query = query.OrderBy(order);

            return GetPaged(query, page, pageSize);
        }
    }
    public abstract class EntityRepositoryBase<TEntity> : IEntityRepository<TEntity> where TEntity : class, IEntity, new()
    {
        protected bool IsAssignableFromICreatedDate, IsAssignableFromIModifiedDate, IsAssignableFromICreatedBy, IsAssignableFromIModifiedBy;
        public EntityRepositoryBase()
        {
            IsAssignableFromICreatedDate = typeof(ICreatedDate).IsAssignableFrom(typeof(TEntity));
            IsAssignableFromIModifiedDate = typeof(IModifiedDate).IsAssignableFrom(typeof(TEntity));
            IsAssignableFromICreatedBy = typeof(ICreatedBy).IsAssignableFrom(typeof(TEntity));
            IsAssignableFromIModifiedBy = typeof(IModifiedBy).IsAssignableFrom(typeof(TEntity));
        }

        public abstract TEntity Get(Expression<Func<TEntity, bool>> filter);
        public abstract TEntity GetSingle(Expression<Func<TEntity, bool>> filter);
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
            if (IsAssignableFromICreatedBy)
                ((ICreatedBy)entity).CreatedBy = (System.Threading.Thread.CurrentPrincipal.Identity as IUserIdentity).UserRef;
            if (IsAssignableFromIModifiedBy)
                ((IModifiedBy)entity).ModifiedBy = null;
        }
        protected void SetForAdd(List<TEntity> entities)
        {
            if (IsAssignableFromICreatedDate)
                entities.ForEach(X => { (X as ICreatedDate).CreatedDate = DateTime.Now; });
            if (IsAssignableFromIModifiedDate)
                entities.ForEach(X => { (X as IModifiedDate).ModifiedDate = null; });
            if (IsAssignableFromICreatedBy)
                entities.ForEach(X => { (X as ICreatedBy).CreatedBy = (System.Threading.Thread.CurrentPrincipal.Identity as IUserIdentity).UserRef; });
            if (IsAssignableFromIModifiedBy)
                entities.ForEach(X => { (X as IModifiedBy).ModifiedBy = null; });
        }
        protected void SetForUpdate(TEntity entity)
        {
            if (IsAssignableFromIModifiedDate)
                ((IModifiedDate)entity).ModifiedDate = DateTime.Now;
            if (IsAssignableFromIModifiedBy)
                ((IModifiedBy)entity).ModifiedBy = (System.Threading.Thread.CurrentPrincipal.Identity as IUserIdentity).UserRef;
        }
        protected void SetForUpdate(List<TEntity> entities)
        {
            if (IsAssignableFromIModifiedDate)
                entities.ForEach(X => { (X as IModifiedDate).ModifiedDate = DateTime.Now; });
            if (IsAssignableFromIModifiedBy)
                entities.ForEach(X => { (X as IModifiedBy).ModifiedBy = (System.Threading.Thread.CurrentPrincipal.Identity as IUserIdentity).UserRef; });
        }
    }
}
