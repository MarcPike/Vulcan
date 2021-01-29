using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using System.Collections.Generic;
using Action = DAL.Vulcan.Mongo.DocClass.CRM.Action;

namespace DAL.Vulcan.Mongo.Models
{
    public class ContactModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public ContactPersonModel Person { get; set; }
        public List<Note> Notes { get; set; } = new List<Note>();
        public ReferenceList<Company,CompanyRef> Companies { get; set; }
        public ReferenceList<Prospect, ProspectRef> Prospects { get; set; }
        public ReferenceList<DocClass.CRM.CrmUser,CrmUserRef> CrmUsers { get; set; }
        public ReferenceList<Action, ActionRef> Actions { get; set; }
        public string Position { get; set; } 
        public ContactRef ReportsTo { get; set; }
        public List<string> SearchTags { get; set; }


        public ContactModel()
        {
        }

        public ContactModel(string application, string userId, Contact contact)
        {
            Application = application;
            UserId = userId;
            Id = contact.Id.ToString();
            Person = new ContactPersonModel(contact)
            {
                Application = application,
                UserId = userId
            };
            Notes = contact.Notes;
            Companies = contact.Companies;
            Prospects = contact.Prospects;
            CrmUsers = contact.CrmUsers;
            Actions = contact.Actions;
            Position = contact.Position;
            SearchTags = contact.SearchTags;
            ReportsTo = contact.ReportsTo;
        }

    }
}
