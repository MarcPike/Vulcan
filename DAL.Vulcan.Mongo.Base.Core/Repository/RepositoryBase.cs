using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.Context;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.EventWatcher;
using DAL.Vulcan.Mongo.Base.Core.Filters;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DAL.Vulcan.Mongo.Base.Core.Repository
{
    public class RepositoryBase<TBaseDocument> 
        where TBaseDocument: BaseDocument 
    {
        public ContextBase _context { get; set; }

        private string GetCollectionName()
        {
            return typeof(TBaseDocument).Name;

        }

        public void RenameAllFields(string oldName, string newName)
        {
            var renameCommand = Builders<TBaseDocument>.Update.Rename(oldName, newName);
            var filterDefinition = Builders<TBaseDocument>.Filter.Empty; // new BsonDocument()
            Collection.UpdateMany(filterDefinition, renameCommand);
        }

        public IMongoCollection<TBaseDocument> Collection => _context.Database.GetCollection<TBaseDocument>(GetCollectionName());

        public void RemoveAllFromCollection()
        {
            var collectionName = GetCollectionName();
            var database = _context.Database;

            var filter = new BsonDocument("name", collectionName);
            var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter }).ToList();
            if (collections.Any())
            {
                _context.Database.DropCollection(collectionName);
            }
            _context.Database.CreateCollection(collectionName);

        }

        public IMongoQueryable<TBaseDocument> AsQueryable()
        {
            return Collection.AsQueryable();
        }

        public IMongoQueryable<TBaseDocument> AsQueryable(AggregateOptions options)
        {
            return Collection.AsQueryable(options);
        }


        //public static Dictionary<ObjectId,TBaseDocument> IEnumerable<TBaseDocument>

        public IMongoCollection<TBaseDocument> AsMongoCollection()
        {
            return Collection;
        }

        public async Task<List<TBaseDocument>> FindAllAsync(BaseFilter<TBaseDocument> filter)
        {
            var result =  await Collection.Find(filter.ToFilterDefinition()).ToListAsync();
            return result;

        }
        public List<TBaseDocument> FindAll(BaseFilter<TBaseDocument> filter)
        {
            var result = Collection.Find(filter.ToFilterDefinition()).ToList();
            return result;
        }

        public List<TBaseDocument> FindAll(string filter)
        {
            var result = Collection.Find(filter).ToList();
            return result;
        }


        public async Task<TBaseDocument> FindOneAsync(ObjectId id)
        {
            var filter = Builders<TBaseDocument>.Filter.Eq(x => x.Id, id);
            var result = await Collection.Find(filter).SingleAsync();
            return result;
        }

        public List<TBaseDocument> FindAllWithTag(string tagName, Object value)
        {
            var filter = Builders<TBaseDocument>.Filter.Eq("Tags."+tagName, value);
            var result = new List<TBaseDocument>();
            return Collection.Find(filter).ToList();
        }

        public TBaseDocument Find(ObjectId id)
        {
            var filter = Builders<TBaseDocument>.Filter.Eq(x => x.Id, id);
            var result = Collection.Find(filter).SingleOrDefault();
            return result;
        }

        public TBaseDocument Find(string id)
        {

            bool res = ObjectId.TryParse(id, out ObjectId objectId);
            if (res == false) return null;

            return Find(objectId);
        }

        public TBaseDocument FindByString(string id)
        {
            return Find(id);
        }

        public async Task<TBaseDocument> UpsertAsync(TBaseDocument doc)
        {
            MongoEventWatcher.InvokeFor(doc.GetType(),
                Find(doc.Id) == null ? WatcherDataOperation.Create : WatcherDataOperation.Update, doc);

            Validate(doc);

            //UpdatePropertyReferences.Execute(doc);

            var replaceOneResult = await Collection.ReplaceOneAsync<TBaseDocument>(
                x => x.Id == doc.Id,
                doc,
                new ReplaceOptions {IsUpsert = true});


            return doc;
        }

        public TBaseDocument Upsert(TBaseDocument doc)
        {
            MongoEventWatcher.InvokeFor(doc.GetType(),
                Find(doc.Id) == null ? WatcherDataOperation.Create : WatcherDataOperation.Update, doc);

            Validate(doc);

            //UpdatePropertyReferences.Execute(doc);

            doc.ModifiedDateTime = DateTime.Now;
            
                 var replaceOneResult = Collection.ReplaceOne(
                    x => x.Id == doc.Id,
                    doc,
                    new ReplaceOptions { IsUpsert = true });

            return doc;
        }

        public void RemoveMany(BaseFilter<TBaseDocument> filter)
        {
            _context.Database.GetCollection<TBaseDocument>(GetCollectionName())
                .DeleteMany(filter.ToFilterDefinition(), CancellationToken.None);
        }

        public async Task RemoveManyAsync(BaseFilter<TBaseDocument> filter)
        {
            await Task.Run(() =>
                _context.Database.GetCollection<TBaseDocument>(GetCollectionName())
                .DeleteManyAsync(filter.ToFilterDefinition(), CancellationToken.None)
               );
        }

        public void RemoveOne(TBaseDocument doc)
        {
            doc.RemoveAllLinks();
            MongoEventWatcher.InvokeFor(doc.GetType(), WatcherDataOperation.Delete, doc);

            var filter = Builders<TBaseDocument>.Filter.Eq(x => x.Id, doc.Id);

            _context.Database.GetCollection<TBaseDocument>(GetCollectionName())
                .DeleteOne(filter, CancellationToken.None);
        }

        public async Task RemoveOneAsync(TBaseDocument doc)
        {
            doc.RemoveAllLinks();

            MongoEventWatcher.InvokeFor(doc.GetType(), WatcherDataOperation.Delete, doc);

            var filter = Builders<TBaseDocument>.Filter.Eq(x => x.Id, doc.Id);

            await Task.Run(() =>
                _context.Database.GetCollection<TBaseDocument>(GetCollectionName())
                .DeleteOneAsync(filter, CancellationToken.None)
            );
        }

        public void InsertNoValidate(TBaseDocument doc)
        {
            MongoEventWatcher.InvokeFor(doc.GetType(),
                Find(doc.Id) == null ? WatcherDataOperation.Create : WatcherDataOperation.Update, doc);

            Collection.InsertOne(doc,new InsertOneOptions() {BypassDocumentValidation = true});
        }

        private static void Validate(TBaseDocument doc)
        {
            var validationErrors = doc.Validate();
            if (validationErrors.Count > 0)
            {
                var errors = new StringBuilder();
                foreach (var validationError in validationErrors)
                {
                    errors.AppendLine(validationError.ErrorMessage);
                }
                throw new Exception("Validation failed:\n" + errors);
            }
        }

        public RepositoryBase()
        {

            bool isCommon = typeof(TBaseDocument).GetInterfaces()
                .Contains(typeof(ICommonDatabaseObject));

            if (isCommon)
            {
                //EnvironmentSettings.Database = MongoDatabase.Common;
                _context = new CommonContext();

            } else if (EnvironmentSettings.Database == MongoDatabase.VulcanCrm)
            {
                _context = new VulcanContext();
            }
            else if (EnvironmentSettings.Database == MongoDatabase.VulcanHrs)
            {
                _context = new HrsContext();
            }
        }

        public List<TBaseDocument> FindAll(FilterDefinition<TBaseDocument> filter)
        {
            return Collection.Find(filter).ToList();
        }
    }
}
