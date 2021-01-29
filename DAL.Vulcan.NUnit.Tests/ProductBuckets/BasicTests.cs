using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.ProductBuckets
{
    [TestFixture]
    public class BasicTests
    {
        [Test]
        public void GetProductBucketHelper()
        {
            var bucketHelper = new ProductBucketHelper("INC");
            Console.WriteLine(ObjectDumper.Dump(bucketHelper));
        }
    }
}
