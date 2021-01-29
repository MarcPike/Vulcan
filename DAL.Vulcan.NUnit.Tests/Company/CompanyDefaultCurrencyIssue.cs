using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Company
{
    [TestFixture]
    class CompanyDefaultCurrencyIssue
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

    //    [Test]
    //    public void FindCompany()
    //    {
    //        var company = Mongo.DocClass.Companies.Company.Helper
    //            .Find(x => x.Location.Branch == "SIN" && x.Code == "00355").SingleOrDefault();

    //        Assert.IsNotNull(company);

    //        var companyDefault = Mongo.DocClass.Companies.CompanyDefaults.GetCompanyDefaults("SIN", company, false);

    //    }

    //    [Test]
    //    public void FixCompanyNamesSingaporeTeam()
    //    {
    //        var team = Team.Helper.Find(x => x.Name == "Singapore Sales").First();

    //        foreach (var teamCompany in team.Companies)
    //        {
    //            var realCompany = teamCompany.AsCompany();
    //            if (teamCompany.Name != realCompany.Name)
    //            {
    //                Console.WriteLine($"{teamCompany.Code}-{teamCompany.Name} changed to {realCompany.Name}");
    //                teamCompany.Name = realCompany.Name;
    //                //var quotes = CrmQuote.Helper.Find(x => x.Company.Id == teamCompany.Id).ToList();
    //                //foreach (var crmQuote in quotes)
    //                //{
    //                //    if (crmQuote.Company.Name != realCompany.Name)
    //                //    {
    //                //        Console.WriteLine($"    QuoteId: {crmQuote.QuoteId} {crmQuote.ReportDate} Company Name is wrong");

    //                //    }
    //                //}
    //            }
    //        }

    //        Team.Helper.Upsert(team);
    //    }

    }
}
