namespace DAL.Osiris
{
    public struct DocumentSearchSQL
    {
        public string SearchSQL { get; set; }
        public string FieldName { get; set; }

        public bool IsEmpty
        {
            get { return (string.IsNullOrWhiteSpace(SearchSQL) && string.IsNullOrWhiteSpace(FieldName)); }
        }
    }
}