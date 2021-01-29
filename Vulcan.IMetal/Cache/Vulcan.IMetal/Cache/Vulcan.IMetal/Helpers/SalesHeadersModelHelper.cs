using System;
using System.Linq;
using Devart.Data.Linq;
using Vulcan.IMetal.Context;
//using AddressModel = Vulcan.IMetal.Models.AddressModel;
//using CompanyModel = Vulcan.IMetal.Models.CompanyModel;

namespace Vulcan.IMetal.Helpers
{
    //public class SalesHeadersModelHelper: BaseHelper
    //{
    //    public AddressModel GetDeliverToAddress(string coid, VulcanIMetalDataContext context, SalesHeader salesHeader)
    //    {
    //        return (salesHeader.DeliverToAddressId != null)
    //            ? AddressModel.Convert(coid, salesHeader.DeliverToAddressId ?? 0, context)
    //            : new AddressModel();
    //    }

    //    public CompanyModel GetCompanyModel(string coid, VulcanIMetalDataContext context, SalesHeader salesHeader)
    //    {
    //        try
    //        {
    //            return CompanyModel.Convert(coid, salesHeader.Company_CustomerId.Id, context);
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }

    //    public string GetSalesPerson(SalesHeader salesHeader)
    //    {
    //        return salesHeader.Personnel_SalespersonId.Name;
    //    }

    //    public string GetInsideSalesPerson(SalesHeader salesHeader)
    //    {
    //        return salesHeader.Personnel_InsideSalespersonId.Name;
    //    }

    //    public string GetCreateUser(SalesHeader salesHeader, VulcanIMetalDataContext context)
    //    {
    //        var createUser = context.Personnel.SingleOrDefault(x => x.Id == (salesHeader.CuserId ?? 0));
    //        if (createUser != null) return createUser.Name;

    //        return String.Empty;
    //    }

    //    public string GetModifiedUser(SalesHeader salesHeader, VulcanIMetalDataContext context)
    //    {
    //        var modifiedUser = context.Personnel.SingleOrDefault(x => x.Id == (salesHeader.MuserId ?? 0));
    //        if (modifiedUser != null) return modifiedUser.Name;

    //        return String.Empty;
    //    }
    //}
}
