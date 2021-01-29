using System.Collections.Generic;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperPortal
    {
        NewPortalInvitationModel CreateNewPortalInvitationModel(string companyId, string contactId, string salesPersonId);
        List<PortalInvitationModel> GetInvitationsForTeam(string teamId);
        List<PortalInvitationModel> GetInvitationsSentBySalesPerson(string salesPersonId);
        List<PortalInvitationModel> GetInvitationsSentToCompany(string companyId);
        List<PortalInvitationModel> GetInvitationsSentToContact(string contactId);
        PortalInvitationModel SendPortalInvitation(NewPortalInvitationModel model);
    }
}