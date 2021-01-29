using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class QuoteQueryModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public CrmUserRef SalesPerson { get; set; }
        public TeamRef Team { get; set; }
        public QuoteQueryCompanyOptions CompanyOptions { get; set; } 

        public QuoteQueryDateOptions DateOptions { get; set; } = new QuoteQueryDateOptions();
        public QuoteQueryProductOptions ProductOptions { get; set; } = new QuoteQueryProductOptions();
        public QuoteQueryQuoteOptions QuoteOptions { get; set; } = new QuoteQueryQuoteOptions();
        public QuoteQueryScope ScopeOption { get; set; } = QuoteQueryScope.OnlyMyQuotes;
        public QuoteQueryTeamMemberOptions TeamMemberOptions { get; set; } = new QuoteQueryTeamMemberOptions();
        public string Name { get; set; } = string.Empty;
        public List<QuoteQueryExecutionHistory> RunHistory { get; set; } = new List<QuoteQueryExecutionHistory>();

        public QuoteQueryModel()
        {

        }

        public QuoteQueryModel(string application, string userId, QuoteQuery query)
        {
            Application = application;
            UserId = userId;
            Id = query.Id.ToString();
            SalesPerson = query.SalesPerson;
            Team = query.Team;
            CompanyOptions = query.CompanyOptions;
            DateOptions = query.DateOptions;
            ProductOptions = query.ProductOptions;
            ScopeOption = query.ScopeOption;
            Name = query.Name;
            RunHistory = query.RunHistory;
            TeamMemberOptions = query.TeamMemberOptions;
        }
    }
}
