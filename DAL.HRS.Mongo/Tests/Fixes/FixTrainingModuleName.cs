using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.Tests.Fixes
{
    [TestFixture]
    class FixTrainingModuleName
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void Execute()
        {

            var supportingDocuments = new RepositoryBase<SupportingDocument>().AsQueryable()
                 .Where(x => x.Module.Id == "5d7a891e095f5825801bbf7e").ToList();

            var trainingSupportingDocuments = new RepositoryBase<SupportingDocument>().AsQueryable()
                .Where(x => x.Module.Id == "5d7a891e095f5825801bbf7f").ToList();

            foreach (var doc in supportingDocuments)
            {

                doc.Module.Id = "5d7a891e095f5825801bbf7f";
                doc.Module.Name = "Training";

                SupportingDocument.Helper.Upsert(doc);


            }

        }
    }
}

