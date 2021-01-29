using Devart.Data.Linq.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Helpers;

namespace Vulcan.IMetal.Models
{
    //public class SalesHeadersModel: BaseModel<SalesHeadersModelHelper>
    //{
    //    public string Coid { get; set; }
    //    public int Id { get; set; } 
    //    public string CustomerOrderNumber { get; set; }
    //    public AddressModel DeliverToAddress { get; set; }

    //    public DateTime FilterDueDate { get; set; } = DateTime.UtcNow;
    //    public DateTime FilterCreateDate { get; set; } = DateTime.UtcNow;
    //    public DateTime? SaleDate { get; set; } = DateTime.UtcNow;
    //    public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;
    //    public Vulcan.IMetal.Models.CompanyModel Company { get; set; }
    //    public string CustomerNameOverride { get; set; }
    //    public int Number { get; set; }
    //    public string CreateUser { get; set; }
    //    public string ModifyUser { get; set; }
    //    public string SalesPerson { get; set; }
    //    public string InsideSalesPerson { get; set; }

    //    public decimal TotalCost => SalesItems.Sum(x => x.TotalCost);
    //    public decimal TotalPrice => SalesItems.Sum(x => x.TotalPrice);
    //    public decimal TotalMargin => SalesItems.Sum(x => x.Margin);
    //    public decimal TotalWeight => SalesItems.Sum(x => x.RequiredWeight);
    //    public decimal TotalLength => SalesItems.Sum(x => x.RequiredLength);

    //    public List<SalesItemsModel> SalesItems;

    //    public static SalesHeadersModel Convert(string coid, int salesHeaderId, VulcanIMetalDataContext context, LinqMonitor monitor)
    //    {
    //        var salesHeader = context.SalesHeader.First(x => x.Id == salesHeaderId);
    //        var deliverTo = Helper.GetDeliverToAddress(coid, context, salesHeader);

            

    //        var salesHeaderModel = new SalesHeadersModel()
    //        {
    //            Coid = coid,
    //            Number = salesHeader.Number ?? 0,
    //            Company = Helper.GetCompanyModel(coid, context, salesHeader),
    //            Id = salesHeader.Id,
    //            FilterCreateDate = salesHeader.Cdate ?? DateTime.UtcNow,
    //            ModifiedDate = salesHeader.Mdate,
    //            SaleDate = salesHeader.SaleDate,
    //            CustomerOrderNumber = salesHeader.CustomerOrderNumber,
    //            DeliverToAddress = deliverTo,
    //            FilterDueDate = salesHeader.FilterDueDate ?? DateTime.Parse("1/1/1980"),
    //            CustomerNameOverride = salesHeader.CustomerNameOverride,
    //            SalesPerson = Helper.GetSalesPerson(salesHeader),
    //            InsideSalesPerson = Helper.GetInsideSalesPerson(salesHeader),
    //            CreateUser = Helper.GetCreateUser(salesHeader, context),
    //            ModifyUser = Helper.GetModifiedUser(salesHeader, context)

                
    //        };
    //        //monitor.IsActive = true;
    //        var salesItems = SalesItemsModel.Convert(salesHeaderId, context, coid);
    //        //monitor.IsActive = false;
    //        if (salesItems != null)
    //            salesHeaderModel.SalesItems = salesItems;
    //        else
    //        {
    //            salesHeaderModel.SalesItems = new List<SalesItemsModel>();
    //        }

    //        return salesHeaderModel;
    //    }

    //}
}
