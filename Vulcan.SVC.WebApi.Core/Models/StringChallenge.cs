using Vulcan.WebApi2.Models;

namespace Vulcan.SVC.WebApi.Core.Models
{
    public class StringChallenge
    {
        public string Type { get; set; } = StringChallengeType.Ignore.ToString();
        public string Value { get; set; } = string.Empty;
    }
}