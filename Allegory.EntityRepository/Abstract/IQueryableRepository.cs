using System.Linq;
using Allegory.Entities.Abstract;

namespace Allegory.EntityRepository.Abstract
{
    public interface IQueryableRepository<T> where T : class, IEntity, new()
    {
        IQueryable<T> Table { get; }
    }
}
