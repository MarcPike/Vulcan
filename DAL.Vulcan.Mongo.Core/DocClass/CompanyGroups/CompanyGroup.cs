using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.DocClass.CompanyGroups
{
    public class CompanyGroup : BaseDocument
    {
        public string Name { get; set; }
        public bool IsGlobal { get; set; }
        public bool IsAnalytical { get; set; }
        public List<ObjectId> ChildGroupsIds { get; set; } = new List<ObjectId>();
        public ReferenceList<Company,CompanyRef> Companies { get; set; } = new ReferenceList<Company, CompanyRef>();
        public string CreatedBy { get; set; } = "Administrator";
        public ObjectId ParentObjectId { get; set; } = ObjectId.Empty;
        public string Branch { get; set; } = string.Empty;
        public string NameContains { get; set; } = string.Empty;
        public bool IsAlliance { get; set; } = false;

        public List<Team> Teams => new LinkResolver<Team>(this).GetAllLinkedDocuments().ToList();

        public List<CompanyGroupUpdate> CompanyGroupUpdates { get; set; } = new List<CompanyGroupUpdate>();

        public List<CompanyRef> RefreshAllianceCompanyList(RepositoryBase<CompanyGroup> repCompanyGroup)
        {
            var result = new List<CompanyRef>();
            if ((!IsAlliance) || (NameContains == String.Empty) || (Branch == String.Empty)) return result;

            var companyGroupUpdate = new CompanyGroupUpdate();

            var repCompany = new RepositoryBase<Company>();
            var companies = repCompany.AsQueryable()
                .Where(x => x.Location.Branch == Branch && x.Name.Contains(NameContains))
                .ToList();

            var addedRow = false;
            foreach (var company in companies)
            {
                if (Companies.All(x => x.SqlId != company.SqlId))
                {
                    if (!company.IsAlliance)
                    {
                        company.IsAlliance = true;
                        repCompany.Upsert(company);
                    }
                    var companyRef = company.AsCompanyRef();
                    Companies.Add(companyRef);
                    companyGroupUpdate.AddedCompanies.Add(companyRef);
                    addedRow = true;
                } else if (!company.IsAlliance)
                {
                    company.IsAlliance = true;
                    repCompany.Upsert(company);
                }

                var count = Companies.Count(x => x.SqlId == company.SqlId);
                if (count > 1)
                {
                    while (count != 1)
                    {
                        var removeCompany = Companies.FirstOrDefault(x => x.SqlId == company.SqlId);
                        Companies.Remove(removeCompany);
                        count--;
                    }
                }

            }
            if (addedRow)
            {
                CompanyGroupUpdates.Add(companyGroupUpdate);
                repCompanyGroup.Upsert(this);

                result.AddRange(companyGroupUpdate.AddedCompanies);
            }

            return result;
        }

        public List<CompanyRef> RefreshNonAllianceCompanyList(RepositoryBase<CompanyGroup> repCompanyGroup)
        {
            var result = new List<CompanyRef>();

            if ((IsAlliance) || (Branch == String.Empty)) return result;

            var repCompany = new RepositoryBase<Company>();
            var companies = repCompany.AsQueryable()
                .Where(x => x.Location.Branch == Branch)
                .ToList();

            var companyGroupUpdate = new CompanyGroupUpdate();

            var updatePerformed = false;
            foreach (var company in companies)
            {

                if (company.IsAlliance)  
                {
                    var removeOne = Companies.SingleOrDefault(x => x.SqlId == company.SqlId);
                    if (removeOne != null)
                    {
                        Companies.Remove(removeOne);
                        updatePerformed = true;
                    }
                    continue;
                }

                if (Companies.All(x => x.SqlId != company.SqlId))
                {
                    var companyRef = company.AsCompanyRef();
                    Companies.Add(companyRef);
                    companyGroupUpdate.AddedCompanies.Add(companyRef);
                    updatePerformed = true;
                }

                var count = Companies.Count(x => x.SqlId == company.SqlId);
                if (count > 1)
                {
                    while (count != 1)
                    {
                        var removeCompany = Companies.FirstOrDefault(x => x.SqlId == company.SqlId);
                        Companies.Remove(removeCompany);
                        updatePerformed = true;
                        count--;
                    }
                }

            }

            if (updatePerformed)
            {
                CompanyGroupUpdates.Add(companyGroupUpdate);
                repCompanyGroup.Upsert(this);
                result.AddRange(companyGroupUpdate.AddedCompanies);
            }

            return result;
        }

        [BsonIgnore]
        public CompanyGroup GetParent
        {
            get
            {
                var repository = new RepositoryBase<CompanyGroup>();
                return repository.Collection.AsQueryable().SingleOrDefault(x => x.Id == ParentObjectId);
            }
        }

        public List<CompanyGroup> GetChildGroups()
        {
            if (ChildGroupsIds.Count == 0) return new List<CompanyGroup>();
            var repository = new RepositoryBase<CompanyGroup>();
            var result = new List<CompanyGroup>();
            foreach (var childGroupId in ChildGroupsIds)
            {
                var childGroup = CompanyGroup.Find(childGroupId);
                if (childGroup != null) result.Add(childGroup);
            }
            return result;
        }

        public List<CompanyGroup> GetAllChildGroups()
        {
            if (ChildGroupsIds.Count == 0) return new List<CompanyGroup>();
            var result = new List<CompanyGroup>();
            var childGroups = GetChildGroups();
            result.AddRange(childGroups);
            foreach (var childGroup in childGroups)
            {
                result.AddRange(childGroup.GetChildGroups());
            }
            return result;
        }

        public void AddCompanyReference(CompanyRef compRef)
        {
            if (Companies.All(x => x.Id != compRef.Id))
            {
                Companies.Add(compRef);
                Save();
            }
        }

        [BsonIgnore]
        public CompanyTreeNode AsTreeNode
        {
            get
            {
                var node = new CompanyTreeNode()
                {
                    Id = Id.ToString(),
                    Label = Name,
                    Companies = Companies.ToList(),
                    Data = Id.ToString(),
                    IsAnalytical = IsAnalytical,
                    IsGlobal = IsGlobal
                };
                if (ChildGroupsIds.Any())
                {
                    foreach (var childGroup in GetChildGroups().OrderBy(x => x.Name).ToList())
                    {
                        var asNode = childGroup.AsTreeNode;
                        if (asNode != null)
                            node.Children.Add(asNode);
                    }
                }
                return node;
            }
        }

        public static CompanyGroup Find(ObjectId id)
        {
            var repository = new RepositoryBase<CompanyGroup>();
            return repository.Collection.AsQueryable().SingleOrDefault(x => x.Id == id);
        }

        public static CompanyGroup Find(string id)
        {
            var objectId = ObjectId.Parse(id);
            return Find(objectId);
        }

        public void Save()
        {
            SaveToDatabase();
        }

        public void AddChild(CompanyGroup childGroup)
        {
            var repository = new RepositoryBase<CompanyGroup>();
            ChildGroupsIds.Add(childGroup.Id);
            repository.Upsert(this);
            childGroup.ParentObjectId = Id;
            repository.Upsert(childGroup);
        }

        public void RemoveChild(ObjectId childId)
        {
            ChildGroupsIds.Remove(childId);
            Save();
        }

        public CompanyGroup FindParentFor(ObjectId id)
        {
            return GetChildGroups().Any(x => x.Id == id) ?
                this : GetChildGroups().FirstOrDefault(childGroup => childGroup.FindParentFor(id) != null);
        }

        public string GetPath()
        {
            try
            {
                var repository = new RepositoryBase<CompanyGroup>();
                var path = Name;
                var currentGroup = this;
                while ((currentGroup != null) && (currentGroup.ParentObjectId != ObjectId.Empty))
                {
                    currentGroup = repository.Collection.AsQueryable().SingleOrDefault(x => x.Id == currentGroup.ParentObjectId);
                    if (currentGroup != null)
                        path = currentGroup.Name + " / " + path;
                }
                return path;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }

        public List<CompanyRef> GetAllCompanies()
        {
            var result = new List<CompanyRef>();

            try
            {
                var repository = new RepositoryBase<CompanyGroup>();
                var path = GetPath();
                var companies = Companies;
                //foreach (var companyReference in companies)
                //{
                //    companyReference.Path = path;
                //}
                result.AddRange(Companies);

                foreach (var childGroupsId in ChildGroupsIds)
                {
                    var childGroup = repository.Find(childGroupsId);
                    if (childGroup != null)
                    {
                        result.AddRange(childGroup.GetAllCompanies());
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
            return result;
        }

        public CompanyGroupRef AsCompanyGroupRef()
        {
            return new CompanyGroupRef(this);
        }

        //public static List<CompanyGroupSearchResult> CompanySearchByCoidCode(string coid, string code)
        //{
        //    var companyView = new CompanyView(coid);
        //    companyView

        //    var compRep = new RepositoryBase<CompanyRef>();
        //    var compGroupRep = RepositoryBase<CompanyGroup>();
        //    var result = new List<CompanyGroupSearchResult>();

        //    var company = compRep.AsQueryable().FirstOrDefault(x => x.Ssid == coid && x.Code == code);
        //    if (company == null) return result;
        //    var groupsWithCompany = compGroupRep.AsQueryable().Where(x => x.Companies.Any(c => c.ObjectId == company.ObjectId));
        //    foreach (var companyGroup in groupsWithCompany)
        //    {
        //        result.Add(new CompanyGroupSearchResult()
        //        {
        //            Company = company.AsCompanyRef(),
        //            CompanyGroup = companyGroup.AsCompanyGroupRef(),
        //            Path = companyGroup.GetPath()
        //        });
        //    }

        //    return result;
        //}
    }
}
