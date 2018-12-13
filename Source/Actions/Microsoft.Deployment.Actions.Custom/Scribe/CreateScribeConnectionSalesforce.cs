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
    public class CreateScribeConnectionSalesforce : BaseAction
    {
        private const string CONNECTOR_ID = "8ADD76FC-525F-4B4B-B79E-945A6A762792";

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
                Name = ScribeUtility.BPST_SOURCE_NAME,
                Properties = new List<ScribeKeyValue>()
            };

            // Set discovery URL
            ScribeKeyValue kvp = new ScribeKeyValue { Key = "Url", Value = ScribeUtility.AesEncrypt(apiToken, $"https://{request.DataStore.GetValue("SalesforceUrl")}/services/Soap/u/33.0") };
            connection.Properties.Add(kvp);
            // Set CRM user name
            kvp = new ScribeKeyValue { Key = "UserId", Value = ScribeUtility.AesEncrypt(apiToken, request.DataStore.GetValue("SalesforceUser")) };
            connection.Properties.Add(kvp);
            // Set CRM user password
            kvp = new ScribeKeyValue { Key = "Password", Value = ScribeUtility.AesEncrypt(apiToken, request.DataStore.GetValue("SalesforcePassword")) };
            connection.Properties.Add(kvp);
            // Set bulk APIs
            kvp = new ScribeKeyValue { Key = "UseBulkApiRS", Value = ScribeUtility.AesEncrypt(apiToken, "true") };
            connection.Properties.Add(kvp);
            // Set metadata refresh
            kvp = new ScribeKeyValue { Key = "RefeshMetaDataUponReconnect", Value = ScribeUtility.AesEncrypt(apiToken, "true") };
            connection.Properties.Add(kvp);
            // Set organization name
            kvp = new ScribeKeyValue { Key = "SecurityToken", Value = ScribeUtility.AesEncrypt(apiToken, request.DataStore.GetValue("SalesforceToken")) };
            connection.Properties.Add(kvp);

            await rc.Post(string.Format(ScribeUtility.URL_CONNECTIONS, orgId), JsonUtility.Serialize(connection));

            return new ActionResponse(ActionStatus.Success);
        }
    }
}