using Microsoft.Azure.Cosmos;
using System.Collections.Concurrent;

namespace CosmosUtilities.BLL.CosmosMonitoring
{
    public class ConnectionManager
    {
        public static ConcurrentDictionary<string, CosmosClient> dbClients = new ConcurrentDictionary<string, CosmosClient>();
    }
}
