using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UserSyncApi.Helpers
{
    public class DbConnectionFactory
    {
        public static SqlConnection GetConnection(string key)
        {
            var connStr = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(connStr))
            {
                throw new ArgumentException($"No connection string found for key: {key}");
            }

            return new SqlConnection(connStr);
        }

        public static SqlConnection GetDefaultConnection()
        {
            return new SqlConnection(ConfigurationManager.AppSettings["Database1"]);
        }

    }
}