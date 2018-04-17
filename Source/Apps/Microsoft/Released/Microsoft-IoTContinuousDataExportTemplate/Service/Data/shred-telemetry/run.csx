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

    var cs = ConfigurationManager.AppSettings["SQL_CONNECTIONSTRING"];
    using (SqlConnection conn = new SqlConnection(cs))
    {
        conn.Open();

        List<SqlParameter> parameters = new List<SqlParameter>();
        List<string> insertFragments = new List<string>();

        for (int i = 0; i < messages.Count; i++)
        {
            var message = messages[i];

            var deviceIdParam = new SqlParameter($"@deviceId_{i}", SqlDbType.NVarChar, 200);
            deviceIdParam.Value = message.deviceId;
            parameters.Add(deviceIdParam);

            var timestampParam = new SqlParameter($"@timestamp_{i}", SqlDbType.DateTime2);
            timestampParam.Value = message.timestamp;
            parameters.Add(timestampParam);

            int j = 0;
            foreach (KeyValuePair<string, dynamic> entry in message.values)
            {
                var fieldParam = new SqlParameter($"@field_{i}_{j}", SqlDbType.NVarChar, 255);
                fieldParam.Value = entry.Key;
                parameters.Add(fieldParam);

                string valueParamName = $"@value_{i}_{j}";
                string insertFragment;

                switch (entry.Value)
                {
                    case bool _:
                        var boolParam = new SqlParameter(valueParamName, SqlDbType.Bit);
                        boolParam.Value = entry.Value;
                        parameters.Add(boolParam);
                        insertFragment = $"({deviceIdParam.ParameterName}, {timestampParam.ParameterName}, {fieldParam.ParameterName}, null, null, {boolParam.ParameterName})";
                        break;
                    case int _:
                    case Int64 _:
                    case double _:
                    case float _:
                        var numParam = new SqlParameter(valueParamName, SqlDbType.Float);
                        numParam.Value = entry.Value;
                        parameters.Add(numParam);
                        insertFragment = $"({deviceIdParam.ParameterName}, {timestampParam.ParameterName}, {fieldParam.ParameterName}, {numParam.ParameterName}, null, null)";
                        break;
                    case null:
                        insertFragment = $"({deviceIdParam.ParameterName}, {timestampParam.ParameterName}, {fieldParam.ParameterName}, null, null, null)";
                        break;
                    default:
                        var strParam = new SqlParameter(valueParamName, SqlDbType.NVarChar);
                        strParam.Value = entry.Value.ToString();
                        parameters.Add(strParam);
                        insertFragment = $"({deviceIdParam.ParameterName}, {timestampParam.ParameterName}, {fieldParam.ParameterName}, null, {strParam.ParameterName}, null)";
                        break;
                }

                insertFragments.Add(insertFragment);

                j++;
            }
        }

        var query = $"INSERT INTO [stage].[Measurements] (deviceId, timestamp, field, numericValue, stringValue, booleanValue) VALUES {string.Join(", ", insertFragments)}";

        log.Info($"Running query: {query}");

        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddRange(parameters.ToArray());
            var rows = await cmd.ExecuteNonQueryAsync();
            log.Info($"Added {rows} rows to the database");
        }
    }
}

public struct Message
{
    public DateTime timestamp;
    public string deviceId;
    public IDictionary<string, dynamic> values;
}