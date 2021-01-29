using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Console.LdapAuthenticate
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                if (!AttemptLogin()) break;
            }
        }

        private static bool AttemptLogin()
        {
            System.Console.WriteLine("Enter UserName:");
            var userName = System.Console.ReadLine();
            System.Console.WriteLine("Password:");
            var password = System.Console.ReadLine();

            var result = Authenticate(userName, password);

            System.Console.WriteLine("");

            if (result == false) System.Console.WriteLine("Login failed");
            if (result == true) System.Console.WriteLine("Login Successful!");

            System.Console.WriteLine("");
            System.Console.WriteLine("Press [Esc to exit] -or- [Enter to try again]");
            var key = System.Console.ReadKey();

            if (key.Key == ConsoleKey.Escape) return false;

            return true;

        }


        private static bool Authenticate(string userName, string password)
        {
            try
            {
                using (LdapConnection connection = new LdapConnection("howcogroup.com:389"))
                {
                    connection.SessionOptions.ProtocolVersion = 3;
                    connection.SessionOptions.Signing = true;
                    connection.SessionOptions.Sealing = true;
                    //connection.AuthType = AuthType.Kerberos;
                    //connection.SessionOptions.SecureSocketLayer = true;

                    NetworkCredential credential = new NetworkCredential
                    {
                        UserName = userName,
                        Password = password,
                        Domain = "howcogroup.com"
                    };

                    connection.Credential = credential;

                    connection.Bind();
                    connection.Dispose();
                }
            }
            catch (LdapException ex)
            {
                return false;
            }

            return true;
        }

    }
}
