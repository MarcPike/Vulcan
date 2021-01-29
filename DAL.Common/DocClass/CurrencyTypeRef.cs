using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Common.DocClass
{
    public class CurrencyTypeRef : ReferenceObject<CurrencyType>
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public CurrencyTypeRef() { }

        public CurrencyTypeRef(CurrencyType c) : base(c)
        {
            Code = c.Code;
            Description = c.Description;
        }

        public CurrencyType AsCurrencyType()
        {
            return ToBaseDocument();
        }
    }
}