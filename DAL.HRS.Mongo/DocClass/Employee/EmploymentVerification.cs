namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class EmploymentVerification
    {
        public string DocType { get; set; }
        public string IssueDate { get; set; }
        public string Expiration { get; set; }
        public bool Dismissed { get; set; }
        public string File { get; set; }
    }
}