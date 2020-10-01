using Allegory.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allegory.EntityRepository.Tests.Setup.Entities
{
    public abstract class EntityBase : IKey<int>, ICreatedDate, IModifiedDate, IActive, ICreatedBy, IModifiedBy
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
