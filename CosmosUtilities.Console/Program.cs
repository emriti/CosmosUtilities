using CosmosUtilities.BLL.CosmosMonitoring;
using CosmosUtilities.BLL.ExportData;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace CosmosUtilities.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var dbName = ConfigurationManager.AppSettings["DB_NAME"];
            var dbConn = ConfigurationManager.ConnectionStrings["DB_CONN"].ConnectionString;
            //await Test(dbName, dbConn);
            await GenerateCSV(dbName, dbConn);
        }

        private static async Task Test(string dbName, string dbConn)
        {
            CosmosHelperService svc = new CosmosHelperService(dbName, dbConn);
            //var a = await svc.GetDatabasesName();
            //await svc.GetContainersName("Forum");
            //await svc.CountContainerRows("Forum", "FeedPost");
            //var b = await svc.GetPartitionKey("Course", "ClassSession");
            //var b = await svc.GetPartitionKey("Course", "GroupMember");
            //var a = await svc.GetAverageSize("Course", "ClassSession");
        }

        private static async Task GenerateCSV(string dbName, string dbConn)
        {
            CosmosPKMonitoringService svc = new CosmosPKMonitoringService();
            var databaseInfos = await svc.GetCosmosPKMonitoringResult(dbName, dbConn);

            object[] header = new object[] { "No", "Database Name", "Database RU", "Container Name", "Partition Key", "Avg Doc Size (Byte)", "Count", "Total Container Size (KB)" };
            var i = 0;
            List<object[]> details = new List<object[]>();
            foreach (var databaseInfo in databaseInfos)
            {
                foreach (var containerInfo in databaseInfo.ListContainerInfo)
                {
                    var detail = new object[] { ++i, databaseInfo.DatabaseName, databaseInfo.DatabaseRU, containerInfo.ContainerName,
                        containerInfo.PartitionKey, containerInfo.AvgDocSize, containerInfo.Count ,containerInfo.TotalContainerSize };
                    details.Add(detail);
                }
            }

            ExportCSVService exportSvc = new ExportCSVService();
            exportSvc.ExportCSV("C:\\Users\\emriti\\Documents\\info_prod_20200804_v2.csv", details, header);
        }
    }
}
