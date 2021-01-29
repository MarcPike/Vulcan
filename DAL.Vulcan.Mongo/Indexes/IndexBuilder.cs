using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.Indexes
{
    public class IndexBuilder
    {
        private VulcanContext _vulcanContext = new VulcanContext();
        private MongoClient _client;
        private IMongoDatabase _db;
        //private CollectionNames _collectionNames = new CollectionNames();


        public IndexBuilder()
        {
            // SalesPersons
            _vulcanContext = new VulcanContext();
            _client = _vulcanContext.MongoClient;
            _db = _client.GetDatabase(_vulcanContext.DatabaseName);
            
        }

        //public void SalesPersons()
        //{
        //    var collectionName = _collectionNames.GetCollectionNameFor(typeof(SalesPerson));
        //    var collection = _db.GetCollection<SalesPerson>(collectionName);
        //    collection.Indexes.CreateOne(Builders<SalesPerson>.IndexKeys.Ascending(x => x.NetworkId));
        //}

        //public void UserSettings()
        //{
        //    var collectionName = _collectionNames.GetCollectionNameFor(typeof(UserSettings));
        //    var collection = _db.GetCollection<UserSettings>(collectionName);
        //    collection.Indexes.CreateOne(Builders<UserSettings>.IndexKeys.Ascending(x => x.SearchKey));
        //}

        //public void CustomerPriceList()
        //{
        //    var collectionName = _collectionNames.GetCollectionNameFor(typeof(CustomerContractPriceList));
        //    var collection = _db.GetCollection<CustomerContractPriceList>(collectionName);
        //    collection.Indexes.CreateOne(Builders<CustomerContractPriceList>.IndexKeys.Ascending(x => x.MaterialPrices));
        //    collection.Indexes.CreateOne(Builders<CustomerContractPriceList>.IndexKeys.Ascending(x => x.BorePrices));

        //}


        public void BuildCompanyIndexes()
        {
            var collection = _db.GetCollection<Company>("Company");
            var builder = Builders<Company>.IndexKeys;

            var indexModel = new CreateIndexModel<Company>(builder
                .Ascending(x => x.SqlId)
                .Descending(x => x.Location.Branch));
            collection.Indexes.CreateOne(indexModel);

            indexModel = new CreateIndexModel<Company>(builder.
                Ascending(x => x.Name));
            collection.Indexes.CreateOne(indexModel);
        }

        public void BuildTeamIndexes()
        {
            var collection = _db.GetCollection<Team>("Team");
            var builder = Builders<Team>.IndexKeys;

            var indexModel = new CreateIndexModel<Team>(builder
                .Ascending(x => x.Name));
            collection.Indexes.CreateOne(indexModel);

            indexModel = new CreateIndexModel<Team>(builder
                .Combine(new BsonDocument("Links.Id", 1), new BsonDocument("Links.AssemblyQualifiedName", 1)));
            collection.Indexes.CreateOne(indexModel);

            indexModel = new CreateIndexModel<Team>(builder
                .Combine(new BsonDocument("Links.TypeFullName", 1)));
            collection.Indexes.CreateOne(indexModel);

        }

        public void BuildCrmUserIndexes()
        {

            var collection = _db.GetCollection<CrmUser>("CrmUser");
            var builder = Builders<CrmUser>.IndexKeys;

            var indexModel = new CreateIndexModel<CrmUser>(builder
                .Combine(new BsonDocument("Links.Id", 1), new BsonDocument("Links.AssemblyQualifiedName", 1)));
            collection.Indexes.CreateOne(indexModel);

            indexModel = new CreateIndexModel<CrmUser>(builder
                .Combine(new BsonDocument("Links.TypeFullName", 1)));
            collection.Indexes.CreateOne(indexModel);

            indexModel = new CreateIndexModel<CrmUser>(builder
                .Combine(new BsonDocument("User.Id", 1)));
            collection.Indexes.CreateOne(indexModel);
        }

        public void BuildActionIndexes()
        {
            var collection = _db.GetCollection<Action>("Action");
            var builder = Builders<Action>.IndexKeys;

            var indexModel = new CreateIndexModel<Action>(builder
                .Combine(new BsonDocument("Links.Id", 1), new BsonDocument("Links.AssemblyQualifiedName", 1)));
            collection.Indexes.CreateOne(indexModel);

            indexModel = new CreateIndexModel<Action>(builder
                .Combine(new BsonDocument("Links.TypeFullName", 1)));
            collection.Indexes.CreateOne(indexModel);
        }

        public void BuildContactIndexes()
        {
            var collection = _db.GetCollection<Contact>("Contact");
            var builder = Builders<Contact>.IndexKeys;

            var indexModel = new CreateIndexModel<Contact>(builder
                .Combine(new BsonDocument("Links.Id", 1), new BsonDocument("Links.AssemblyQualifiedName", 1)));
            collection.Indexes.CreateOne(indexModel);

            indexModel = new CreateIndexModel<Contact>(builder
                .Combine(new BsonDocument("Links.TypeFullName", 1)));
            collection.Indexes.CreateOne(indexModel);
        }

        public void BuildNotificationIndexes()
        {
            var collection = _db.GetCollection<Notification>("Notification");
            var builder = Builders<Notification>.IndexKeys;

            var indexModel = new CreateIndexModel<Notification>(builder
                .Combine(new BsonDocument("Links.Id", 1), new BsonDocument("Links.AssemblyQualifiedName", 1)));
            collection.Indexes.CreateOne(indexModel);

            indexModel = new CreateIndexModel<Notification>(builder
                .Combine(new BsonDocument("Links.TypeFullName", 1)));
            collection.Indexes.CreateOne(indexModel);
        }

        public void BuildQuoteIndexes()
        {
            var collection = _db.GetCollection<CrmQuote>("CrmQuote");

            var builder = Builders<CrmQuote>.IndexKeys;

            var indexModel = new CreateIndexModel<CrmQuote>(builder
                .Ascending(x => x.Team.Id));
            collection.Indexes.CreateOne(indexModel);

            indexModel = new CreateIndexModel<CrmQuote>(builder
                .Ascending(x => x.SalesPerson.Id));
            collection.Indexes.CreateOne(indexModel);

            indexModel = new CreateIndexModel<CrmQuote>(builder
                .Ascending(x => x.Status));
            collection.Indexes.CreateOne(indexModel);

            indexModel = new CreateIndexModel<CrmQuote>(builder
                .Ascending(x => x.ReportDate));
            collection.Indexes.CreateOne(indexModel);

            indexModel = new CreateIndexModel<CrmQuote>(builder
                .Ascending(x => x.ReportDate).Ascending(x => x.SalesPerson.Id));
            collection.Indexes.CreateOne(indexModel);

            indexModel = new CreateIndexModel<CrmQuote>(builder
                .Ascending(x => x.ReportDate).Ascending(x => x.SalesPerson.Id).Ascending(x => x.Team.Id));
            collection.Indexes.CreateOne(indexModel);

        }

        public void BuildCrmUserTokenIndexes()
        {
            var collection = _db.GetCollection<CrmUserToken>("CrmUserToken");

            var builder = Builders<CrmUserToken>.IndexKeys;

            var indexModel = new CreateIndexModel<CrmUserToken>(builder
                .Ascending(x => x.User.Id));
            collection.Indexes.CreateOne(indexModel);
        }

        public void Execute()
        {
            BuildCompanyIndexes();
            BuildTeamIndexes();
            BuildCrmUserIndexes();
            BuildActionIndexes();
            BuildContactIndexes();
            BuildNotificationIndexes();
        }


    }
}
