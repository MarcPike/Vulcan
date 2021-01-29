namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class QuoteQueryRange<T>
    {
        public T Min { get; set; } = default(T);
        public T Max { get; set; } = default(T);

        public bool IsUsed => !(Equals(Min, default(T)) && Equals(Max, default(T)));
    }
}