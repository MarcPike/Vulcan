namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class LdapUserRef
    {

        public string ActiveDirectoryId { get; set; }
        public string NetworkId { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public string UserName { get; set; }

        public LocationRef Location { get; set; }


        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

    }
}