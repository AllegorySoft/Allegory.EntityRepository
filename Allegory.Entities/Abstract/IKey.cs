using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allegory.Entities.Abstract
{
    public interface IKey<TKey> : IEntity where TKey : IComparable, IFormattable, IConvertible, IComparable<TKey>, IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
}
