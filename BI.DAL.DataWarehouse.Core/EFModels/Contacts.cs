using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Contacts
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string Nickname { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public int? TitleId { get; set; }
        public int? BranchId { get; set; }
        public int? PersonnelId { get; set; }
        public int? RelationshipId { get; set; }
        public int? CompanyId { get; set; }
        public int? TypeId { get; set; }
        public int? TerritoryId { get; set; }
        public string Telephone { get; set; }
        public string FastDial { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public int? ContactFrequency { get; set; }
        public DateTime? LastContactDate { get; set; }
        public bool? AcceptMail { get; set; }
        public bool? AcceptEmail { get; set; }
        public bool? AcceptFax { get; set; }
        public bool? AcceptCalls { get; set; }
        public bool? AcceptVisits { get; set; }
        public string GeneralNote { get; set; }
        public string ProductsNote { get; set; }
        public string PersonalNote { get; set; }
        public string InterestsNote { get; set; }
        public string EmploymentNote { get; set; }
        public string Categories { get; set; }
        public string Idxfti { get; set; }
        public int? StatusId { get; set; }
        public int? AddressId { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
