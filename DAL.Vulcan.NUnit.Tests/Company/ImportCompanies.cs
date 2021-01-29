using System;
using System.Diagnostics;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Importers;
//using DAL.Vulcan.Mongo.Importers;
using DAL.Vulcan.Test;
using MongoDB.Driver;
using NUnit.Framework;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Company
{
    [TestFixture]
    public class ImportCompanies
    {
        [Test]
        public void DoIt()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;

            var importer = new CompanyImporter {DebugMode = false};
            importer.Execute("INC");
            Console.WriteLine("INC Import Results");
            Console.WriteLine("==================");
            Console.WriteLine(ObjectDumper.Dump(importer));
            //importer.Refresh("EUR");
            //Trace.WriteLine("EUR Import Results", ObjectDumper.Dump(importer));
            ////importer.LoadAllTEmplates("CHI");
            ////Trace.WriteLine("CHI Import Results", ObjectDumper.Dump(importer));
            //importer.Refresh("SIN");
            //Trace.WriteLine("SIN Import Results", ObjectDumper.Dump(importer));
            //importer.Refresh("MSA");
            //Trace.WriteLine("MSA Import Results", ObjectDumper.Dump(importer));
            //importer.Refresh("DUB");
            //Trace.WriteLine("DUB Import Results", ObjectDumper.Dump(importer));
            //importer.Refresh("CAN");
            //Trace.WriteLine("CAN Import Results", ObjectDumper.Dump(importer));
        }

        [Test]
        public void CorrectMandRManufacturing()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;

            var correctCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>().AsQueryable()
                .Single(x => x.Code == "02350" && x.Location.Office == "Telge");

            Console.WriteLine(correctCompany.Name);
            var correctCompanyRef = correctCompany.AsCompanyRef();

            var repQuote = new RepositoryBase<CrmQuote>();
            var quotesWithWrongCompany = repQuote.AsQueryable().Where(x=>x.Company.Code == "04547").ToList();
            foreach (var crmQuote in quotesWithWrongCompany)
            {
                crmQuote.Company = correctCompanyRef;
                Console.WriteLine(crmQuote.QuoteId);
                repQuote.Upsert(crmQuote);
            }
        }

        [Test]
        public void CorrectDoubleEagle()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;

            var correctCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>().AsQueryable()
                .Single(x => x.Code == "00413" && x.Location.Office == "Telge");

            Console.WriteLine(correctCompany.Name);
            var correctCompanyRef = correctCompany.AsCompanyRef();

            var repQuote = new RepositoryBase<CrmQuote>();
            var quotesWithWrongCompany = repQuote.AsQueryable().Where(x => x.Company.Code == "04582" && x.Company.Name == "Double Eagle Alloys").ToList();
            foreach (var crmQuote in quotesWithWrongCompany)
            {
                crmQuote.Company = correctCompanyRef;
                Console.WriteLine(crmQuote.QuoteId);
                repQuote.Upsert(crmQuote);
            }
        }

        [Test]
        public void CorrectFarmersCopper()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;

            var correctCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>().AsQueryable()
                .Single(x => x.Code == "00833" && x.Location.Office == "Telge" && x.Name.StartsWith("Farm"));

            Console.WriteLine(correctCompany.Name);
            var correctCompanyRef = correctCompany.AsCompanyRef();

            var repQuote = new RepositoryBase<CrmQuote>();
            var quotesWithWrongCompany = repQuote.AsQueryable().Where(x => x.Company.Code == "00602").ToList();
            foreach (var crmQuote in quotesWithWrongCompany)
            {
                crmQuote.Company = correctCompanyRef;
                Console.WriteLine(crmQuote.QuoteId);
                repQuote.Upsert(crmQuote);
            }
        }

        [Test]
        public void CorrectSpecialMetals()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;

            var correctCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>().AsQueryable()
                .Single(x => x.Code == "00122" && x.Location.Office == "Telge" && x.Name.StartsWith("Special"));

            Console.WriteLine(correctCompany.Name);
            var correctCompanyRef = correctCompany.AsCompanyRef();

            var repQuote = new RepositoryBase<CrmQuote>();
            var quotesWithWrongCompany = repQuote.AsQueryable().Where(x => x.Company.Code == "00045").ToList();
            foreach (var crmQuote in quotesWithWrongCompany)
            {
                crmQuote.Company = correctCompanyRef;
                Console.WriteLine(crmQuote.QuoteId);
                repQuote.Upsert(crmQuote);
            }
        }



    }
}
