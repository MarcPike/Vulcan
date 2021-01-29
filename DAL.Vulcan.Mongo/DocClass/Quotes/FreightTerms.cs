using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using Vulcan.IMetal.Context;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class FreightTerms: BaseDocument
    {
        public string Coid { get; set; }
        public string Code { get; set; }
        public string Terms { get; set; }

        public static List<FreightTerms> GetFreightTermsForCoid(string coid)
        {
            var context = ContextFactory.GetCompanyContextForCoid(coid);
            var rep = new RepositoryBase<FreightTerms>();
            var iMetalFreightTerms = context.TransportTypeCode.Where(x => x.Status == "A" && x.Type == "S").Select(x => new { coid, x.Code, x.Description });

            foreach (var freightTerm in iMetalFreightTerms.ToList())
            {
                var crmFreightTerm =
                    rep.AsQueryable().FirstOrDefault(x => x.Coid == coid && x.Code == freightTerm.Code);
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
                crmFreightTerm.SaveToDatabase();
            }

            return rep.AsQueryable().Where(x => x.Coid == coid).Select(x => x).OrderBy(x => x.Code).ToList();
        }

        public FreightTerms()
        {
        }
    }
}
