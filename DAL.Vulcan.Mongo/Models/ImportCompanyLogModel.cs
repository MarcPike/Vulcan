using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.Importers;

namespace DAL.Vulcan.Mongo.Models
{
    public class ImportCompanyLogModel
    {
        public string Coid { get; set; }
        public DateTime ExecutedOn { get; set; }
        public bool DebugMode { get; set; }

        public List<CompanyRef> Imported { get; set; }
        public List<CompanyRef> Updated { get; set; }
        public List<CompanyRef> Removed { get; set; }
        public List<CompanyRef> Skipped { get; set; }
        public List<string> SkipReasons { get; set; }

        public ImportCompanyLogModel(CompanyImporter log)
        {
            Coid = log.Coid;
            ExecutedOn = log.ExecutedOn;
            DebugMode = log.DebugMode;
            Imported = log.Imported.Select(x => x.AsCompanyRef()).ToList();
            Updated = log.Updated.Select(x => x.AsCompanyRef()).ToList();
            Removed = log.Removed.Select(x => new CompanyRef() {Coid = x.Location.GetCoid(), Id = x.Id.ToString(), Name = x.Name, Code = x.Code, SqlId = x.SqlId, ShortName = x.ShortName, Branch = x.Branch}).ToList();
            Skipped = log.Skipped.Select(x => x.AsCompanyRef()).ToList();
            SkipReasons = log.SkipReasons;

        }

    }
}
