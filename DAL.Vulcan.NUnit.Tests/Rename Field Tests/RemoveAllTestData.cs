using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Rename_Field_Tests
{
    [TestFixture]
    public class RemoveAllTestData
    {
        [Test]
        public void ClearData()
        {
            var context = new VulcanContext();
            context.Database.DropCollection("RenameTest");

        }
    }
}
