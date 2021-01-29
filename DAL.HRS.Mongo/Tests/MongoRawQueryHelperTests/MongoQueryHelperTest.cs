using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.Test.MongoRawQueryHelperTests
{
    [TestFixture()]
    public class MongoQueryHelperTest
    {
        private MongoRawQueryHelper<Employee> _queryHelper;


        public struct MyResultValueObject
        {

            public ObjectId Id;
            public string FirstName;
            public string LastName;
            public DateTime BirthDay;

            public MyResultValueObject(ObjectId id, string firstName, string lastName, byte[] birthDayEncrypted)
            {
                var encryption = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

                Id = id;
                FirstName = firstName;
                LastName = lastName;
                BirthDay = encryption.Decrypt<DateTime>(birthDayEncrypted);
            }
        }

        public class MyResultClass
        {
            private DAL.Vulcan.Mongo.Base.Encryption.Encryption _encryption = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;
            public ObjectId Id;
            public string FirstName;
            public string LastName;
            public DateTime BirthDay;

            public MyResultClass(ObjectId id, string firstName, string lastName, byte[] birthDayEncrypted)
            {

                Id = id;
                FirstName = firstName;
                LastName = lastName;
                BirthDay = _encryption.Decrypt<DateTime>(birthDayEncrypted);
            }
        }

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
            _queryHelper = new MongoRawQueryHelper<Employee>();
        }

        [Test]
        public void SyntaxTests()
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var emps = Employee.Helper.
                Find(emp => emp.LastName == "Smith").
                Project(emp => new
                {
                    emp.Id,
                    emp.FirstName,
                    emp.LastName
                }).
                SortByDescending(x => x.FirstName).ToList();


            sw.Stop();

            Console.WriteLine($"Elapsed: {sw.Elapsed}");

            foreach (var emp in emps)
            {
                Console.WriteLine($"{emp.Id} {emp.LastName}, {emp.FirstName}");
            }

        }

        [Test]
        public void FindWithProjectionAnonymousTypeUpdatedWithMongoSort()
        {
            var sw = new Stopwatch();
            sw.Start();
            var encryption = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

            var results = Employee.Helper
                .Find(x => x.Location.Office == "Telge" &&
                           (x.TerminationDate == null || x.TerminationDate >= DateTime.Now))
                .Project(x => new {x.Id, x.FirstName, x.LastName, x.Birthday})
                .SortBy(x => x.FirstName)
                  .ThenBy(x => x.LastName).ToList();
            sw.Stop();
            Console.WriteLine($"Query Time: {sw.Elapsed} Count: {results.Count}");
            foreach (var result in results)
            {
                Console.WriteLine($"Id: {result.Id} Name: {result.FirstName} {result.LastName} DOB: {encryption.Decrypt<DateTime>(result.Birthday).ToShortDateString()}");
            }
        }

        /*
         * Love this Syntax however it is just slower than other means
         */

        [Test]
        public void BestSyntaxSlowest() //  00:00:00.8275465
        {
            var sw = new Stopwatch();
            sw.Start();
            var encryption = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

            var results = Employee.Helper
                .Find(x => x.Location.Office == "Telge" &&
                           (x.TerminationDate == null || x.TerminationDate >= DateTime.Now))
                .Project(x => new { x.Id, x.FirstName, x.LastName, x.Birthday  }).SortBy(x=>x.FirstName).ThenBy(x=>x.LastName).ToList();
            sw.Stop();
            Console.WriteLine($"Query Time: {sw.Elapsed} Count: {results.Count}");
            foreach (var result in results)
            {
                Console.WriteLine($"Id: {result.Id} Name: {result.FirstName} {result.LastName} DOB: {encryption.Decrypt<DateTime>(result.Birthday).ToShortDateString()}");
            }
        }
        /*
         * Mongo sorting is slow, Linq ToList().OrderBy() is fast, but this is still slower
         */
        [Test]
        public void BestSyntaxWithLinqSortStillSlower() //  00:00:00.8057551
        {
            var sw = new Stopwatch();
            sw.Start();
            var encryption = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

            var results = Employee.Helper
                .Find(x => x.Location.Office == "Telge" &&
                           (x.TerminationDate == null || x.TerminationDate >= DateTime.Now))
                .Project(x => new { x.Id, x.FirstName, x.LastName, x.Birthday }).ToList().OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            sw.Stop();
            Console.WriteLine($"Query Time: {sw.Elapsed} Count: {results.Count}");
            foreach (var result in results)
            {
                Console.WriteLine($"Id: {result.Id} Name: {result.FirstName} {result.LastName} DOB: {encryption.Decrypt<DateTime>(result.Birthday).ToShortDateString()}");
            }
        }

        /*
         * Using a FilterBuilder, ProjectionBuilder and SortBuilder is much faster, primarily using the Mongo functions
         * however again the Mongo sort is not as fast
         */

        [Test]
        public void FindWithMongoFunctionsAndMongoSortSecondFastest() // 00:00:00.7509647
        {
            var sw = new Stopwatch();
            sw.Start();
            var encryption = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

            var filterBuilder = Employee.Helper.FilterBuilder;
            var projectBuilder = Employee.Helper.ProjectionBuilder;
            var sortBuilder = Employee.Helper.SortBuilder;
            var results = Employee.Helper
                .FindWithProjection(filterBuilder.Eq(x => x.Location.Office, "Telge") &
                                    (filterBuilder.Eq(x => x.TerminationDate, null) |
                                     filterBuilder.Gte(x => x.TerminationDate, DateTime.Now)),
                    projectBuilder.Expression(x => new {x.Id, x.FirstName, x.LastName, x.Birthday}),
                    sortBuilder.Ascending(x => x.FirstName).Ascending(x => x.LastName));
            sw.Stop();
            Console.WriteLine($"Query Time: {sw.Elapsed} Count: {results.Count}");
            foreach (var result in results)
            {
                Console.WriteLine($"Id: {result.Id} Name: {result.FirstName} {result.LastName} DOB: {encryption.Decrypt<DateTime>(result.Birthday).ToShortDateString()}");
            }
        }

        /*
         * This is the 2nd best syntax and best performance
         */

        [Test]
        public void FindWithMongoFunctionsAndLinqSortFastest() // 00:00:00.7244804
        {
            var sw = new Stopwatch();
            sw.Start();
            var encryption = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

            var fb = Employee.Helper.FilterBuilder;
            var pb = Employee.Helper.ProjectionBuilder;
            var results = Employee.Helper
                .FindWithProjection(fb.Eq(x => x.Location.Office, "Telge") &
                                    (fb.Eq(x => x.TerminationDate, null) |
                                     fb.Gte(x => x.TerminationDate, DateTime.Now)),
                    pb.Expression(x => new {x.Id, x.FirstName, x.LastName, x.Birthday}))
                .OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            sw.Stop();
            Console.WriteLine($"Query Time: {sw.Elapsed} Count: {results.Count}");
            foreach (var result in results)
            {
                Console.WriteLine($"Id: {result.Id} Name: {result.FirstName} {result.LastName} DOB: {encryption.Decrypt<DateTime>(result.Birthday).ToShortDateString()}");
            }
        }

        /*
         * Original code, which works well, but is ugly
         */

        [Test]
        public void FindWithProjectionAnonymousType() //  00:00:00.7399469
        {
            var sw = new Stopwatch();
            sw.Start();
            var encryption = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

            var filter = _queryHelper.FilterBuilder.Eq(x => x.Location.Office, "Telge") &
                         (_queryHelper.FilterBuilder.Eq(x => x.TerminationDate, null) | _queryHelper.FilterBuilder.Gte(x => x.TerminationDate, DateTime.Now));
            var project = _queryHelper.ProjectionBuilder.Expression(x =>
                new {x.Id, x.FirstName, x.LastName, x.Birthday});
            //var sort = _queryHelper.SortBuilder.Ascending(x => x.FirstName).Ascending(x => x.LastName);

            var results = _queryHelper.FindWithProjection(filter, project).OrderBy(x=>x.FirstName).ThenBy(x=>x.LastName).ToList();//, sort);
            sw.Stop();
            Console.WriteLine($"Query Time: {sw.Elapsed} Count: {results.Count}");
            foreach (var result in results)
            {
                Console.WriteLine($"Id: {result.Id} Name: {result.FirstName} {result.LastName} DOB: {encryption.Decrypt<DateTime>(result.Birthday).ToShortDateString()}");
            }
        }

        [Test]
        public void FindWithProjectionValueObject()
        {
            var sw = new Stopwatch();
            sw.Start();
            var encryption = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

            var filter = _queryHelper.FilterBuilder.Eq(x => x.Location.Office, "Telge") &
                         (_queryHelper.FilterBuilder.Ne(x => x.TerminationDate, null) | _queryHelper.FilterBuilder.Gte(x => x.TerminationDate, DateTime.Now));
            var project = _queryHelper.ProjectionBuilder.Expression(x =>
                new MyResultValueObject(x.Id, x.FirstName, x.LastName, x.Birthday));
            var sort = _queryHelper.SortBuilder.Ascending(x => x.FirstName).Ascending(x => x.LastName);


            var results = _queryHelper.FindWithProjection(filter, project, sort);
            sw.Stop();
            Console.WriteLine($"Query Time: {sw.Elapsed} Count: {results.Count}");
            foreach (var result in results)
            {
                Console.WriteLine($"Id: {result.Id} Name: {result.FirstName} {result.LastName} DOB: {result.BirthDay.ToShortDateString()}");
            }
        }

        [Test]
        public void FindWithProjectionClass()
        {
            var sw = new Stopwatch();
            sw.Start();
            var encryption = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

            var filter = _queryHelper.FilterBuilder.Eq(x => x.Location.Office, "Telge") & 
                         (_queryHelper.FilterBuilder.Eq(x=>x.TerminationDate,null) | _queryHelper.FilterBuilder.Gte(x=>x.TerminationDate,DateTime.Now));
            var project = _queryHelper.ProjectionBuilder.Expression(x =>
                new MyResultClass(x.Id, x.FirstName, x.LastName, x.Birthday));
            var sort = _queryHelper.SortBuilder.Ascending(x => x.FirstName).Ascending(x => x.LastName);


            var results = _queryHelper.FindWithProjection(filter, project, sort);
            sw.Stop();
            Console.WriteLine($"Query Time: {sw.Elapsed} Count: {results.Count}");
            foreach (var result in results)
            {
                Console.WriteLine($"Id: {result.Id} Name: {result.FirstName} {result.LastName} DOB: {result.BirthDay.ToShortDateString()}");
            }
        }

    }
}
