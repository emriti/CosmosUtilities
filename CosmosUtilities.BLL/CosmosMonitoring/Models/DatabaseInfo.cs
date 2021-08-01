using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosUtilities.BLL.CosmosMonitoring.Models
{
    public class DatabaseInfo
    {
        public string DatabaseName { get; set; }
        public string DatabasePlan { get; set; }
        public int? DatabaseRU { get; set; }
        public List<ContainerInfo> ListContainerInfo { get; set; }
    }
}
