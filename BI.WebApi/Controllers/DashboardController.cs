using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BI.WebApi.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class DashboardController : Controller
    {
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("dashboard/GetAvailableDashboardList")]
        //public JsonResult GetAvailableDashboardList()
        //{
        //    dynamic result = new ExpandoObject();
        //    try
        //    {

        //        result.DashboardList = new List<string>()
        //        {
        //            "Sales", "Inventory"
        //        };

        //        //_helperUser.ControllerMethodCalled(application, userId, "ActionController", "GetActionScheduleModel");

        //    }
        //    catch (Exception e)
        //    {
        //        result.ErrorMessage = e.Message;
        //        SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
        //    }
        //    return new JsonResult(result);

        //}
    }
}
