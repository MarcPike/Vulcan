using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Email;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Messages;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using Action = System.Action;

namespace DAL.Vulcan.Mongo.Models
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
