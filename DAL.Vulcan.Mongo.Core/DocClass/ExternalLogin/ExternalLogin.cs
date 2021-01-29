using DAL.Vulcan.Mongo.Base.Core.DocClass;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExternalLogin
{
    public class ExternalLogin : BaseDocument
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
