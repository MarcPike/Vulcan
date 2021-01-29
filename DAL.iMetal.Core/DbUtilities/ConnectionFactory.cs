using Npgsql;
using System.Collections.Generic;
using System.Linq;

namespace DAL.iMetal.Core.Context
{
    public static class ConnectionFactory
    {

        public static bool IsMssQc { get; set; } = false;

        private static  Dictionary<string, string> ConnectionStrings = new Dictionary<string, string>();
        public static void Initialize()
        {
            if (IsMssQc)
            {
                ConnectionStrings.Add("INC", "Host=s-us-imetalus;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Keepalive=10000;");
                ConnectionStrings.Add("DUB", "Host=s-us-imetaldu;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Keepalive=10000;");
                ConnectionStrings.Add("MSA", "Host=s-us-imetalma;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Keepalive=10000;");
                ConnectionStrings.Add("SIN", "Host=10.5.20.20;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Keepalive=10000;");
                ConnectionStrings.Add("EUR", "Host=s-us-imetaleu;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Keepalive=10000;");
                ConnectionStrings.Add("CAN", "Host=172.30.48.48;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Keepalive=10000;");
            }
            else
            {
                ConnectionStrings.Add("INC", "Host=s-us-imetalus;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Keepalive=10000;");
                ConnectionStrings.Add("DUB", "Host=s-us-imetaldu;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Keepalive=10000;");
                ConnectionStrings.Add("MSA", "Host=s-us-imetalma;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Keepalive=10000;");
                ConnectionStrings.Add("SIN", "Host=10.5.20.20;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Keepalive=10000;");
                ConnectionStrings.Add("EUR", "Host=s-us-imetaleu;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Keepalive=10000;");
                ConnectionStrings.Add("CAN", "Host=172.30.48.48;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Keepalive=10000;");
            }

        }

        public static NpgsqlConnection GetConnection(string coid)
        {
            var connectionString = ConnectionStrings.Single(x => x.Key == coid).Value;
            return new NpgsqlConnection(connectionString);
        }

    }

}
