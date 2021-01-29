using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Rename_Field_Tests
{
    [TestFixture]
    public class LoadInitialTestData
    {
        [Test]
        public void InitializeTestData()
        {
            var context = new VulcanContext();
            context.Database.DropCollection("RenameTest");

            var testData = new RenameTest()
            {
                Version = new DocVersion() { VersionId = "1.0",Comments = "Initial loading of Data", ExecutedOn = DateTime.Now},
                Items = new List<RenameTestChildCollection>()
                {
                    new RenameTestChildCollection() {FrstName = "Ken", LstName = "Holtgrewe"},
                    new RenameTestChildCollection() {FrstName = "Marc", LstName = "Pike"},
                }
            };
            testData.SaveToDatabase();
        }


        public class RenameTest : BaseDocument
        {
            public List<RenameTestChildCollection> Items { get; set; } = new List<RenameTestChildCollection>();

        }

        //Original Class
        public class RenameTestChildCollection : BaseDocument
        {
            public string FrstName { get; set; }
            public string LstName { get; set; }
        }


        // Use to create test data first

    }
}
