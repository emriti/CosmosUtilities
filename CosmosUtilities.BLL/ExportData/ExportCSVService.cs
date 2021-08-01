using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CosmosUtilities.BLL.ExportData
{
    public class ExportCSVService
    {
        public void ExportCSV(string outputLocation, List<object[]> details, object[] header = null)
        {
            if (details.Count == 0) return;

            StreamWriter sw = new StreamWriter(outputLocation, false, Encoding.UTF8);

            if (header.Length > 0)
            {
                var tmpHeader = header[0];
                for (int i = 1; i < header.Length; i++)
                {
                    tmpHeader += $";{header[i]}";
                }
                sw.WriteLine(tmpHeader);
            }
           
            foreach (var detail in details)
            {
                var tmp = detail[0];
                for (int i = 1; i < detail.Length; i++)
                {
                    tmp += $";{detail[i]}";
                }
                sw.WriteLine(tmp);
            }
        }
    }
}
