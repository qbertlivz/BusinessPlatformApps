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


// Process measurements data
public static async Task Run(CloudBlockBlob myBlob, TraceWriter log)
{
    log.Info($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - Processing blob {myBlob.StorageUri}");

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
                    // log.Info($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - {record.ToString()}");
                    try
                    {
                        var messageId = Guid.NewGuid();
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
                                        messageId = messageId,
                                        timestamp = enqueueTime,
                                        deviceId = deviceId,
                                        values = body,
                                        messageSize = (int)stream.Length
                                    });
                                }
                                catch (Exception e)
                                {
                                    log.Error($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - Failed to process the body for device {deviceId}");
                                    log.Error($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - {e.ToString()}");
                                    parseFailCount++;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        log.Error($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - Failed to process Avro record");
                        log.Error($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - {e.ToString()}");
                        parseFailCount++;
                    }
                }
            }
        }
    }

    log.Info($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - Parsed {messages.Count} messages with {parseFailCount} failures");

    var measurementsTable = CreateMeasurementsTable();
    var messagesTable = CreateMessagesTable();
    foreach (var message in messages)
    {
        var messageRow = messagesTable.NewRow();
        messageRow["messageId"] = message.messageId;
        messageRow["deviceId"] = message.deviceId;
        messageRow["timestamp"] = message.timestamp;
        messageRow["size"] = message.messageSize;
        messagesTable.Rows.Add(messageRow);

        foreach (KeyValuePair<string, dynamic> entry in message.values)
        {
            var row = measurementsTable.NewRow();
            row["messageId"] = message.messageId;
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

            measurementsTable.Rows.Add(row);
        }
    }


    var cs = ConfigurationManager.AppSettings["SQL_CONNECTIONSTRING"];
    using (SqlConnection conn = new SqlConnection(cs))
    {
        conn.Open();

        log.Info($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - Inserting into table: {messagesTable.TableName}");
        using (SqlCommand cmd = new SqlCommand("dbo.[InsertMessages]", conn) { CommandType = CommandType.StoredProcedure, CommandTimeout = 60 })
        {
            cmd.Parameters.Add(new SqlParameter("@tableType", messagesTable));

            var stopWatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var rows = await cmd.ExecuteNonQueryAsync();
                stopWatch.Stop();
                log.Info($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - Added {rows} rows to the database table {messagesTable.TableName}. Elapsed: {stopWatch.Elapsed}");
            }
            catch (Exception exception)
            {
                stopWatch.Stop();
                log.Error($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - Elapsed: {stopWatch.Elapsed} - database table {messagesTable.TableName}", exception);
                throw;
            }
        }

        log.Info($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - Inserting into table: {measurementsTable.TableName}");
        using (SqlCommand cmd = new SqlCommand("dbo.[InsertMeasurements]", conn) { CommandType = CommandType.StoredProcedure, CommandTimeout = 60 })
        {
            cmd.Parameters.Add(new SqlParameter("@tableType", measurementsTable));

            var stopWatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var rows = await cmd.ExecuteNonQueryAsync();
                stopWatch.Stop();
                log.Info($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - Added {rows} rows to the database table {measurementsTable.TableName}. Elapsed: {stopWatch.Elapsed}");
            }
            catch (Exception exception)
            {
                stopWatch.Stop();
                log.Error($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")} - Elapsed: {stopWatch.Elapsed} - Database table {measurementsTable.TableName}", exception);
                throw;
            }
        }
    }
}

// The length for the columns matches the length inside database
private static DataTable CreateMeasurementsTable()
{
    var table = new DataTable("Measurements");

    table.Columns.Add(new DataColumn("messageId", typeof(Guid)));
    table.Columns.Add(new DataColumn("deviceId", typeof(string)) { MaxLength = 200 });
    table.Columns.Add(new DataColumn("timestamp", typeof(DateTime)));
    table.Columns.Add(new DataColumn("field", typeof(string)) { MaxLength = 255 });
    table.Columns.Add(new DataColumn("numericValue", typeof(decimal)));
    table.Columns.Add(new DataColumn("stringValue", typeof(string)));
    table.Columns.Add(new DataColumn("booleanValue", typeof(bool)));

    return table;
}

private static DataTable CreateMessagesTable()
{
    var table = new DataTable("Messages");

    table.Columns.Add(new DataColumn("messageId", typeof(Guid)));
    table.Columns.Add(new DataColumn("deviceId", typeof(string)) { MaxLength = 200 });
    table.Columns.Add(new DataColumn("timestamp", typeof(DateTime)));
    table.Columns.Add(new DataColumn("size", typeof(int)));

    return table;
}

public struct Message
{
    public Guid messageId;
    public DateTime timestamp;
    public string deviceId;
    public IDictionary<string, dynamic> values;
    public int messageSize;
}