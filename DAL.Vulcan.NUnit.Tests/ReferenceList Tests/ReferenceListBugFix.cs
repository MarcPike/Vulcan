using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson.Serialization.Attributes;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.ReferenceList_Tests
{

    public class TestGroup : BaseDocument
    {
        public ReferenceList<TestObject, TestObjectRef> TestObjectList = new ReferenceList<TestObject, TestObjectRef>();
    }
    public class TestObject : BaseDocument
    {
        public string Name { get; set; }

        public TestObjectRef AsTestObjectRef()
        {
            return new TestObjectRef(this);
        }
    }

    [BsonIgnoreExtraElements]
    public class TestObjectRef : ReferenceObject<TestObject>
    {
        public string Name { get; set; }
        public TestObjectRef(TestObject testObject) : base(testObject)
        {
            
        }

        public TestObject AsTestObject()
        {
            return ToBaseDocument();
        }
    }

    [TestFixture()]
    public class ReferenceListBugFix
    {

        private TestGroup TestGroup;
        private ReferenceList<TestObject, TestObjectRef> TestList = new ReferenceList<TestObject, TestObjectRef>();

        [SetUp]
        public void SetUp()
        {
            TearDown();

            TestGroup = new TestGroup();

            var testObject = new TestObject() {Name = "Marc Pike"};   
            testObject.SaveToDatabase();
            TestGroup.TestObjectList.AddReferenceObject(testObject.AsTestObjectRef());

            testObject = new TestObject() {Name = "TestPerson"};
            testObject.SaveToDatabase();
            TestGroup.TestObjectList.AddReferenceObject(testObject.AsTestObjectRef());
            
            TestGroup.SaveToDatabase();
        }

        [Test]
        public void TestResynch()
        {
            var rep = new RepositoryBase<TestObject>();
            var myself = rep.AsQueryable().Single(x => x.Name == "Marc Pike");
            myself.Name = "Mr. Marc Pike";
            myself.SaveToDatabase();

            var originalValue = TestGroup.TestObjectList.SingleOrDefault(x => x.Name == "Marc Pike");
            Assert.IsNotNull(originalValue);

            for (int i = 0; i < TestGroup.TestObjectList.Count -1; i++)
            {
                TestGroup.TestObjectList[i] = TestGroup.TestObjectList[i].ToBaseDocument().AsTestObjectRef();
            }

            var newValue = TestGroup.TestObjectList.SingleOrDefault(x => x.Name == "Mr. Marc Pike");
            Assert.IsNotNull(newValue);

        }

        [TearDown]
        public void TearDown()
        {
            var context = new VulcanContext();
            context.Database.DropCollection("TestGroup");
            context.Database.DropCollection("TestObject");

        }
    }
}
