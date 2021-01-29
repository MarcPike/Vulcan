using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;

namespace DAL.Vulcan.Mongo.Models
{
    public class TestModel //: IModelBinder
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime Birthday { get; set; } = DateTime.Parse("9/29/1983");
        public List<CrmUserRef> CrmUsers { get; set; } = new List<CrmUserRef>();

        public UserRef CreatedByUser { get; set; }


        //public Task BindModelAsync(ModelBindingContext bindingContext)
        //{
        //    if (bindingContext == null)
        //    {
        //        throw new ArgumentNullException(nameof(bindingContext));
        //    }

        //    var values = bindingContext.ActionContext.RouteData.StockItems["CreateByUser"]?.ToString();

        //    return Task.FromResult(0);

        //}
    }

}
