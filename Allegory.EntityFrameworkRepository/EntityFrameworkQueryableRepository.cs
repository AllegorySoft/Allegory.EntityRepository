using Allegory.Entities.Abstract;
using Allegory.EntityRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allegory.EntityFrameworkRepository
{
    public class EntityFrameworkQueryableRepository<T> : IQueryableRepository<T> where T : class, IEntity, new()
    {
        DbContext _context;
        DbSet<T> _entities;
        public EntityFrameworkQueryableRepository(DbContext context)
        {
            _context = context;
        }
        public IQueryable<T> Table => Entities;
        protected virtual IDbSet<T> Entities
        {
            get
            {
                return _entities ?? (_entities = _context.Set<T>());
            }
        }
    }
}
