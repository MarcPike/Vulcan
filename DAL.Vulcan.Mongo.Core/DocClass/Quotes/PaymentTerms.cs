using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.Core.Queries;
using DAL.Vulcan.Mongo.Base.Core.Queries;
using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class PaymentTerms : BaseDocument
    {
        public static MongoRawQueryHelper<PaymentTerms> Helper = new MongoRawQueryHelper<PaymentTerms>();
        public string Coid { get; set; }
        public string Code { get; set; }
        public string Terms { get; set; }

        public static void ImportPaymentTerms(string coid)
        {
            var iMetalPaymentTerms = CompanyQuery.GetPaymentTerms(coid).Result;
            var rep = new RepositoryBase<PaymentTerms>();
            foreach (var paymentTerm in iMetalPaymentTerms.ToList())
            {
                var crmPaymentTerm =
                    Helper.Find(x => x.Coid == coid && x.Code == paymentTerm.Code).FirstOrDefault();
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

                Helper.Upsert(crmPaymentTerm);
            }

        }
        public static List<PaymentTerms> GetPaymentTermsForCoid(string coid)
        {
            return Helper.Find(x => x.Coid == coid).ToList().OrderBy(x => x.Code).ToList();
        }

        public PaymentTerms()
        {
        }
    }

}