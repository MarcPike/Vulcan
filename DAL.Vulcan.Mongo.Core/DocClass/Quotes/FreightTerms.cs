using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.Core.Queries;
using DAL.Vulcan.Mongo.Base.Core.Queries;
using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class FreightTerms: BaseDocument
    {
        public static MongoRawQueryHelper<FreightTerms> Helper = new MongoRawQueryHelper<FreightTerms>();
        public string Coid { get; set; }
        public string Code { get; set; }
        public string Terms { get; set; }

        public static void ImportFreightTerms(string coid)
        {
            var freightTerms = CompanyQuery.GetFreightTerms(coid).Result.ToList();

            foreach (var freightTerm in freightTerms.ToList())
            {
                var crmFreightTerm =
                    Helper.Find(x => x.Coid == coid && x.Code == freightTerm.Code).FirstOrDefault();
                if (crmFreightTerm == null)
                {
                    crmFreightTerm = new FreightTerms()
                    {
                        Coid = coid,
                        Code = freightTerm.Code,
                        Terms = freightTerm.Description
                    };
                }
                else
                {
                    crmFreightTerm.Terms = freightTerm.Description;
                }

                Helper.Upsert(crmFreightTerm);
            }

        }

        public static List<FreightTerms> GetFreightTermsForCoid(string coid)
        {
            return Helper.Find(x => x.Coid == coid).ToList().OrderBy(x => x.Code).ToList();
        }

        public FreightTerms()
        {
        }
    }
}
