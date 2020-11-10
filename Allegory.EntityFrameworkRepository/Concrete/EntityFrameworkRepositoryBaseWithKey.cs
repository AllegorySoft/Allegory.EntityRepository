using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Allegory.Entities.Abstract;
using Allegory.Entities.Concrete;
using Allegory.EntityRepository.Abstract;

namespace Allegory.EntityFrameworkRepository.Concrete
{
    public class EntityFrameworkRepositoryBaseWithKey<TEntity, TKey, TContext> : EntityRepositoryBaseWithKey<TEntity, TKey>, IEntityRepository<TEntity, TKey>
        where TEntity : class, IKey<TKey>, new()
        where TContext : DbContext, new()
        where TKey : IComparable, IFormattable, IConvertible, IComparable<TKey>, IEquatable<TKey>
    {
        public EntityFrameworkRepositoryBaseWithKey()
        {
            EntityRepositoryBase = new EntityFrameworkRepositoryBase<TEntity, TContext>();
        }
      
        public override PagedResult<TEntity> GetPagedList(int page = 1, int pageSize = 20, Expression<Func<TEntity, bool>> filter = null, bool desc = false)
        {
            using (var context = new TContext())
            {
                var query = (IQueryable<TEntity>)context.Set<TEntity>().AsNoTracking();
                return Allegory.EntityRepository.Abstract.EntityRepositoryBase.GetPaged(query, o => o.Id, page, pageSize, filter, desc);
            }
        }
    }
}
