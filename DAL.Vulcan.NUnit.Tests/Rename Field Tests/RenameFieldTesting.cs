using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.Rename_Field_Tests
{
    [TestFixture]
    public class RenameFieldTesting
    {

        [Test]
        public void Test()
        {
            var rep = new RepositoryBase<RenameTest>();
            var tests = rep.AsQueryable().ToList();
            Assert.IsTrue(tests.Any());
            foreach (var renameTest in tests)
            {
                Assert.IsTrue(renameTest.Items.All(x => x.FirstName != null && x.LastName != null));
                renameTest.SaveToDatabase();
            }

        }

        public class RenameTest : BaseDocument
        {
            public List<RenameTestChildCollection> Items { get; set; } = new List<RenameTestChildCollection>();

        }


        //Modified Class
        public class RenameTestChildCollection : BaseValueObject
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public override void BeginInit()
            {
            }

            public override void EndInit()
            {
                
                // Changed FrstName to FirstName
                object nameValue;
                if (ExtraElements.TryGetValue("FrstName", out nameValue))
                {
                    var firstName = (string)nameValue;

                    // remove the Name element so that it doesn't get persisted back to the database
                    ExtraElements.Remove("FrstName");
                    FirstName = firstName;
                }

                // Changed LstName to LastName
                if (ExtraElements.TryGetValue("LstName", out nameValue))
                {
                    var lastName = (string)nameValue;
                    // remove the Name element so that it doesn't get persisted back to the database
                    ExtraElements.Remove("LstName");
                    LastName = lastName;
                }
            }

        }


    }
}
