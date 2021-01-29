using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Driver;

namespace UTL.Hrs.Production.Builder
{
    public class HrsDocumentCopier<TBaseDocument> where TBaseDocument : BaseDocument
    {
        public virtual (long Removed, long Added) Move()
        {
            EnvironmentSettings.HrsDevelopment();
            var values = new MongoRawQueryHelper<TBaseDocument>().GetAll();

            EnvironmentSettings.HrsProduction();
            var queryHelper = new MongoRawQueryHelper<TBaseDocument>();

            var session = queryHelper.GetClientSession();

            queryHelper.StartTransaction(session);

            var removeFilter = queryHelper.FilterBuilder.Empty;
            var deleteResult = queryHelper.DeleteMany(removeFilter);
            queryHelper.InsertMany(values.ToArray());

            queryHelper.CommitTransaction(session);

            return (deleteResult.DeletedCount,values.Count);
        }

        public virtual (long Removed, long Added) RemoveAllAndAddThese(FilterDefinition<TBaseDocument> filter)
        {
            EnvironmentSettings.HrsDevelopment();
            var queryHelperSource = new MongoRawQueryHelper<TBaseDocument>();
            var documentsToInsert = queryHelperSource.Find(filter).ToList();

            EnvironmentSettings.HrsProduction();
            var queryHelper = new MongoRawQueryHelper<TBaseDocument>();

            var session = queryHelper.GetClientSession();

            queryHelper.StartTransaction(session);

            var removeFilter = queryHelper.FilterBuilder.Empty;
            var deleteResult = queryHelper.DeleteMany(removeFilter);
            queryHelper.InsertMany(documentsToInsert.ToArray());

            queryHelper.CommitTransaction(session);

            return (deleteResult.DeletedCount, documentsToInsert.Count);

        }

        public virtual (long Removed, long Added) AddThese(List<TBaseDocument> documents)
        {

            EnvironmentSettings.HrsProduction();
            var queryHelper = new MongoRawQueryHelper<TBaseDocument>();

            var session = queryHelper.GetClientSession();

            queryHelper.StartTransaction(session);

            queryHelper.InsertMany(documents.ToArray());

            queryHelper.CommitTransaction(session);

            return (0, documents.Count);

        }

        public virtual (long Removed, long Added) AddThese(FilterDefinition<TBaseDocument> filter)
        {
            EnvironmentSettings.HrsDevelopment();
            var queryHelperSource = new MongoRawQueryHelper<TBaseDocument>();
            var documentsToInsert = queryHelperSource.Find(filter).ToList();

            EnvironmentSettings.HrsProduction();
            var queryHelper = new MongoRawQueryHelper<TBaseDocument>();

            var session = queryHelper.GetClientSession();

            queryHelper.StartTransaction(session);

            queryHelper.InsertMany(documentsToInsert.ToArray());

            queryHelper.CommitTransaction(session);

            return (0, documentsToInsert.Count);

        }

        public virtual (long Removed, long Added) AddMissing()
        {
            EnvironmentSettings.HrsDevelopment();
            var queryHelperSource = new MongoRawQueryHelper<TBaseDocument>();
            var documentsToInsert = queryHelperSource.GetAll();

            EnvironmentSettings.HrsProduction();
            var queryHelper = new MongoRawQueryHelper<TBaseDocument>();

            var session = queryHelper.GetClientSession();

            queryHelper.StartTransaction(session);

            foreach (var baseDocument in documentsToInsert)
            {
                if (!queryHelper.Exists(baseDocument))
                {
                    queryHelper.Upsert(baseDocument);
                }
            }

            queryHelper.CommitTransaction(session);

            return (0, documentsToInsert.Count);

        }

        public virtual (long Removed, long Added) RemoveAllNotInDevelopment()
        {
            EnvironmentSettings.HrsDevelopment();
            var queryHelperDev = new MongoRawQueryHelper<TBaseDocument>();
            var documentsInDevelopment = queryHelperDev.GetAll();

            EnvironmentSettings.HrsProduction();
            var queryHelperProduction = new MongoRawQueryHelper<TBaseDocument>();
            var documentsInProduction = queryHelperProduction.GetAll();
            var session = queryHelperProduction.GetClientSession();

            queryHelperProduction.StartTransaction(session);

            var deleted = 0;
            foreach (var baseDocument in documentsInProduction)
            {
                if (documentsInDevelopment.All(x => x.Id != baseDocument.Id))
                {
                    queryHelperProduction.DeleteOne(baseDocument.Id);
                    deleted++;
                }
            }

            queryHelperProduction.CommitTransaction(session);

            return (deleted, 0);

        }

    }
}
