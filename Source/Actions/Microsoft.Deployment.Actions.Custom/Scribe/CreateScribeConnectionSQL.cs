using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Scribe;

namespace Microsoft.Deployment.Actions.Custom.Scribe
{
    [Export(typeof(IAction))]
    public class CreateScribeConnectionSql : BaseAction
    {
        private const string CONNECTOR_ID = "AC103458-FCB6-41D3-94A0-43D25B4F4FF4";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetValue("ScribeUsername"), request.DataStore.GetValue("ScribePassword"));

            string apiToken = request.DataStore.GetValue("ScribeApiToken");
            string orgId = request.DataStore.GetValue("ScribeOrganizationId");

            await ScribeUtility.InstallConnector(rc, orgId, CONNECTOR_ID);

            ScribeConnection connection = new ScribeConnection
            {
                ConnectorId = CONNECTOR_ID,
                Color = "#FFEA69A6",
                Name = ScribeUtility.BPST_TARGET_NAME,
                Properties = new List<ScribeKeyValue>()
            };

            // Set authentication
            ScribeKeyValue kvp = new ScribeKeyValue
            {
                Key = "WindowsAuthentication",
                Value = request.DataStore.GetJson("SqlCredentials", "AuthType").EqualsIgnoreCase("Windows") ? ScribeUtility.AesEncrypt(apiToken, "true") : ScribeUtility.AesEncrypt(apiToken, "false")
            };
            connection.Properties.Add(kvp);
            // Set server name
            kvp = new ScribeKeyValue { Key = "Server", Value = ScribeUtility.AesEncrypt(apiToken, request.DataStore.GetValue("Server")) };
            connection.Properties.Add(kvp);
            // Set database
            kvp = new ScribeKeyValue { Key = "Database", Value = ScribeUtility.AesEncrypt(apiToken, request.DataStore.GetValue("Database")) };
            connection.Properties.Add(kvp);
            // Set user name
            kvp = new ScribeKeyValue { Key = "UserName", Value = ScribeUtility.AesEncrypt(apiToken, request.DataStore.GetValue("Username")) };
            connection.Properties.Add(kvp);
            // Set password
            kvp = new ScribeKeyValue { Key = "Password", Value = ScribeUtility.AesEncrypt(apiToken, request.DataStore.GetValue("Password")) };
            connection.Properties.Add(kvp);

            await rc.Post(string.Format(ScribeUtility.URL_CONNECTIONS, orgId), JsonUtility.Serialize(connection), null);

            return new ActionResponse(ActionStatus.Success);
        }
    }
}