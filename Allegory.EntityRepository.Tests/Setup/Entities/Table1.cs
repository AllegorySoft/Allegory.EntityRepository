using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allegory.EntityRepository.Tests.Setup.Entities
{
    public class Table1 : EntityBase
    {
        public string CustomField1 { get; set; }
        public Nullable<double> CustomField2 { get; set; }
    }
}
