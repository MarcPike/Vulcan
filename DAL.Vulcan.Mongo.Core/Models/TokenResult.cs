using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class TokenResult
    {
        public string AppName;
        public string Token;
        public List<string> TaskPermissions = new List<string>(); 
        public List<string> ApplicationRoles = new List<string>();
        public bool IsError;

        public TokenResult(string appName)
        {
            AppName = appName;
            Token = "New Token";
            IsError = false;
        }
       
    }
}