using CosmosUtilities.BLL.CosmosMonitoring.Models;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosUtilities.BLL.CosmosMonitoring
{
    public class CosmosHelperService
    {
        private CosmosClient _client;

        public CosmosHelperService(string databaseName, string connString)
        {
            _client = ConnectionManager.dbClients.GetOrAdd(databaseName, new CosmosClient(connString));
        }

        public async Task<List<string>> GetDatabasesName()
        {
            List<string> result = new List<string>();
            var qDef = new QueryDefinition("select * from c");
            var iterator = _client.GetDatabaseQueryStreamIterator(qDef);
            if (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                StreamReader sr = new StreamReader(response?.Content);
                var msg = await sr.ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<DBQueryContent>(msg);
                foreach (var db in data?.Databases)
                {
                    result.Add(db?.Id);
                }
            }
            return result;
        }

        public async Task<int?> GetThroughput (string databaseId)
        {
            var db = _client.GetDatabase(databaseId);
            return await db.ReadThroughputAsync();
        }

        public async Task<List<string>> GetContainersName(string databaseId) 
        {
            List<string> result = new List<string>();
            var db = _client.GetDatabase(databaseId);
            var iterator = db.GetContainerQueryStreamIterator();
            if (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                StreamReader sr = new StreamReader(response?.Content);
                var msg = await sr.ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<CollQueryContent>(msg);
                foreach (var coll in data?.DocumentCollections)
                {
                    result.Add(coll?.Id);
                }
            }
            return result;
        }

        public async Task<long?> CountContainerRows(string databaseId, string containerId)
        {
            long result = 0;
            var coll = _client.GetContainer(databaseId, containerId);
            var qDef = new QueryDefinition("select count(1) as _result from c");
            var iterator = coll.GetItemQueryStreamIterator(qDef);
            if (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                StreamReader sr = new StreamReader(response?.Content);
                var msg = await sr.ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<DocsQueryContent>(msg);

                try
                {
                    result = long.Parse(data?.Documents?.First().Result);
                }
                catch (Exception)
                {
                    result = 0;
                }
            }
            return result;
        }

        public async Task<string> GetPartitionKey(string databaseId, string containerId)
        {
            var coll = _client.GetContainer(databaseId, containerId);
            var qDef = new QueryDefinition("SELECT * FROM c OFFSET 0 LIMIT 1");
            var iterator = coll.GetItemQueryStreamIterator(qDef);
            if (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                StreamReader sr = new StreamReader(response?.Content);
                var msg = await sr.ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(msg);
                try
                {
                    string msgDetail = JsonConvert.SerializeObject(data?.Documents?[0]);
                    var dataDetail = JsonConvert.DeserializeObject<Dictionary<string, string>>(msgDetail);

                    var pks = dataDetail["partitionKey"].Split("/");
                    var result = pks[0];
                    for (int i = 1; i < pks.Length; i++)
                    {
                        result += "/" + dataDetail.Where(p => p.Value == pks[i]).Select(p => p.Key).FirstOrDefault(); 
                    }
                    return result;
                }
                catch (Exception)
                {
                    return "";
                }
            }
            return "";
        }

    }
}
