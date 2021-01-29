

using Newtonsoft.Json;

namespace Vulcan.IMetal.ViewFilterObjects
{
    public class RangeFilterBase<TType> : FilterBase
    {

        [JsonIgnore]
        public bool IsActive => EqualUsed || MinUsed || MaxUsed;
        public TType EqualToValue { get; set; }
        public TType MinValue { get; set; }
        public TType MaxValue { get; set; }

        public void EqualTo(TType value)
        {
            EqualToValue = value;
            EqualUsed = true;
            MinUsed = false;
            MaxUsed = false;
        }

        public void Min(TType value)
        {
            if (!EqualUsed)
            {
                MinValue = value;
                MinUsed = true;
            }

        }

        public void Max(TType value)
        {
            if (!EqualUsed)
            {
                MaxValue = value;
                MaxUsed = true;
            }

        }
        public bool MinUsed { get; set; } = false;
        public bool MaxUsed { get; set; } = false;
        public bool EqualUsed { get; set; } = false;

        public void Between(TType min, TType max)
        {
            MinUsed = true;
            MinValue = min;

            MaxUsed = true;
            MaxValue = max;

        }

        public RangeFilterBase()
        {
            Kind = "RangeFilter<" + typeof(TType) + ">";
        }
    }
}