using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.ExcelTemplates;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.ExcelTemplates
{
    [TestFixture]
    class ExcelTemplateTesting
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void TestNewTemplate()
        {
            var helperExcelTemplate = new HelperExcelTemplate();
            var crmUser = Mongo.DocClass.CRM.CrmUser.Helper.
                Find(x => x.User.LastName == "Pike").First();
            var model = helperExcelTemplate.GetNewExcelTemplateModel(crmUser);

        }

        [Test]
        public void GetTeamTemplates()
        {
            var crmUser = Mongo.DocClass.CRM.CrmUser.Helper.
                Find(x => x.User.LastName == "Pike").First();
            var team = Team.Helper.Find(x => x.Name == "Houston Sales").First();
            var helperExcelTemplate = new HelperExcelTemplate();
            var models = helperExcelTemplate.GetTemplatesForTeam(team.AsTeamRef());
            foreach (var model in models)
            {
                model.Application = "vulcancrm";
                model.UserId = crmUser.UserId;
                Console.WriteLine(ObjectDumper.Dump(model));
            }

        }

        [Test]
        public void SetAllCreatedBySalesPerson()
        {
            var crmUser = Mongo.DocClass.CRM.CrmUser.Helper.Find(x => x.User.LastName == "Gallegos" && x.User.FirstName == "Isidro").First();
            var crmUserRef = crmUser.AsCrmUserRef();

            foreach (var excelTemplate in ExcelTemplate.Helper.GetAll())
            {
                excelTemplate.CreatedBySalesPerson = crmUserRef;
                ExcelTemplate.Helper.Upsert(excelTemplate);
            }

        }

        [Test]
        public void ExcelTemplatePrototype()
        {
            var helperUser = new HelperUser(new HelperPerson());

            var crmUser = Mongo.DocClass.CRM.CrmUser.Helper.Find(x => x.User.LastName == "Pike").First();
            var crmUserRef = crmUser.AsCrmUserRef();

            var quote = CrmQuote.Helper.Find(x => x.QuoteId == 16704).First();
            var quoteModel = new QuoteModel("vulcancrm", crmUserRef.UserId, quote);

            // Create a new template
            var excelTemplate = new ExcelTemplate()
            {
                Name = "Pike Test Template",
                CreatedBySalesPerson = crmUserRef,
                Team = crmUser.ViewConfig.Team
            };

            excelTemplate.Initialize();
            SimulateExcelDump(excelTemplate, quoteModel, "Last values read from database or new row");


            excelTemplate.GetDefaultQuoteValues();
            SimulateExcelDump(excelTemplate, quoteModel, "After calling GetDefaultValues()");
            

            // Change a column and save it
            var companyName = excelTemplate.GetQuoteColumnForFieldName("CompanyName");
            companyName.Label = "Customer";
            excelTemplate.SetQuoteColumn(companyName);
            ExcelTemplate.Helper.Upsert(excelTemplate);

            // Load the template
            excelTemplate = ExcelTemplate.Helper.FindById(excelTemplate.Id);
            excelTemplate.Initialize();

            SimulateExcelDump(excelTemplate, quoteModel, "After changing Company to Customer");

            // Remove a column
            var companyCode = excelTemplate.GetQuoteColumnForFieldName("CompanyCode");
            excelTemplate.RemoveQuoteColumn(companyCode.Id);

            SimulateExcelDump(excelTemplate, quoteModel, "After Removing Company Code");


            // Change another Label
            var poNumber = excelTemplate.GetQuoteColumnForFieldName("PoNumber");
            poNumber.Label = "Customer PO#";
            excelTemplate.SetQuoteColumn(poNumber);

            SimulateExcelDump(excelTemplate, quoteModel, "After changing PO# to Customer PO#");

            excelTemplate.GetDefaultQuoteValues();
            SimulateExcelDump(excelTemplate, quoteModel, "After going back to Quote Defaults");

            // Remove the template
            ExcelTemplate.Helper.DeleteOne(excelTemplate.Id);

        }

        [Test]
        public void ProductExcelDoc()
        {
            var helperUser = new HelperUser(new HelperPerson());

            var helperExcelTemplate = new HelperExcelTemplate();

            var crmUser = Mongo.DocClass.CRM.CrmUser.Helper.Find(x => x.User.LastName == "Pike").First();
            var crmUserRef = crmUser.AsCrmUserRef();

            var excelTemplateModel = helperExcelTemplate.GetNewExcelTemplateModel(crmUser);

            var lineType = excelTemplateModel.QuoteItemValues.FirstOrDefault(x => x.FieldName == "LineType");
            excelTemplateModel.QuoteItemValues.Remove(lineType);

            excelTemplateModel.Name = "Pike Test Template";

            helperExcelTemplate.SaveExcelTemplate(excelTemplateModel);


            var quote = CrmQuote.Helper.Find(x => x.QuoteId == 16704).First();


            var workbook = helperExcelTemplate.GenerateExcelStream("vulcancrm", crmUser.UserId,
                quote.Id.ToString(),
                excelTemplateModel.Id);
            
            FileStream file = new FileStream(
                $"d:\\{quote.QuoteId.ToString()}-{quote.RevisionNumber.ToString()}.xlsx",
                FileMode.Create, System.IO.FileAccess.Write);

            workbook.Write(file);
            
            helperExcelTemplate.RemoveExcelTemplate(excelTemplateModel.Id);


        }

        private static void SimulateExcelDump(ExcelTemplate excelTemplate, QuoteModel quoteModel, string comments)
        {
            Console.WriteLine(comments);
            Console.WriteLine("Quote Values that will be exported to Excel");
            Console.WriteLine("-------------------------------------------");


            var quoteHeadings = excelTemplate.GetQuoteHeadings();
            var quoteValues = excelTemplate.GetQuoteValues(quoteModel);

            for (int i = 0; i < quoteHeadings.Count; i++)
            {
                Console.WriteLine($"{quoteHeadings[i]} = {quoteValues[i]}");
            }

            var quoteItemHeadings = excelTemplate.GetQuoteItemHeadings(quoteModel);
            var quoteItemValues = excelTemplate.GetQuoteItemValues(quoteModel);

            Console.WriteLine("");
            Console.WriteLine("Quote Item Values that will be exported to Excel");
            Console.WriteLine("------------------------------------------------");

            foreach (var quoteItem in quoteItemValues)
            {
                for (int i = 0; i < quoteItemHeadings.Count; i++)
                {
                    Console.WriteLine($"{quoteItemHeadings[i]} = {quoteItem[i]}");
                }
            }


            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("");
        }
    }
}
