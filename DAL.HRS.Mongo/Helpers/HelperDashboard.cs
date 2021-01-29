using DAL.HRS.Mongo.DocClass.Hrs_User;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperDashboard : IHelperDashboard
    {
        public HrsDashboardResults GetHrsDashboard(HrsUser hrsUser)
        {
            return HrsDashboardResults.GetDashboardResults(hrsUser);
        }

        public HseDashboardResults GetHseDashboard(HrsUser hrsUser)
        {
            return HseDashboardResults.GetDashboardResults(hrsUser);
        }

        public ExecutiveDashboardResults GetExecutiveDashboard()
        {
            return ExecutiveDashboardResults.GetDashboardResults();
        }
    }
}