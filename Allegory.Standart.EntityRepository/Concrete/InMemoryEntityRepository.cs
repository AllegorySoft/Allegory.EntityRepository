using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Allegory.Standart.Entities.Abstract;
using Allegory.Standart.Entities.Concrete;
using Allegory.Standart.EntityRepository.Abstract;

namespace Allegory.Standart.EntityRepository.Concrete
{
    public class InMemoryEntityRepository<TEntity> : EntityRepositoryBase<TEntity> where TEntity : class, IEntity, new()
    {
        private static readonly IList<TEntity> _entities = new List<TEntity>();
        public override TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            Console.WriteLine($"{MethodBase.GetCurrentMethod()} : {filter}");
            return filter == null ?_entities.FirstOrDefault(): _entities.FirstOrDefault(filter?.Compile());
        }
        public override TEntity GetSingle(Expression<Func<TEntity, bool>> filter)
        {
            Console.WriteLine($"{MethodBase.GetCurrentMethod()} : {filter}");
            return filter == null ? _entities.Single() : _entities.Single(filter?.Compile());
        }
        public override List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            Console.WriteLine($"{MethodBase.GetCurrentMethod()} : {filter}");
            return filter == null ? _entities.ToList() : _entities.Where(filter.Compile()).ToList();
        }

        protected override TEntity AddOrm(TEntity entity)
        {
            Console.WriteLine($"{MethodBase.GetCurrentMethod()} : {entity}");
            _entities.Add(entity);
            return entity;
        }
        protected override TEntity UpdateOrm(TEntity entity)
        {
            Console.WriteLine($"{MethodBase.GetCurrentMethod()} : {entity}");
            return entity;
        }
        public override void Delete(TEntity entity)
        {
            Console.WriteLine($"{MethodBase.GetCurrentMethod()} : {entity}");
            _entities.Remove(entity);
        }

        protected override List<TEntity> AddOrm(List<TEntity> entities)
        {
            Console.WriteLine($"{MethodBase.GetCurrentMethod()} : {entities}");
            entities.ForEach(entity => _entities.Add(entity));
            return entities;
        }
        protected override List<TEntity> UpdateOrm(List<TEntity> entities)
        {
            Console.WriteLine($"{MethodBase.GetCurrentMethod()} : {entities}");
            return entities;
        }
        protected override void DeleteOrm(List<TEntity> entities)
        {
            Console.WriteLine($"{MethodBase.GetCurrentMethod()} : {entities}");
            entities.ForEach(entity => _entities.Remove(entity));
        }

        public override PagedResult<TEntity> GetPagedList<TKey>(Expression<Func<TEntity, TKey>> order, int page = 1, int pageSize = 20, Expression<Func<TEntity, bool>> filter = null, bool desc = false)
        {
            Console.WriteLine($"{MethodBase.GetCurrentMethod()} : {filter}");
            return EntityRepositoryBase.GetPaged(_entities.AsQueryable(), order, page, pageSize, filter, desc);
        }
    }
}
