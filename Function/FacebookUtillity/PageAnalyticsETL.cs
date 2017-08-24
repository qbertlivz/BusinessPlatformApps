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
        public static void PopulateNestedValues(DataTable table, List<JObject> objects, string pageId)
        {
            foreach (var obj in objects)
            {
                foreach (var entry in obj["data"])
                {
                    foreach (var val in entry["values"])
                    {
                        if (val?["value"] != null && val["value"].Children().Count() > 1)
                        {
                            foreach (var child in val["value"])
                            {
                                var att = child as JProperty;

                                DataRow row = table.NewRow();
                                if (table.Columns.Contains("EndTime")) { row["EndTime"] = val["end_time"]; }
                                row["Name"] = entry["name"];
                                row["Entry Name"] = att.Name;
                                row["Value"] = att.Value;
                                row["Period"] = entry["period"];
                                row["Title"] = entry["title"];
                                row["Description"] = entry["description"];
                                row["Id"] = entry["id"];
                                row["PageId"] = pageId;
                                table.Rows.Add(row);
                            }
                        }
                        else
                        {
                            DataRow row = table.NewRow();
                            if (table.Columns.Contains("EndTime")) { row["EndTime"] = val["end_time"]; }
                            row["Name"] = entry["name"];
                            row["Value"] = val["value"];
                            row["Period"] = entry["period"];
                            row["Title"] = entry["title"];
                            row["Description"] = entry["description"];
                            row["Id"] = entry["id"];
                            row["PageId"] = pageId;
                            table.Rows.Add(row);
                        }
                    }
                }
            }
        }

        public static void PopulatePostsInfo(DataTable table, List<JObject> objects, string pageId)
        {
            foreach (var obj in objects)
            {
                foreach (var val in obj["data"])
                {
                    if (val?["to"] != null &&
                        (val["to"]["data"] as JArray).Count > 0)
                    {
                        foreach (var t in val["to"]["data"])
                        {
                            DataRow row = table.NewRow();
                            row["Id"] = val["id"];
                            row["Message"] = val["message"];
                            row["Created Time"] = val["created_time"];
                            row["Updated Time"] = val["updated_time"];
                            row["Icon"] = val["icon"];
                            row["Link"] = val["link"];
                            row["Name"] = val["name"];
                            row["Object"] = val["object_id"];
                            row["Permalink URL"] = val["permalink_url"];
                            row["Picture"] = val["picture"];
                            row["Source"] = val["source"] == null ? null : val["source"];
                            row["Shares"] = val["shares"]["count"];
                            row["To Id"] = t["id"];
                            row["To Name"] = t["name"];
                            row["Type"] = val["type"];
                            row["Status Type"] = val["status_type"];
                            row["Is Hidden"] = val["is_hidden"];
                            row["Is Published"] = val["is_published"];
                            row["Story"] = val["story"];
                            table.Rows.Add(row);
                        }

                    }
                }
            }
        }
    }
}
