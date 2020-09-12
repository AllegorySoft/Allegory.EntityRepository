using Allegory.EntityRepository.Tests.EntityFramework.Configuration.Mappings;
using Allegory.EntityRepository.Tests.Setup.Entities;
using System.Data.Entity;

namespace Allegory.EntityRepository.Tests.EntityFramework.Configuration
{
    public class TestsContext : DbContext
    {
        public TestsContext()
        {
            Database.SetInitializer<TestsContext>(null);
        }
        public DbSet<Table1> Table1s { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Table1Map());
        }
    }
}
