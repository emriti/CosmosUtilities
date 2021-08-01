using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosUtilities.BLL.CosmosMonitoring.Models
{
    public class ContainerInfo
    {
        public string ContainerName { get; set; }
        public string PartitionKey { get; set; }
        public long? Count { get; set; }
    }
}
