using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosUtilities.BLL.CosmosMonitoring.Models
{
    public class DocsQueryContent
    {
        [JsonProperty("_rid")]
        public string Rid { get; set; }

        [JsonProperty("Documents")]
        public List<DocsQueryContentDetail> Documents { get; set; }
    }

    public class DocsQueryContentDetail
    {
        [JsonProperty("_result")]
        public string Result { get; set; }
    }
}
