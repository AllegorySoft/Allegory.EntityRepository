using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Allegory.Entities.Abstract;
using Allegory.Entities.Concrete;

namespace Allegory.EntityRepository.Abstract
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        T Get(Expression<Func<T, bool>> filter);
        T GetSingle(Expression<Func<T, bool>> filter);
        List<T> GetList(Expression<Func<T, bool>> filter = null);

        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);

        List<T> Add(List<T> entities);
        List<T> Update(List<T> entities);
        void Delete(List<T> entities);

        PagedResult<T> GetPagedList<TKey>(Expression<Func<T, TKey>> order, int page = 1, int pageSize = 20
                                        , Expression<Func<T, bool>> filter = null, bool desc = false);

    }
}
