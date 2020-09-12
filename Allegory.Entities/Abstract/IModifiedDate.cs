using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allegory.Entities.Abstract
{
    public interface IModifiedDate
    {
        DateTime? ModifiedDate { get; set; }
    }
}
