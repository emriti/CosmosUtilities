using CosmosUtilities.BLL.CosmosMonitoring.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosUtilities.BLL.CosmosMonitoring
{
    public class CosmosPKMonitoringService
    {
        public async Task<List<DatabaseInfo>> GetCosmosPKMonitoringResult(string databaseName, string connString)
        {
            List<DatabaseInfo> result = new List<DatabaseInfo>();
            CosmosHelperService svc = new CosmosHelperService(databaseName, connString);
            var databases = await svc.GetDatabasesName();
            foreach (var db in databases)
            {
                var dbInfo = new DatabaseInfo();
                dbInfo.DatabaseName = db;
                dbInfo.DatabaseRU = await svc.GetThroughput(db);
                dbInfo.DatabasePlan = "";

                var listCollInfo = new List<ContainerInfo>();
                var collections = await svc.GetContainersName(db);
                foreach (var coll in collections)
                {
                    var collInfo = new ContainerInfo();
                    collInfo.ContainerName = coll;
                    var count = await svc.CountContainerRows(db, coll);
                    collInfo.Count = count;
                    collInfo.PartitionKey = await svc.GetPartitionKey(db, coll);
                    var avgDocSize = await svc.GetAverageDocumentSize(db, coll);
                    collInfo.AvgDocSize = avgDocSize > 0 ? avgDocSize : 0;
                    collInfo.TotalContainerSize = (count * avgDocSize) / (1024);
                    listCollInfo.Add(collInfo);
                }
                dbInfo.ListContainerInfo = listCollInfo;
                result.Add(dbInfo);
            }
            return result;
        }
    }
}
