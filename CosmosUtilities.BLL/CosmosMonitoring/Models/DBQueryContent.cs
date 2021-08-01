using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosUtilities.BLL.CosmosMonitoring.Models
{
    public class DBQueryContent
    {
        [JsonProperty("_rid")]
        public string Rid { get; set; }

        [JsonProperty("Databases")]
        public List<DBQueryContentDetail> Databases { get; set; }
    }

    public class DBQueryContentDetail
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
