using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.AzureCustom.Common
{
    [Export(typeof(IAction))]
    class ConfigureNotifier : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string azureToken = request.DataStore.GetJson("AzureToken", "access_token");
            string subscription = request.DataStore.GetJson("SelectedSubscription", "SubscriptionId");
            string resourceGroup = request.DataStore.GetValue("SelectedResourceGroup");
            string triggerUrl = request.DataStore.GetValue("NotifierTriggerUrl");
            string connectionString = request.DataStore.GetValue("SqlConnectionString");
            string deploymentIdsConnection = Constants.BpstDeploymentIdDatabase;

            string notifierUrl = Constants.BpstNotifierUrl;
            string emails = request.DataStore.GetValue("notificationEmails");
            string table = request.DataStore.GetValue("notificationTable");
            string deploymentId = Guid.NewGuid().ToString();
            string sprocName = request.DataStore.GetValue("sprocName");
            string templateName = request.Info.AppName;
            string initialPullComplete = "-1";


            string cmd = $"INSERT INTO {table} VALUES('{deploymentId}','{notifierUrl}','{emails}','{sprocName}','{templateName}','{initialPullComplete}','{DateTime.UtcNow.ToString("o")}')";
            SqlUtility.InvokeSqlCommand(connectionString, cmd, new Dictionary<string, string>());

            cmd = $"INSERT INTO deploymentids VALUES('{deploymentId}','{DateTime.UtcNow.ToString("o")}')";
            SqlUtility.InvokeSqlCommand(deploymentIdsConnection, cmd, new Dictionary<string, string>());

            AzureHttpClient azureClient = new AzureHttpClient(azureToken, subscription, resourceGroup);
            var response = await azureClient.ExecuteGenericRequestNoHeaderAsync(HttpMethod.Post, triggerUrl, string.Empty);

            return new ActionResponse(ActionStatus.Success);
        }
    }
}
