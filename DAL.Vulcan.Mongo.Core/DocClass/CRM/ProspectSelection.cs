using DAL.Vulcan.Mongo.Core.DocClass.Companies;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class ProspectSelection
    {
        public string Id
        {
            get
            {
                var result = string.Empty;
                if (Prospect != null)
                    result = Prospect.Id;
                return result;
            }
        }

        public ProspectRef Prospect { get; set; }
        public bool Selected { get; set; }
    }
}