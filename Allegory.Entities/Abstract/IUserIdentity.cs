using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Allegory.Entities.Abstract
{
    public interface IUserIdentity : IIdentity
    {
        int UserRef { get; set; }
    }
}
