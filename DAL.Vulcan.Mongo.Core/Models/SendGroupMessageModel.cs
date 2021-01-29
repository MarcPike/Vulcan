namespace Vulcan.WebApi2.Models
{
    public class SendGroupMessageModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string MessageGroupId { get; set; }
        public string Message { get; set; }

    }
}