using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using System.Collections.Generic;
using Action = DAL.Vulcan.Mongo.Core.DocClass.CRM.Action;

namespace DAL.Vulcan.Mongo.Core.Finders
{
    public class ActionSearch
    {

        public Dictionary<string, ActionRef> Results { get; set; } = new Dictionary<string, ActionRef>();

        public void ExecuteFind(string searchFor, List<Action> searchValues, ActionSearchLevel searchLevel)
        {
            searchFor = searchFor.ToUpper();
            foreach (var action in searchValues)
            {
                if (ActionMainSearch(action, searchFor)) continue;

                if (searchLevel != ActionSearchLevel.Deep) continue;

                if (ActionEmailSearch(action, searchFor)) continue;

                if (ActionPhoneNumberSearch(action, searchFor)) continue;

                if (ActionContactsSearch(action, searchFor)) continue;
            }
        }
    

    private  bool ActionContactsSearch(Action action, string searchFor)
        {
            foreach (var contact in action.Contacts.ConvertAll(x => x.AsContact()))
            {
                if (ContactSearchName(action, searchFor, contact)) return true;
                foreach (var company in contact.Companies.ConvertAll(x => x.AsCompany()))
                {
                    if (ContactCompanySearch(action, searchFor, company)) return true;
                }
                foreach (var address in contact.Person.Addresses)
                {
                    if (AddressSearch(action, searchFor, address)) return true;
                }
                foreach (var note in contact.Notes)
                {
                    if (ContactNotesSearch(action, searchFor, note)) return true;
                }
                foreach (var searchTag in contact.SearchTags)
                {
                    if (ContactSearchTagSearch(action, searchFor, searchTag)) return true;
                }
            }

                return false;
        }

        private  bool ContactSearchTagSearch(Action action, string searchFor, string searchTag)
        {
            if (searchTag.ToUpper().Contains(searchFor))
            {
                Results.Add("Contact.SearchTag", action.AsActionRef());
                return true;
            }
            return false;
        }

        private  bool ContactNotesSearch(Action action, string searchFor, Note note)
        {
            if (note.Notes.ToUpper().Contains(searchFor))
            {
                Results.Add("Action.Contact.Notes",action.AsActionRef());
            }
            foreach (var searchTag in note.SearchTags)
            {
                if (searchTag.ToUpper().Contains(searchFor))
                {
                    Results.Add("Action.Contact.Notes.SearchTag", action.AsActionRef());
                }

            }
            return false;
        }


        private  bool AddressSearch(Action action, string searchFor, Address address)
        {
            if (address.AddressLine1.ToUpper().Contains(searchFor))
            {
                Results.Add("Action.Contact.Person.Address.AddressLine1", action.AsActionRef());
                return true;
            }
            if (address.AddressLine2.ToUpper().Contains(searchFor))
            {
                Results.Add("Action.Contact.Person.Address.AddressLine2", action.AsActionRef());
                return true;
            }
            if (address.City.ToUpper().Contains(searchFor))
            {
                Results.Add("Action.Contact.Person.Address.City", action.AsActionRef());
                return true;
            }
            if (address.StateProvince.ToUpper().Contains(searchFor))
            {
                Results.Add("Action.Contact.Person.Address.StateProvince", action.AsActionRef());
                return true;
            }
            if (address.PostalCode.ToUpper().Contains(searchFor))
            {
                Results.Add("Action.Contact.Person.Address.PostalCode", action.AsActionRef());
                return true;
            }
            foreach (var searchTag in address.SearchTags)
            {
                if (searchTag.ToUpper().Contains(searchFor))
                {
                    Results.Add("Action.Contact.Person.Address.SearchTag", action.AsActionRef());
                }

            }

            return false;
        }

        private  bool ContactCompanySearch(Action action, string searchFor, Company company)
        {
            if (company.Name.ToUpper().Contains(searchFor))
            {
                Results.Add("Action.Contact.Company.Name", action.AsActionRef());
                return true;
            }
            if (company.Code.ToUpper().Contains(searchFor))
            {
                Results.Add("Action.Contact.Company.Code", action.AsActionRef());
                return true;
            }
            foreach (var searchTag in company.SearchTags)
            {
                if (searchTag.ToUpper().Contains(searchFor))
                {
                    Results.Add("Action.Contact.Company.SearchTag", action.AsActionRef());
                }

            }

            return false;
        }

        private  bool ContactSearchName(Action action, string searchFor, Contact contact)
        {
            if (contact.Person.FirstName.ToUpper().Contains(searchFor))
            {
                Results.Add("Action.Contact.FirstName", action.AsActionRef());
                return true;
            }
            if (contact.Person.LastName.ToUpper().Contains(searchFor))
            {
                Results.Add("Action.Contact.LastName", action.AsActionRef());
                return true;
            }
            if (contact.Person.MiddleName.ToUpper().Contains(searchFor))
            {
                Results.Add("Action.Contact.MiddleName", action.AsActionRef());
                return true;
            }
            return false;
        }

        private  bool ActionPhoneNumberSearch(Action action, string searchFor)
        {
            foreach (var phoneNumber in action.PhoneNumbers)
            {
                if (phoneNumber.Number.ToUpper().Contains(searchFor))
                {
                    Results.Add("Action.Phone#", action.AsActionRef());
                    return true;
                }
                foreach (var searchTag in phoneNumber.SearchTags)
                {
                    if (searchTag.ToUpper().Contains(searchFor))
                    {
                        Results.Add("Action.PhoneNumber.SearchTag", action.AsActionRef());
                    }

                }

            }

            return false;
        }

        private  bool ActionEmailSearch(Action action, string searchFor)
        {
            foreach (var emailAddress in action.EmailAddresses)
            {
                if (emailAddress.Address.ToUpper().Contains(searchFor))
                {
                    Results.Add("Action.EmailAddress", action.AsActionRef());
                    return true;
                }
                foreach (var searchTag in emailAddress.SearchTags)
                {
                    if (searchTag.ToUpper().Contains(searchFor))
                    {
                        Results.Add("Action.EmailAddress.SearchTag", action.AsActionRef());
                    }

                }

            }

            return false;
        }

        private  bool ActionMainSearch(Action action, string searchFor)
        {
            if (action.Label.ToUpper().Contains(searchFor))
            {
                Results.Add("Action.Label", action.AsActionRef());
                return true;
            }
            foreach (var searchTag in action.SearchTags)
            {
                if (searchTag.ToUpper().Contains(searchFor))
                {
                    Results.Add("Action.SearchTag", action.AsActionRef());
                    return true;
                }
            }

            foreach (var note in action.Notes)
            {
                if (note.Notes.ToUpper().Contains(searchFor))
                {
                    Results.Add("Action.Notes",action.AsActionRef());
                    return true;
                }
                foreach (var searchTag in note.SearchTags)
                {
                    if (searchTag.ToUpper().Contains(searchFor))
                    {
                        Results.Add("Action.Notes.SearchTag", action.AsActionRef());
                    }

                }

            }

            return false;
        }

    }
}
