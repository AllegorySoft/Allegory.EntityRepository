using System;

namespace Allegory.Standart.Entities.Abstract
{
    public interface ICreatedBy<TKey> : IKey<TKey> where TKey : IEquatable<TKey>
    {
         TKey CreatedBy { get; set; }
    }
}
