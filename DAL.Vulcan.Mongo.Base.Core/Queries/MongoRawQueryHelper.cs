using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DAL.Vulcan.Mongo.Base.Core.Context;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.EventWatcher;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.Base.Core.Queries
{

    public class MongoRawQueryHelper<TBaseDocument> where TBaseDocument : BaseDocument
    {
        public FilterDefinitionBuilder<TBaseDocument> FilterBuilder => Builders<TBaseDocument>.Filter;

        public ProjectionDefinitionBuilder<TBaseDocument> ProjectionBuilder => Builders<TBaseDocument>.Projection;

        public SortDefinitionBuilder<TBaseDocument> SortBuilder => Builders<TBaseDocument>.Sort;

        public UpdateDefinitionBuilder<TBaseDocument> UpdateBuilder => Builders<TBaseDocument>.Update;

        public IndexKeysDefinitionBuilder<TBaseDocument> IndexKeysDefinitionBuilder => Builders<TBaseDocument>.IndexKeys;

        public ContextBase Context { get; set; }

        public IAggregateFluent<TBaseDocument> GetAggregateFluent()
        {
            var aggregateOptions = new AggregateOptions() {AllowDiskUse = true};
            return Collection.Aggregate(aggregateOptions);
        }

        public IClientSession GetClientSession()
        {
            var client = new MongoClient(Context.ConnectionString);
            return client.StartSession();
        }

        public void StartTransaction(IClientSession session)
        {
            session.StartTransaction();
        }

        public void AbortTransaction(IClientSession session)
        {
            session.AbortTransaction();
        }

        public void CommitTransaction(IClientSession session)
        {
            session.CommitTransaction();
        }

        public IMongoDatabase Database => Context.Database;

        public long GetRowCount(FilterDefinition<TBaseDocument> filter)
        {
            return Collection.CountDocuments(filter);
        }
        public long GetRowCount()
        {
            var filter = FilterBuilder.Empty;
            return Collection.CountDocuments(filter);
        }

        public MongoRawQueryHelper()
        {

            bool isCommon = typeof(TBaseDocument).GetInterfaces()
                .Contains(typeof(ICommonDatabaseObject));

            if (isCommon)
            {
                Context = new CommonContext();
            }
            else if (EnvironmentSettings.Database == MongoDatabase.VulcanCrm)
            {
                Context = new VulcanContext();
            }
            else if (EnvironmentSettings.Database == MongoDatabase.VulcanHrs)
            {
                Context = new HrsContext();
            }
            else if (EnvironmentSettings.Database == MongoDatabase.VulcanBI)
            {
                Context = new BiContext();
            }
        }



        public List<TBaseDocument> GetAll()
        {
            var filter = FilterBuilder.Empty;
            return Find(filter).ToList();
        }

        public IMongoCollection<TBaseDocument> Collection => Context.Database.GetCollection<TBaseDocument>(GetCollectionName());

        public string GetCollectionName()
        {
            return typeof(TBaseDocument).Name;
        }

        public TBaseDocument FindById(ObjectId id)
        {
            var filter = FilterBuilder.Eq(x => x.Id, id);
            return Collection.Find(filter).FirstOrDefault();
        }

        public FilterDefinition<TBaseDocument> GetFilterDefinitionForId(ObjectId id)
        {
            return FilterBuilder.Eq(x => x.Id, id);
        }
        public FilterDefinition<TBaseDocument> GetFilterDefinitionForId(string id)
        {
            var objectId = ObjectId.Parse(id);
            return GetFilterDefinitionForId(objectId);
        }

        public bool Exists(TBaseDocument doc)
        {
            var filter = FilterBuilder.Where(x => x.Id == doc.Id);
            var projection = ProjectionBuilder.Include(x => x.Id);
            return FindWithProjection<TBaseDocument>(filter, projection).Any();
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

        public TBaseDocument FindById(string id)
        {
            if (id == string.Empty) return null;
            var objectId = ObjectId.Parse(id);
            return FindById(objectId);
        }

        public IFindFluent<TBaseDocument, TBaseDocument> Find(Expression<Func<TBaseDocument,bool>> filter, FindOptions options = null )
        {
            return Collection.Find(filter,options);
        }

        public List<TBaseDocument> Find(FilterDefinition<TBaseDocument> filter)
        {
            return Collection.Find(filter).ToList();
        }

        //public BsonDocument Explain(FilterDefinition<TBaseDocument> filter)
        //{
        //    var options = new FindOptions()
        //    {
        //        Modifiers = new BsonDocument("$explain", true)
        //    };
        //    return Collection.Find(filter, options)
        //        .Project(new BsonDocument())
        //        .FirstOrDefault();
        //}


        public List<TBaseDocument> FindWithSort(FilterDefinition<TBaseDocument> filter,
            SortDefinition<TBaseDocument> sortDefinition)
        {
            return Collection.Find(filter).Sort(sortDefinition).ToList();
        }

        public List<TProjection> FindWithProjection<TProjection>(FilterDefinition<TBaseDocument> filter,
            ProjectionDefinition<TBaseDocument, TProjection> project, SortDefinition<TBaseDocument> sortDefinition = null)
        {
            Collection.WithReadConcern(ReadConcern.Local);

            if (sortDefinition != null)
            {
                return Collection.Find(filter).Project(project).Sort(sortDefinition).ToList();
            }
            else
            {
                return Collection.Find(filter).Project(project).ToList();
            }
        }

        public void InsertMany(TBaseDocument[] docs)
        {
            Collection.InsertMany(docs);
        }

        public TBaseDocument Upsert(TBaseDocument doc)
        {
            var watcherOperation = Exists(doc) ? WatcherDataOperation.Create : WatcherDataOperation.Update;

            MongoEventWatcher.InvokeFor(doc.GetType(), watcherOperation, doc);

            Validate(doc);

            doc.ModifiedDateTime = DateTime.Now;

            var replaceOneResult = Collection.ReplaceOne(
                x => x.Id == doc.Id,
                doc,
                new ReplaceOptions { IsUpsert = true });

            return doc;
        }

        public UpdateResult UpdateOne(FilterDefinition<TBaseDocument> filter, UpdateDefinition<TBaseDocument> update)
        {
            return Collection.UpdateOne(filter, update);
        }

        public ReplaceOneResult ReplaceOne(TBaseDocument doc)
        {
            var filter = FilterBuilder.Eq(x => x.Id, doc.Id);
            return Collection.ReplaceOne(filter, doc);
        }

        public UpdateResult UpdateMany(FilterDefinition<TBaseDocument> filter, UpdateDefinition<TBaseDocument> update)
        {
            return Collection.UpdateMany(filter, update);
        }

        public DeleteResult DeleteOne(FilterDefinition<TBaseDocument> filter)
        {
            return Collection.DeleteOne(filter);
        }

        public DeleteResult DeleteOne(ObjectId id)
        {
            var filter = FilterBuilder.Where(x => x.Id == id);
            return Collection.DeleteOne(filter);
        }

        public DeleteResult DeleteOne(string id)
        {
            return DeleteOne(ObjectId.Parse(id));
        }

        public DeleteResult DeleteMany(FilterDefinition<TBaseDocument> filter)
        {
            return Collection.DeleteMany(filter);
        }

    }
}
