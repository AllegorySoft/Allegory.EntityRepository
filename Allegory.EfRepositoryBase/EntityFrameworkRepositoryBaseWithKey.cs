using Allegory.Entities.Abstract;
using Allegory.Entities.Concrete;
using Allegory.EntityRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace Allegory.EfRepositoryBase
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
        public override TEntity GetById(TKey Id)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(x => x.Id.Equals(Id));
            }
        }
        public override List<TEntity> GetById(List<TKey> Ids)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().Where(x => Ids.Contains(x.Id)).ToList();
            }
        }
        public override void DeleteById(TKey Id)
        {
            using (var context = new TContext())
            {
                var entry = context.Entry(GetById(Id));
                entry.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
        public override PagedResult<TEntity> GetPagedList(int page = 1, int pageSize = 20, Expression<Func<TEntity, bool>> filter = null, bool desc = false)
        {
            using (var context = new TContext())
            {
                var query = (IQueryable<TEntity>)context.Set<TEntity>();
                if (filter != null)
                    query = query.Where(filter);
                if (desc)
                    query = query.OrderByDescending(o => o.Id);
                else
                    query = query.OrderBy(o => o.Id);
                return Allegory.EntityRepository.EntityRepositoryBase.GetPaged(query, page, pageSize);
            }
        }
    }
}
