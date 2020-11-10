using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Allegory.Entities.Abstract;
using Allegory.Entities.Concrete;

namespace Allegory.EntityRepository.Abstract
{
    public interface IEntityRepository<T, TKey> : IEntityRepository<T>
        where T : class, IKey<TKey>, new()
        where TKey : IComparable, IFormattable, IConvertible, IComparable<TKey>, IEquatable<TKey>
    {
        T GetById(TKey id);
        List<T> GetById(HashSet<TKey> ids);

        T AddOrUpdate(T entity);
        List<T> AddOrUpdate(List<T> entities);
        void DeleteById(TKey id);
        void DeleteById(HashSet<TKey> ids);

        PagedResult<T> GetPagedList(int page = 1, int pageSize = 20, Expression<Func<T, bool>> filter = null, bool desc = false);
    }
}