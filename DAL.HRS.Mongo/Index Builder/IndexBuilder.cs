using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Discipline;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Performance;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.Mongo.Indexes
{
    [TestFixture()]
    public class IndexBuilder
    {
        private HrsContext _context = new HrsContext();
        private MongoClient _client;
        private IMongoDatabase _db;
        //private CollectionNames _collectionNames = new CollectionNames();

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
            _context = new HrsContext();
        }

        public IndexBuilder()
        {
            // SalesPersons
            _client = _context.MongoClient;
            _db = _client.GetDatabase(_context.DatabaseName);
            
        }


        [Test]
        public void BuildDisciplineIndexes()
        {
            var collection = _db.GetCollection<Discipline>("Discipline");
            var builder = Builders<Discipline>.IndexKeys;

            var indexModel = new CreateIndexModel<Discipline>(builder
                .Ascending(x => x.Employee.Id));
            collection.Indexes.CreateOne(indexModel);
        }

        [Test]
        public void BuildEmployeeIndexes()
        {
            var collection = _db.GetCollection<Employee>("Employee");
            var builder = Builders<Employee>.IndexKeys;

            var indexModel = new CreateIndexModel<Employee>(builder
                .Ascending(x => x.PayrollId));
            collection.Indexes.CreateOne(indexModel);
        }

        [Test]
        public void BuildPropertyTypeIndexes()
        {
            var collection = _db.GetCollection<PropertyType>("PropertyType");
            var builder = Builders<PropertyType>.IndexKeys;

            var indexModel = new CreateIndexModel<PropertyType>(builder
                .Ascending(x => x.Type));
            var result = collection.Indexes.CreateOne(indexModel);
        }

        [Test]
        public void BuildPropertyValueIndexes()
        {
            var collection = _db.GetCollection<PropertyValue>("PropertyValue");
            var builder = Builders<PropertyValue>.IndexKeys;

            var indexModel = new CreateIndexModel<PropertyValue>(builder
                .Ascending(x => x.Type));
            collection.Indexes.CreateOne(indexModel);
        }



        [Test]
        public void Execute()
        {
            BuildDisciplineIndexes();
            BuildEmployeeIndexes();
            BuildPropertyTypeIndexes();
            BuildPropertyValueIndexes();
        }


    }
}
