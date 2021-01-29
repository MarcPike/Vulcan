using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;

namespace DAL.HRS.Mongo.Models
{
    public class NewHrsUserModel
    {
        public EmployeeRef Employee { get; set; }
        public LdapUserRef LdapUser { get; set; }

        public NewHrsUserModel()
        {
        }

        public NewHrsUserModel(EmployeeRef employee, LdapUserRef ldapUser)
        {
            Employee = employee;
            LdapUser = ldapUser;
        }
    }
}
