using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;

namespace AdoGemeenschap
{
    public class StripManager
    {
        private static ConnectionStringSettings conStripSettings = ConfigurationManager.ConnectionStrings["Strips"];
        private static DbProviderFactory factory = DbProviderFactories.GetFactory(conStripSettings.ProviderName);
        
        public DbConnection GetConnection()
        {
            var conStrip = factory.CreateConnection();
            conStrip.ConnectionString = conStripSettings.ConnectionString;
            return conStrip;
        }
    }
}
