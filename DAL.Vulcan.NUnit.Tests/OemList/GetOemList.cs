using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.OemList
{
    [TestFixture]
    public class GetOemList
    {
        [Test]
        public void GetList()
        {
            var oemList = new RepositoryBase<OemType>().AsQueryable().OrderBy(x => x.Name).Select(x=>x.Name).ToList();
            foreach (var oemType in oemList)
            {
                Console.WriteLine(oemType);
            }
        }
    }
}
