using Allegory.Entities.Abstract;
using Allegory.Entities.Concrete;
using Allegory.EntityRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;

namespace Allegory.EntityFrameworkRepository
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
                return Allegory.EntityRepository.EntityRepositoryBase.GetPaged(query, o => o.Id, page, pageSize, filter, desc);
            }
        }
    }
}
