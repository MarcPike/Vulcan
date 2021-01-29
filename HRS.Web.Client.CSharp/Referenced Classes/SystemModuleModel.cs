namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class SystemModuleModel
    {
        public SystemModuleTypeRef ModuleType { get; set; }
        public string Id { get; set; }
        public bool View { get; set; } = false;
        public bool Add { get; set; } = false;
        public bool Modify { get; set; } = false;
        public bool Delete { get; set; } = false;
        public bool DirectReportsOnly { get; set; } = true;
        public FileOperations FileOperations { get; set; } = new FileOperations();

    }
}
