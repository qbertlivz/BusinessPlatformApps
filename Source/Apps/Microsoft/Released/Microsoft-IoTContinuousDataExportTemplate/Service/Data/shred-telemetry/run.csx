#r "System.Configuration"
#r "System.Data"
#r "Newtonsoft.Json"
#r "Microsoft.WindowsAzure.Storage"

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Hadoop.Avro;
using Microsoft.Hadoop.Avro.Container;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

public static async Task Run(CloudBlockBlob myBlob, TraceWriter log)
{
    log.Info($"Processing blob {myBlob.StorageUri}");

    IList<Message> messages = new List<Message>();
    int parseFailCount = 0;

    using (var blobStream = new MemoryStream())
    {
        await myBlob.DownloadToStreamAsync(blobStream);
        blobStream.Position = 0;

        using (var reader = AvroContainer.CreateGenericReader(blobStream))
        {
            while (reader.MoveNext())
            {
                foreach (AvroRecord record in reader.Current.Objects)
                {
                    log.Info(record.ToString());
                    try
                    {
                        var systemProperties = record.GetField<IDictionary<string, object>>("SystemProperties");
                        var deviceId = systemProperties["connectionDeviceId"] as string;
                        var enqueueTime = DateTime.Parse(record.GetField<string>("EnqueuedTimeUtc"));

                        using (var stream = new MemoryStream(record.GetField<byte[]>("Body")))
                        {
                            using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                            {
                                try
                                {
                                    var body = JsonSerializer.Create().Deserialize(streamReader, typeof(IDictionary<string, dynamic>)) as IDictionary<string, dynamic>;
                                    messages.Add(new Message
                                    {
                                        timestamp = enqueueTime,
                                        deviceId = deviceId,
                                        values = body
                                    });
                                }
                                catch (Exception e)
                                {
                                    log.Error($"Failed to process the body for device {deviceId}");
                                    log.Error(e.ToString());
                                    parseFailCount++;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        log.Error("$Failed to process Avro record");
                        log.Error(e.ToString());
                        parseFailCount++;
                    }
                }
            }
        }
    }

    log.Info($"Parsed {messages.Count} messages with {parseFailCount} failures");

    var dataTable = CreateTable();
    foreach (var message in messages)
    {
        foreach (KeyValuePair<string, dynamic> entry in message.values)
        {
            var row = dataTable.NewRow();
            row["deviceId"] = message.deviceId;
            row["timestamp"] = message.timestamp;
            row["field"] = entry.Key;

            switch (entry.Value)
            {
                case bool _:
                    row["booleanValue"] = bool.Parse(entry.Value.ToString());
                    break;

                case int _:
                case Int64 _:
                case double _:
                case float _:
                    row["numericValue"] = decimal.Parse(entry.Value.ToString());
                    break;

                case null:
                    break;

                default:
                    row["stringValue"] = entry.Value.ToString();
                    break;
            }

            dataTable.Rows.Add(row);
        }
    }

    log.Info($"Inserting into table: {dataTable.TableName}");

    var cs = ConfigurationManager.AppSettings["SQL_CONNECTIONSTRING"];
    using (SqlConnection conn = new SqlConnection(cs))
    {
        using (SqlCommand cmd = new SqlCommand("dbo.[InsertMeasurements]", conn) { CommandType = CommandType.StoredProcedure })
        {
            cmd.Parameters.Add(new SqlParameter("@tableType", dataTable));

            conn.Open();
            var rows = await cmd.ExecuteNonQueryAsync();
            log.Info($"Added {rows} rows to the database");
        }
    }
}

private static DataTable CreateTable()
{
    var table = new DataTable("Measurements");
    table.Columns.Add(new DataColumn("deviceId", typeof(string)) { MaxLength = 200 });
    table.Columns.Add(new DataColumn("timestamp", typeof(DateTime)));
    table.Columns.Add(new DataColumn("field", typeof(string)) { MaxLength = 255 });
    table.Columns.Add(new DataColumn("numericValue", typeof(decimal)));
    table.Columns.Add(new DataColumn("stringValue", typeof(string)));
    table.Columns.Add(new DataColumn("booleanValue", typeof(bool)));

    return table;
}

public struct Message
{
    public DateTime timestamp;
    public string deviceId;
    public IDictionary<string, dynamic> values;
}