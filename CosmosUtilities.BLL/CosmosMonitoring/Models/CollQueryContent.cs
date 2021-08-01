using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosUtilities.BLL.CosmosMonitoring.Models
{
    public class CollQueryContent
    {
        [JsonProperty("_rid")]
        public string Rid { get; set; }

        [JsonProperty("DocumentCollections")]
        public List<CollQueryContentDetail> DocumentCollections { get; set; }
    }

    public class CollQueryContentDetail
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
