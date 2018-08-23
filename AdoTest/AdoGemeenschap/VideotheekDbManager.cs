using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoGemeenschap
{
    public class VideotheekDbManager
    {
        private static ConnectionStringSettings conVideoSettings = ConfigurationManager.ConnectionStrings["videoAdo"];
        private static DbProviderFactory factory = DbProviderFactories.GetFactory(conVideoSettings.ProviderName);

        public DbConnection GetConnection()
        {
            var conVideo = factory.CreateConnection();
            conVideo.ConnectionString = conVideoSettings.ConnectionString;
            return conVideo;
        }
    }
}
