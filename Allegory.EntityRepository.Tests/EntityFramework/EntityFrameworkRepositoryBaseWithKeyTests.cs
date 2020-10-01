using Allegory.EfRepositoryBase;
using Allegory.EntityRepository.Tests.EntityFramework.Configuration;
using Allegory.EntityRepository.Tests.Setup.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allegory.EntityRepository.Tests.EntityFramework
{
    [TestClass]
    public class EntityFrameworkRepositoryBaseWithKeyTests
    {
        /// <summary>
        /// !!!UnitTest project getting error on VS19 but in application works fine
        /// </summary>
        protected TestContext TestContext { get; set; }
        private static IEntityRepository<Table1, int> EntityRepository { get; set; }

        [ClassInitialize]
        public static void TestInitialize(TestContext testContexts)
        {
            EntityRepository = new EntityFrameworkRepositoryBaseWithKey<Table1, int, TestsContext>();
        }

        [TestMethod]
        public void GetById()
        {
            Table1 record = new Table1
            {
                CustomField1 = "getRecord",
                CustomField2 = 12,
            };

            EntityRepository.Add(record);
            Table1 result = EntityRepository.GetById(record.Id);

            Assert.AreNotSame(record, result);
            Assert.AreEqual(record.CustomField1, result.CustomField1);
        }

        [TestMethod]
        public void AddOrUpdate()
        {
            Table1 record = new Table1
            {
                CustomField1 = "data1"
            };

            EntityRepository.AddOrUpdate(record);
            var updatedRecord = EntityRepository.GetById(record.Id);
            updatedRecord.CustomField2 = 100;
            EntityRepository.Update(updatedRecord);

            Assert.AreNotEqual(0, record.Id);
            Assert.AreNotSame(record, updatedRecord);
            Assert.IsNull(record.ModifiedDate);
            Assert.IsNotNull(updatedRecord.ModifiedDate);
        }

        [TestMethod]
        public void DeleteById()
        {
            Table1 record = new Table1
            {
                CustomField1 = "deletedRecord"
            };

            var addedRecord = EntityRepository.Add(record);
            EntityRepository.DeleteById(record.Id);
            Table1 getById = EntityRepository.Get(f => f.Id == record.Id);

            Assert.IsNull(getById);
        }

        [TestMethod,Description("Default paged list by id")]
        public void GetPagedList()
        {
            for (int i = 1; i <= 6; i++)
                AddOrUpdate();

            var pagedList = EntityRepository.GetPagedList(pageSize: 5);

            Assert.AreEqual(5, pagedList.Results.Count);
            Assert.IsTrue(pagedList.PageCount > 1);
        }
    }
}
