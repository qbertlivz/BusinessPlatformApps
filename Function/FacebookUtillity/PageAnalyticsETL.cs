using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookUtillity
{
    public class PageAnalyticsETL
    {
        public static void PopulateSingleValues(DataTable table, List<JObject> objects, string pageId)
        {
            foreach (var obj in objects)
            {
                foreach (var entry in obj["data"])
                {
                    foreach (var val in entry["values"])
                    {
                        DataRow row = table.NewRow();
                        row["EndTime"] = val["end_time"];
                        row["Name"] = entry["name"];
                        row["Value"] = val["value"];
                        row["Period"] = entry["period"];
                        row["Title"] = entry["title"];
                        row["Id"] = entry["id"];
                        row["PageId"] = pageId;
                        table.Rows.Add(row);
                    }
                }
            }
        }

        public static void PopulateNestedValues(DataTable table, List<JObject> objects, string pageId)
        {
            foreach (var obj in objects)
            {
                foreach (var entry in obj["data"])
                {
                    foreach (var val in entry["values"])
                    {
                        if (val?["value"] != null &&val["value"].Children().Count() > 1)
                        {
                            foreach (var child in val["value"])
                            {
                                var att = child as JProperty;

                                DataRow row = table.NewRow();
                                if(table.Columns.Contains("EndTime")) { row["EndTime"] = val["end_time"]; }                                
                                row["Name"] = entry["name"];
                                row["Entry Name"] = att.Name;
                                row["Value"] = att.Value;
                                row["Period"] = entry["period"];
                                row["Title"] = entry["title"];
                                row["Id"] = entry["id"];
                                row["PageId"] = pageId;
                                table.Rows.Add(row);
                            }
                        }
                        else
                        {
                            DataRow row = table.NewRow();
                            row["EndTime"] = val["end_time"];
                            row["Name"] = entry["name"];
                            row["Value"] = val["value"];
                            row["Period"] = entry["period"];
                            row["Title"] = entry["title"];
                            row["Id"] = entry["id"];
                            row["PageId"] = pageId;
                            table.Rows.Add(row);
                        }
                    }
                }
            }
        }
    }
}
