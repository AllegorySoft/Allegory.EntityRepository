using Allegory.EntityRepository.Tests.Setup.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Allegory.EntityRepository.Tests.EntityFramework.Configuration.Mappings
{
    public class Table1Map : EntityTypeConfiguration<Table1>
    {
        public Table1Map()
        {
            ToTable(@"Table1", "dbo");
            HasKey(x => x.Id);
        }
    }
}
