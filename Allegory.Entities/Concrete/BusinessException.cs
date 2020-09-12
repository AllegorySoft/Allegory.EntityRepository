using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allegory.Entities.Concrete
{
    public class BusinessException : Exception
    {
        public BusinessException(string Message) : base(Message)
        {

        }
    }
}
