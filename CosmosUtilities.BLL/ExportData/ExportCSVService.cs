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

            using StreamWriter sw = new StreamWriter(outputLocation, false, Encoding.UTF8);
            int batchSize = 10;
            int rowNum = 0;


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
                ++rowNum;
                var tmp = detail[0];
                for (int i = 1; i < detail.Length; i++)
                {
                    var data = "";
                    try
                    {
                        data = $";{detail[i]}";
                    }
                    catch (Exception)
                    {
                        data = $";";
                    }
                    tmp += data;
                }
                sw.WriteLine(tmp);

                if (rowNum % batchSize == 0)
                {
                    sw.Flush();
                    rowNum = 0;
                }
            }

            if (rowNum > 0)
            {
                sw.Flush();
            }
        }
    }
}
