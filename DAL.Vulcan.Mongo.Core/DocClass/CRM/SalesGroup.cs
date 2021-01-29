using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.Core.Queries;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class SalesGroup: BaseDocument
    {
        public string Coid { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IgnoreInVulcan { get; set; } = false;

        public static (List<SalesGroup> Added, List<SalesGroup> modified, List<SalesGroup> DeActivated) UpdateListForCoid(string coid)
        {
            var added = new List<SalesGroup>();
            var modified = new List<SalesGroup>();
            var deActivated = new List<SalesGroup>();

            var rep = new RepositoryBase<SalesGroup>();
            var list = SalesGroupQuery.ExecuteAsync(coid).Result;
            var existingList = rep.AsQueryable().Where(x => x.Coid == coid).ToList();

            // Add new ones
            AddModify(list, existingList, rep, added, modified);

            // Refresh existing list
            DeActivate(coid, rep, list, deActivated);

            return (added, modified, deActivated);
        }

        private static void DeActivate(string coid, RepositoryBase<SalesGroup> rep, List<SalesGroupQuery> list, List<SalesGroup> deActivated)
        {
            var existingList = rep.AsQueryable().Where(x => x.Coid == coid).ToList();

            // Remove if no longer in iMetal
            foreach (var existingSalesGroup in existingList)
            {
                if (!list.Any(x => x.Code == existingSalesGroup.Code && x.Coid == existingSalesGroup.Coid))
                {
                    existingSalesGroup.IsActive = false;
                    rep.Upsert(existingSalesGroup);
                    deActivated.Add(existingSalesGroup);
                }
                else
                {
                    existingSalesGroup.IsActive = true;
                    rep.Upsert(existingSalesGroup);
                }
            }
        }

        private static void AddModify(List<SalesGroupQuery> list, List<SalesGroup> existingList, RepositoryBase<SalesGroup> rep, List<SalesGroup> added, List<SalesGroup> modified)
        {
            foreach (var iMetalSalesGroup in list)
            {
                var matchingSalesGroup = existingList.FirstOrDefault(x => x.Code == iMetalSalesGroup.Code);

                if (matchingSalesGroup == null)
                {
                    matchingSalesGroup = AddSalesGroup(rep, iMetalSalesGroup.Coid, iMetalSalesGroup.Code,
                        iMetalSalesGroup.Description);
                    added.Add(matchingSalesGroup);
                }

                if (matchingSalesGroup.Description != iMetalSalesGroup.Description)
                {
                    matchingSalesGroup.Description = iMetalSalesGroup.Description;
                    modified.Add(matchingSalesGroup);
                    rep.Upsert(matchingSalesGroup);
                }
            }
        }

        private static SalesGroup AddSalesGroup(RepositoryBase<SalesGroup> rep, string coid, string code, string description)
        {
            return rep.Upsert(new SalesGroup()
            {
                Coid = coid,
                Code = code,
                Description = description
            });

        }
    }
}
