using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class CrmUserModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public UserRef User { get; set; }
        public string UserType { get; set; }
        public bool IsAdmin { get; set; } = false;

        public PersonModelBase PersonalInfo { get; set; }

        public GuidList<Note> Notes { get; set; }

        public ViewConfig ViewConfig { get; set; } 

        public ReferenceList<Team, TeamRef> Teams { get; set; } 
        public ReferenceList<Contact, ContactRef> Contacts { get; set; } 

        public LocationRef Location { get; }
       
        public bool UseMyLocationForPdf { get; set; } = true;

        public bool ReadOnly { get; set; }

        public bool IsCalcAdmin { get; set; }
        public string Coid { get; set; }

        public CrmUserModel(string application, string userId, CrmUser crmUser)
        {
            Application = application;
            UserId = userId;
            User = crmUser.User;
            UserType = crmUser.UserType.ToString();
            Notes = crmUser.Notes;
            ViewConfig = crmUser.ViewConfig;
            //ViewConfig.GetSelectedCompanies(crmUser.User);
            Teams = crmUser.Teams;
            Contacts = crmUser.Contacts;
            IsAdmin = crmUser.IsAdmin;
            PersonalInfo = new PersonModelBase(crmUser.User.AsUser().Person);
            Location = crmUser.User.AsUser().Location;
            UseMyLocationForPdf = crmUser.UseMyLocationForPdf;
            ReadOnly = crmUser.ReadOnly;
            IsCalcAdmin = crmUser.IsCalcAdmin;
            Coid = crmUser.Coid;
        }
    }
}
