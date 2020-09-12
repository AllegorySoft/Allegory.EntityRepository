using Allegory.Entities.Abstract;
using Allegory.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Allegory.EntityRepository
{
    public interface IEntityRepository<T, TKey> : IEntityRepository<T>
        where T : class, IKey<TKey>, new()
        where TKey : IComparable, IFormattable, IConvertible, IComparable<TKey>, IEquatable<TKey>
    {
        T GetById(TKey Id);
        List<T> GetById(List<TKey> Ids);

        T AddOrUpdate(T entity);
        List<T> AddOrUpdate(List<T> entities);
        void DeleteById(TKey Id);
        void DeleteById(List<TKey> Ids);

        PagedResult<T> GetPagedList(int page = 1, int pageSize = 20, Expression<Func<T, bool>> filter = null, bool desc = false);
    }
}