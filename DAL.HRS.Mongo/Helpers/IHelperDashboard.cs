using DAL.HRS.Mongo.DocClass.Hrs_User;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperDashboard
    {
        HrsDashboardResults GetHrsDashboard(HrsUser hrsUser);
        HseDashboardResults GetHseDashboard(HrsUser hrsUser);
        ExecutiveDashboardResults GetExecutiveDashboard();
    }
}