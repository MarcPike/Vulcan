using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Import.Tests
{
    public class TestClass : BaseDocument
    {
        public static MongoRawQueryHelper<TestClass> Helper = new MongoRawQueryHelper<TestClass>();
        public string Message { get; set; } = "We are here!";
    }
}
