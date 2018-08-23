using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Transactions;


namespace AdoGemeenschap
{
    public class Bank2DbManager
    {
        private static ConnectionStringSettings conBank2Settings = ConfigurationManager.ConnectionStrings["Bank2"];
        private static DbProviderFactory factory = DbProviderFactories.GetFactory(conBank2Settings.ProviderName);

        public DbConnection getConnection()
        {
            var conBank2 = factory.CreateConnection();
            conBank2.ConnectionString = conBank2Settings.ConnectionString;
            return conBank2;
        }
    }
}
