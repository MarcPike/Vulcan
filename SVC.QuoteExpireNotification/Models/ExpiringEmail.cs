using System.Collections.Generic;

namespace SVC.QuoteExpireNotification.Models
{
    public class ExpiringEmail
    {
        public string Subject { get; set; } = "VulcanCRM: List of Quotes about to Expire tomorrow";

        public string Body { get; set; }

        public List<string> To { get; set; } = new List<string>();
        public string From => "VulcanCrm@Howcogroup.com";

    }

}
