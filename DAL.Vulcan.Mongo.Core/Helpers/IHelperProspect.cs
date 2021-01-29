using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperProspect
    {
        ProspectModel GetNewProspectModel(string application, string userId);
        ProspectModel GetNewProspectModel(string application, string userId, string locationId);
        (ProspectModel ProspectModel, ProspectRef ProspectRef) SaveProspect(ProspectModel model);

        (ProspectModel ProspectModel, ProspectRef ProspectRef) GetProspect(string application, string userId,
            string prospectId);
        void ConvertProspectIntoCompany(string prospectId, string companyId, string teamId);
    }
}