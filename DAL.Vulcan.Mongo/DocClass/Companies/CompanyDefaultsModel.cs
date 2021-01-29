using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.Vulcan.Mongo.DocClass.Companies
{
    public class CompanyDefaultsModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public string Coid { get; set; }
        public CompanyRef CompanyRef { get; set; }

        public ContactRef ContactRef { get; set; }

        public Address ShipToAddress { get; set; } 

        public string PaymentTerm { get; set; } = "30 Days From Invoice Date";
        public string FreightTerm { get; set; } = "FCA Free Carrier";
        public string SalesGroupCode { get; set; } = string.Empty;
        public string DisplayCurrency { get; set; } = string.Empty;
        public int ValidityDays { get; set; } = 7;
        [JsonConverter(typeof(StringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)] // Mongo
        public CustomerUom CustomerUom { get; set; } = Models.CustomerUom.Inches;
        public string LeadTime { get; set; } = string.Empty;
        public string SalesPersonNotes { get; set; }

        public string CustomerNotes { get; set; }

        public string ExcelTemplateId { get; set; }

        public CompanyDefaultsModel()
        {
        }

        public CompanyDefaultsModel(string application, string userId, CompanyDefaults companyDefaults)
        {
            var company = new RepositoryBase<Company>().Find(companyDefaults.CompanyId);
            if (company == null) throw new Exception("Company not found");

            LoadFromDatabase(application, userId, company, companyDefaults);

            this.CustomerUom = companyDefaults.CustomerUom;
            this.Application = application;
            this.UserId = userId;
            this.Coid = companyDefaults.Coid;
            this.DisplayCurrency = companyDefaults.DisplayCurrency;
            ExcelTemplateId = companyDefaults.ExcelTemplateId;
        }

        //public CompanyDefaultsModel(string application, string userId, string companyId)
        //{

        //    var company = new RepositoryBase<Company>().Find(companyId);
        //    if (company == null) throw new Exception("Company not found");


        //    var coid = company.Location.GetCoid();
        //    LoadFromDatabase(application, userId, companyId, coid, company);
        //}

        private void LoadFromDatabase(string application, string userId, Company company, CompanyDefaults companyDefaults)
        {
            if (companyDefaults.Coid == null)
            {
                throw new Exception("Company defaults missing a Coid");
            }

            Application = application;
            UserId = userId;


            Id = companyDefaults.Id.ToString();

            var repContacts = new RepositoryBase<Contact>();
            if (!String.IsNullOrEmpty(companyDefaults.ContactId))
            {
                var contact = repContacts.Find(companyDefaults.ContactId);
                if (contact != null) ContactRef = contact.AsContactRef();
            }

            var companyId = company.Id.ToString();

            CompanyRef = company.AsCompanyRef();

            //var contactList = repContacts.AsQueryable().Where(x => x.Companies.Any(c => c.Id == companyId)).ToList();
            //ContactList = contactList.Select(x => x.AsContactRef()).ToList();

            if (companyDefaults.ShippingAddressId != Guid.Empty)
            {
                ShipToAddress = company.Addresses.FirstOrDefault(x => x.Id == companyDefaults.ShippingAddressId);
            }

            PaymentTerm = companyDefaults.PaymentTerm;
            FreightTerm = companyDefaults.FreightTerm;
            SalesGroupCode = companyDefaults.SalesGroupCode;
            ValidityDays = companyDefaults.ValidityDays;
            CustomerUom = companyDefaults.CustomerUom;
            LeadTime = companyDefaults.LeadTime;
            DisplayCurrency = companyDefaults.DisplayCurrency;
            CustomerNotes = companyDefaults.CustomerNotes;
            SalesPersonNotes = companyDefaults.SalesPersonNotes;
            ExcelTemplateId = companyDefaults.ExcelTemplateId;

        }

        public void SaveToDatabase()
        {
            var rep = new RepositoryBase<CompanyDefaults>();

            var companyDefaults = rep.Find(ObjectId.Parse(Id));
            if (companyDefaults == null)
            {
                companyDefaults = new CompanyDefaults
                {
                    Coid = Coid,
                    CompanyId = CompanyRef.Id
                };
            }

            companyDefaults.ContactId = ContactRef?.Id;
            companyDefaults.ShippingAddressId = ShipToAddress?.Id;
            companyDefaults.PaymentTerm = PaymentTerm;
            companyDefaults.FreightTerm = FreightTerm;
            companyDefaults.SalesGroupCode = SalesGroupCode;
            companyDefaults.ValidityDays = ValidityDays;
            companyDefaults.CustomerUom = CustomerUom;
            companyDefaults.LeadTime = LeadTime;
            companyDefaults.DisplayCurrency = DisplayCurrency;
            companyDefaults.CustomerNotes = CustomerNotes;
            companyDefaults.SalesPersonNotes = SalesPersonNotes;
            companyDefaults.ExcelTemplateId = ExcelTemplateId;
            rep.Upsert(companyDefaults);
        }

    }

}
