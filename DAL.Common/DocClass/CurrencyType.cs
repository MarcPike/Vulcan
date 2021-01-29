using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.Common.DocClass
{
    public class CurrencyType: BaseDocument, ICommonDatabaseObject
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public static List<CurrencyType> GenerateDefaults()
        {
            var rep = new CommonRepository<CurrencyType>();
            
            // US Dollars
            if (!rep.AsQueryable().Any(x => x.Code == "USD"))
            {
                rep.Upsert(new CurrencyType() {Code = "USD", Description = "US Dollars"});
            }

            // GBP
            if (!rep.AsQueryable().Any(x => x.Code == "GBP"))
            {
                rep.Upsert(new CurrencyType() { Code = "GBP", Description = "Great Britain Pounds" });
            }

            // Malaysian Ringgit
            if (!rep.AsQueryable().Any(x => x.Code == "EUR"))
            {
                rep.Upsert(new CurrencyType() { Code = "EUR", Description = "Euros" });
            }

            // Canadian Dollars
            if (!rep.AsQueryable().Any(x => x.Code == "CAD"))
            {
                rep.Upsert(new CurrencyType() { Code = "CAD", Description = "Canadian Dollars" });
            }

            // Chinese Yen
            if (!rep.AsQueryable().Any(x => x.Code == "CNY"))
            {
                rep.Upsert(new CurrencyType() { Code = "CNY", Description = "Chinese Yen" });
            }

            // United Arab Emirates Dirham
            if (!rep.AsQueryable().Any(x => x.Code == "AED"))
            {
                rep.Upsert(new CurrencyType() { Code = "AED", Description = "United Arab Emirates Dirham" });
            }

            // Norwegian krone
            if (!rep.AsQueryable().Any(x => x.Code == "NOK"))
            {
                rep.Upsert(new CurrencyType() { Code = "NOK", Description = "Norwegian krones" });
            }

            // Norwegian krone
            if (!rep.AsQueryable().Any(x => x.Code == "SEK"))
            {
                rep.Upsert(new CurrencyType() { Code = "SEK", Description = "Swedish Kronas" });
            }

            // Malaysian Ringgit
            if (!rep.AsQueryable().Any(x => x.Code == "MYR"))
            {
                rep.Upsert(new CurrencyType() { Code = "MYR", Description = "Malaysian Ringgits" });
            }

            // Malaysian Ringgit
            if (!rep.AsQueryable().Any(x => x.Code == "SGD"))
            {
                rep.Upsert(new CurrencyType() { Code = "SGD", Description = "Singapore Dollars" });
            }

            return rep.AsQueryable().ToList();
        }

        public static CurrencyType GetCurrencyTypeFor(string code)
        {
            var rep = new CommonRepository<CurrencyType>();
            return rep.AsQueryable().SingleOrDefault(x => x.Code == code);
        }

        public static CurrencyTypeRef GetCurrencyTypeRefFor(string code)
        {
            var currencyType = GetCurrencyTypeFor(code);
            return currencyType?.AsCurrencyTypeRef();
        }

        public static List<CurrencyType> GetDefaults()
        {
            return GenerateDefaults();
        }

        public CurrencyTypeRef AsCurrencyTypeRef()
        {
            return new CurrencyTypeRef(this);
        }
    }
}