using Allegory.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allegory.EntityRepository.Tests.Setup.Entities
{
    [Serializable]
    public class UserAuth : IUserIdentity
    {
        public int UserRef { get; set; }

        public string Name { get; set; }

        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}
