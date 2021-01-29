using DAL.Vulcan.Mongo.Base.Core.Repository;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.DocClass.Dashboards
{
    public class PortfolioBooked : BaseDashboard
    {
        public PortfolioBooked()
        {
            Name = "Portfolio Booked";
        }

        public static PortfolioBooked GetDashboardForUserId(string userId)
        {
            var rep = new RepositoryBase<PortfolioBooked>();
            var dashboard = rep.AsQueryable().SingleOrDefault(x => x.UserId == userId);
            if (dashboard == null)
            {
                dashboard = new PortfolioBooked();

                dashboard.DateRangeChosen = dashboard.AvailableDateValues.Single(x => x.Name == "This Year to Date");
                dashboard.CurrencyChosen = dashboard.AvailableCurrencies.Single(x => x.Code == "USD");
                dashboard.CoidCountriesChosen = dashboard.AvailableCoidCountries;
                dashboard.UserId = userId;
                dashboard.ChosenRollup = Dashboards.RollupBy.GetDefaults().Single(x => x.Value == "Coid");
                rep.Upsert(dashboard);
            }
            return dashboard;
        }
    }
}