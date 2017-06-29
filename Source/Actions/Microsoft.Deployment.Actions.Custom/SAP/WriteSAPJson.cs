using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model;

namespace Microsoft.Deployment.Actions.Custom.SAP
{
    [Export(typeof(IAction))]
    public class WriteSAPJson : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string rowBatchSize = request.DataStore.GetValue("RowBatchSize");
            string sapClient = request.DataStore.GetValue("SapClient");
            string sapHost = request.DataStore.GetValue("SapHost");
            string sapLanguage = request.DataStore.GetValue("SapLanguage");
            string sapRouterString = request.DataStore.GetValue("SapRouterString");
            string sapSystemId = request.DataStore.GetValue("SapSystemId");
            string sapSystemNumber = request.DataStore.GetValue("SapSystemNumber");

            var sqlConnectionStrings = request.DataStore.GetAllValues("SqlConnectionString");
            string sqlConnectionString = sqlConnectionStrings != null ? sqlConnectionStrings[0] : string.Empty;
            if (!string.IsNullOrEmpty(sqlConnectionString))
            {
                SqlCredentials sqlCredentials = SqlUtility.GetSqlCredentialsFromConnectionString(sqlConnectionString);
                sqlCredentials.Username = string.Empty;
                sqlCredentials.Password = string.Empty;
                sqlConnectionString = SqlUtility.GetConnectionString(sqlCredentials);
            }

            string jsonDestination = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), JSON_PATH);
            (new FileInfo(jsonDestination)).Directory.Create();

            JObject config = new JObject(
                new JProperty("RowBatchSize", rowBatchSize),
                new JProperty("SapClient", sapClient),
                new JProperty("SapHost", sapHost),
                new JProperty("SapLanguage", sapLanguage),
                new JProperty("SapRouterString", sapRouterString),
                new JProperty("SapSystemId", sapSystemId),
                new JProperty("SapSystemNumber", sapSystemNumber),
                new JProperty("SqlConnectionString", sqlConnectionString)
            );

            using (StreamWriter file = File.CreateText(jsonDestination))
            { 
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    config.WriteTo(writer);
                }
            }

            return new ActionResponse(ActionStatus.Success);
        }

        private const string JSON_PATH = @"Simplement, Inc\Solution Template AR\config.json";
    }
}