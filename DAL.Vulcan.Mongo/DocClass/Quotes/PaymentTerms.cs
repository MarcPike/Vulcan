using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using Vulcan.IMetal.Context;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class PaymentTerms : BaseDocument
    {
        public string Coid { get; set; }
        public string Code { get; set; }
        public string Terms { get; set; }
        public static List<PaymentTerms> GetPaymentTermsForCoid(string coid)
        {

            var context = ContextFactory.GetCompanyContextForCoid(coid);
            var rep = new RepositoryBase<PaymentTerms>();
            var iMetalPaymentTerms = context.Term.Where(x => x.Status == "A").Select(x => new {coid, x.Code, x.Description});
            foreach (var paymentTerm in iMetalPaymentTerms.ToList())
            {
                var crmPaymentTerm =
                    rep.AsQueryable().FirstOrDefault(x => x.Coid == coid && x.Code == paymentTerm.Code);
                if (crmPaymentTerm == null)
                {
                    crmPaymentTerm = new PaymentTerms()
                    {
                        Coid = coid,
                        Code = paymentTerm.Code,
                        Terms = paymentTerm.Description
                    };
                }
                else
                {
                    crmPaymentTerm.Terms = paymentTerm.Description;
                }
                crmPaymentTerm.SaveToDatabase();
            }

            return rep.AsQueryable().Where(x => x.Coid == coid).Select(x=>x).OrderBy(x => x.Code).ToList();
        }

        public PaymentTerms()
        {
        }
    }

}