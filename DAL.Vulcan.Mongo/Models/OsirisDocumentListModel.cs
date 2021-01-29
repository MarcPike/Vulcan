namespace DAL.Vulcan.Mongo.Models
{
    public class OsirisDocumentListModel : OsirisDocumentListForPortal
    {
        public string Application { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

        public OsirisDocumentListModel()
        {
            
        }
    }
}