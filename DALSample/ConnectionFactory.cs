using System.Configuration;

namespace DALSample
{
    public class ConnectionFactory : IConnectionFactory
    {
        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["cs"].ToString();
        }
    }
}