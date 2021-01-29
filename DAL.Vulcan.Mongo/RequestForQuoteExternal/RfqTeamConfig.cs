using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.RequestForQuoteExternal
{
    public class RfqTeamConfig : BaseDocument
    {
        public TeamRef Team { get; set; }
        public List<string> Keywords { get; set; } = new List<string>();
        public string AcceptText { get; set; } = String.Empty;
        public string RejectText { get; set; } = String.Empty;
    }
}