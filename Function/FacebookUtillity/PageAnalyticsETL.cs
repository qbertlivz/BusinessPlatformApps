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
        public static void PopulateSingleValues(DataTable table, List<JObject> objects)
        {
            foreach (var postPayload in objects)
            {
                foreach (var post in postPayload["data"])
                {
                    foreach (var val in post["values"])
                    {
                        DataRow postRow = table.NewRow();
                        postRow["EndTime"] = val["end_time"];
                        postRow["Name"] = post["name"];
                        postRow["Value"] = val["value"];
                        postRow["Period"] = post["period"];
                        postRow["Title"] = post["title"];
                        postRow["Id"] = post["id"];

                        table.Rows.Add(postRow);
                    }
                }
            }
        }

        public static void PopulateNestedValues(DataTable table, List<JObject> objects)
        {
            foreach (var postPayload in objects)
            {
                foreach (var post in postPayload["data"])
                {
                    foreach (var val in post["values"])
                    {
                        if (val?["value"] != null &&val["value"].Children().Count() > 1)
                        {
                            foreach (var child in val["value"])
                            {
                                var att = child as JProperty;

                                DataRow postRow = table.NewRow();
                                postRow["EndTime"] = val["end_time"];
                                postRow["Name"] = post["name"];
                                postRow["Entry Name"] = att.Name;
                                postRow["Value"] = att.Value;
                                postRow["Period"] = post["period"];
                                postRow["Title"] = post["title"];
                                postRow["Id"] = post["id"];

                                table.Rows.Add(postRow);
                            }
                        }
                        else
                        {
                            DataRow postRow = table.NewRow();
                            postRow["EndTime"] = val["end_time"];
                            postRow["Name"] = post["name"];
                            postRow["Value"] = val["value"];
                            postRow["Period"] = post["period"];
                            postRow["Title"] = post["title"];
                            postRow["Id"] = post["id"];

                            table.Rows.Add(postRow);
                        }
                    }
                }
            }
        }
    }
}
