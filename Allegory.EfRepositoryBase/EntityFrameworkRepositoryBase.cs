using Allegory.Entities.Abstract;
using Allegory.Entities.Concrete;
using Allegory.EntityRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.Migrations;

namespace Allegory.EfRepositoryBase
{
    public class EntityFrameworkRepositoryBase<TEntity, TContext> : EntityRepositoryBase<TEntity>, IEntityRepository<TEntity>
      where TEntity : class, IEntity, new()
      where TContext : DbContext, new()
    {
        public override TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().AsNoTracking().FirstOrDefault(filter);
            }
        }
        public override TEntity GetSingle(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().AsNoTracking().SingleOrDefault(filter);
            }
        }
        public override List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().AsNoTracking().ToList()
                    : context.Set<TEntity>().AsNoTracking().Where(filter).ToList();
            }
        }

        protected override TEntity AddOrm(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Added;
                context.SaveChanges();
                return entity;
            }
        }
        protected override TEntity UpdateOrm(TEntity entity)
        {
            using (var context = new TContext())
            {
                var entry = context.Entry(entity);
                entry.State = EntityState.Modified;

                if (IsAssignableFromICreatedDate)
                    entry.Property(x => ((ICreatedDate)x).CreatedDate).IsModified = false;
                if(IsAssignableFromICreatedBy)
                    entry.Property("CreatedBy").IsModified = false;

                context.SaveChanges();
                return entity;
            }
        }
        public override void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        protected override List<TEntity> AddOrm(List<TEntity> entities)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().AddRange(entities);
                context.SaveChanges();
                return entities;
            }
        }
        protected override List<TEntity> UpdateOrm(List<TEntity> entities)
        {
            using (var context = new TContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < entities.Count; i++)
                {
                    context.Entry(entities[i]).State = EntityState.Modified;
                }

                if (IsAssignableFromICreatedDate)
                    for (int i = 0; i < entities.Count; i++)
                        context.Entry(entities[i]).Property(y => ((ICreatedDate)y).CreatedDate).IsModified = false;
                if(IsAssignableFromICreatedBy)
                    for (int i = 0; i < entities.Count; i++)
                        context.Entry(entities[i]).Property("CreatedBy").IsModified = false;

                context.SaveChanges();
                return entities;
            }
        }
        protected override void DeleteOrm(List<TEntity> entities)
        {
            using (var context = new TContext())
            {
                context.Set<TEntity>().RemoveRange(entities);
                context.SaveChanges();
            }
        }

        public override PagedResult<TEntity> GetPagedList<TKey>(Expression<Func<TEntity, TKey>> order, int page = 1, int pageSize = 20, Expression<Func<TEntity, bool>> filter = null, bool desc = false)
        {
            using (var context = new TContext())
            {
                var query = (IQueryable<TEntity>)context.Set<TEntity>().AsNoTracking();
                return EntityRepositoryBase.GetPaged(query, order, page, pageSize, filter, desc);
            }
        }
    }
}
