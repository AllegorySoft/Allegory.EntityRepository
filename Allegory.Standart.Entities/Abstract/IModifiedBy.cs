using System;

namespace Allegory.Standart.Entities.Abstract
{
    public interface IModifiedBy<TKey> : IKey<TKey> where TKey : IEquatable<TKey>
    {
        TKey ModifiedBy { get; set; }
    }
}
