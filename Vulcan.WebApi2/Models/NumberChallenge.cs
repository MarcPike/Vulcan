namespace Vulcan.WebApi2.Models
{
    public class NumberChallenge
    {
        public string Type { get; set; } = NumberChallengeType.Ignore.ToString();
        public decimal Value { get; set; } = 0;
    }
}