namespace Vulcan.IMetal.Helpers
{
    public interface IHelperCompanyPaymentTerms
    {
        PaymentTermModel GetPaymentTermsForCompany(string coid, int companyId);
    }
}