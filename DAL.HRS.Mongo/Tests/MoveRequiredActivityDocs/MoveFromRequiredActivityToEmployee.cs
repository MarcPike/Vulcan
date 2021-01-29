using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.MoveRequiredActivityDocs
{
    [TestFixture]
    public class MoveFromRequiredActivityToEmployee
    {

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            foreach (var requiredActivity in RequiredActivity.Helper.GetAll())
            {

                var docs = SupportingDocument.Helper.Find(x => x.BaseDocumentId == requiredActivity.Id).ToList();
                if (docs.Any())
                {
                    Console.WriteLine(docs.First().FileName);
                }
            }
        }
    }
}
