using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class CompanyActivityView
    {
        public CrmUserRef SalesPerson { get; set; }
        public int TotalQuotes => CompanyActivities.Sum(x => x.TotalQuotes);
        public List<CompanyActivity> CompanyActivities { get; set; } = new List<CompanyActivity>();

        public CompanyActivityView()
        {

        }

        public CompanyActivityView(CrmUserRef salesPerson)
        {
            SalesPerson = salesPerson;
        }

    }

    public class ProspectActivityView
    {
        public CrmUserRef SalesPerson { get; set; }
        public int TotalQuotes => ProspectActivities.Sum(x => x.TotalQuotes);
        public List<ProspectActivity> ProspectActivities { get; set; } = new List<ProspectActivity>();

        public ProspectActivityView()
        {

        }

        public ProspectActivityView(CrmUserRef salesPerson)
        {
            SalesPerson = salesPerson;
        }

    }

    public class CompanyActivity
    {
        public CompanyRef Company { get; set; }
        public int TotalQuotes { get; set; }
        public int Wins { get; set; }
        //public decimal AverageMargin { get; set; }

        public CompanyActivity(CompanyRef company, int count, int wins) //, decimal averageMargin)
        {
            Company = company;
            TotalQuotes = count;
            Wins = wins;
            //AverageMargin = averageMargin;
        }

    }

    public class ProspectActivity
    {
        public ProspectRef Prospect { get; set; }
        public int TotalQuotes { get; set; }
        public int Wins { get; set; }
    }

}
