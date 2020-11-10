using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Allegory.Entities.Abstract;
using Allegory.Entities.Concrete;

namespace Allegory.EntityRepository.Abstract
{
    public abstract class EntityRepositoryBaseWithKey<TEntity, TKey> : IEntityRepository<TEntity, TKey>
        where TEntity : class, IKey<TKey>, new()
        where TKey : IComparable, IFormattable, IConvertible, IComparable<TKey>, IEquatable<TKey>
    {
        #region EntityRepositoryBase
        protected EntityRepositoryBase<TEntity> EntityRepositoryBase { get; set; }

        public TEntity Get(Expression<Func<TEntity, bool>> filter) => EntityRepositoryBase.Get(filter);
        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter) => EntityRepositoryBase.GetSingle(filter);
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null) => EntityRepositoryBase.GetList(filter);

        public TEntity Add(TEntity entity) => EntityRepositoryBase.Add(entity);
        public TEntity Update(TEntity entity) => EntityRepositoryBase.Update(entity);
        public void Delete(TEntity entity) => EntityRepositoryBase.Delete(entity);

        public List<TEntity> Add(List<TEntity> entities) => EntityRepositoryBase.Add(entities);
        public List<TEntity> Update(List<TEntity> entities) => EntityRepositoryBase.Update(entities);
        public void Delete(List<TEntity> entities) => EntityRepositoryBase.Delete(entities);

        public PagedResult<TEntity> GetPagedList<TOrder>(Expression<Func<TEntity, TOrder>> order, int page = 1, int pageSize = 20, Expression<Func<TEntity, bool>> filter = null, bool desc = false) => EntityRepositoryBase.GetPagedList(order, page, pageSize, filter, desc);
        #endregion

        public TEntity GetById(TKey id) => GetSingle(f => f.Id.Equals(id));
        public List<TEntity> GetById(HashSet<TKey> ids) => GetList(f => ids.Contains(f.Id));

        public TEntity AddOrUpdate(TEntity entity)
        {
            if (default(TKey).Equals(entity.Id))
                return Add(entity);
            return Update(entity);
        }
        public List<TEntity> AddOrUpdate(List<TEntity> entities)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 15, 0)))
            {
                try
                {
                    Update(entities.Where(x => !x.Id.Equals(default(TKey))).ToList());
                    Add(entities.Where(x => x.Id.Equals(default(TKey))).ToList());
                    scope.Complete();
                    return entities;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }
        public void DeleteById(TKey id) => Delete(GetById(id));
        public void DeleteById(HashSet<TKey> ids) => Delete(GetById(ids));

        public abstract PagedResult<TEntity> GetPagedList(int page = 1, int pageSize = 20, Expression<Func<TEntity, bool>> filter = null, bool desc = false);
    }
}
