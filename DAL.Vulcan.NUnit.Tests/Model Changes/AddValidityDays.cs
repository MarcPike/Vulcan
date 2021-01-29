using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Model_Changes
{
    [TestFixture]
    public class AddValidityDays
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
        }

        [Test]
        public void Execute()
        {
            var rep = new RepositoryBase<CrmQuote>();
            var quotes = rep.AsQueryable().ToList();
            foreach (var crmQuote in quotes)
            {
                var modified = false;
                var validitySplit = crmQuote.Validity.Split(' ');
                var validityValueString = validitySplit.FirstOrDefault() ?? "7";
                int validityDays = 7;
                try
                {
                    validityDays = Convert.ToInt32(validityValueString);

                }
                catch (Exception e)
                {
                    Console.WriteLine($"QuoteId: {crmQuote.QuoteId} Validity: {crmQuote.Validity} had error:"+"\n"+e.Message);   
                }
                if (crmQuote.ValidityDays != validityDays)
                {
                    crmQuote.ValidityDays = validityDays;
                    modified = true;
                }
                if ((crmQuote.Status == PipelineStatus.Submitted) && (crmQuote.SubmitDate != null))
                {
                    
                    if (crmQuote.SubmitDate.Value.Date.AddDays(crmQuote.ValidityDays +1) < DateTime.Now.Date)
                    {
                        crmQuote.Status = PipelineStatus.Expired;
                        crmQuote.ExpireDate = DateTime.Now.Date;
                        modified = true;
                    } 
                }

                if ((crmQuote.Status == PipelineStatus.Expired) && (crmQuote.SubmitDate != null))
                {
                    if (crmQuote.SubmitDate.Value.AddDays(crmQuote.ValidityDays + 1) > DateTime.Now.Date)
                    {
                        crmQuote.Status = PipelineStatus.Submitted;
                        modified = true;
                    }
                }

                if (modified) rep.Upsert(crmQuote);

            }
        }
    }
}
