using System.Data.Entity;
using System.Linq;
using Allegory.Entities.Abstract;
using Allegory.EntityRepository.Abstract;

namespace Allegory.EntityFrameworkRepository.Concrete
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
