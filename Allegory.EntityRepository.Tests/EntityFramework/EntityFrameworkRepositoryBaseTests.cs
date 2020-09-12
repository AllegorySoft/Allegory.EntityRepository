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
    public class EntityFrameworkRepositoryBaseTests
    {
        protected TestContext TestContext { get; set; }
        private static IEntityRepository<Table1> EntityRepository { get; set; }

        [ClassInitialize]
        public static void TestInitialize(TestContext testContexts)
        {
            EntityRepository = new EntityFrameworkRepositoryBase<Table1, TestsContext>();
        }

        [TestMethod, Description("When record added auto fill created date")]
        public void Add()
        {
            Table1 record = new Table1
            {
                CustomField1 = "data1",
                CustomField2 = 10.5,

                ModifiedDate = DateTime.Now
            };

            var addedRecord = EntityRepository.Add(record);

            Assert.AreSame(record, addedRecord);
            Assert.AreNotEqual(0, record.Id);
            Assert.IsNull(record.ModifiedDate);
            Assert.AreNotEqual(DateTime.MinValue, record.CreatedDate);
        }

        [TestMethod, Description("Get record by expression")]
        public void Get()
        {
            Table1 record = new Table1
            {
                CustomField1 = "getRecord",
                CustomField2 = 12,
            };

            EntityRepository.Add(record);
            Table1 result = EntityRepository.Get(f => f.Id == record.Id);

            Assert.AreNotSame(record, result);
            Assert.AreEqual(record.CustomField1, result.CustomField1);
        }

        [TestMethod, Description("When record updated auto fill modified date")]
        public void Update()
        {
            Table1 record = new Table1
            {
                CustomField1 = "addedRecord",

                ModifiedDate = DateTime.Now
            };

            EntityRepository.Add(record);
            Assert.IsNull(record.ModifiedDate);

            record.CustomField1 = "updatedRecord";
            EntityRepository.Update(record);

            Assert.IsNotNull(record.ModifiedDate);
        }

        [TestMethod]
        public void Delete()
        {
            Table1 record = new Table1
            {
                CustomField1 = "deletedRecord"
            };

            var addedRecord = EntityRepository.Add(record);
            EntityRepository.Delete(record);
            Table1 getById = EntityRepository.Get(f => f.Id == record.Id);

            Assert.IsNull(getById);
        }

        [TestMethod]
        public void GetList()
        {
            Add();

            var result = EntityRepository.GetList();

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetPagedList()
        {
            for (int i = 1; i <= 6; i++)
                Add();

            var pagedList = EntityRepository.GetPagedList(o => o.Id, pageSize: 5);

            Assert.AreEqual(5, pagedList.Results.Count);
            Assert.IsTrue(pagedList.PageCount > 1);
        }
    }
}
