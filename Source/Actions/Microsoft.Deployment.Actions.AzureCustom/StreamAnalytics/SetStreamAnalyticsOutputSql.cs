using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Bpst;
using Microsoft.Deployment.Common.Model.StreamAnalytics;

namespace Microsoft.Deployment.Actions.AzureCustom.StreamAnalytics
{
    [Export(typeof(IAction))]
    public class SetStreamAnalyticsOutputSql : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            BpstAzure ba = new BpstAzure(request.DataStore);

            AzureHttpClient ahc = new AzureHttpClient(ba.TokenAzure);

            StreamAnalyticsOutputSql parameters = JsonUtility.Deserialize<StreamAnalyticsOutputSql>(request.DataStore.GetJson("streamAnalyticsOutputSql"));
            string nameStreamAnalyticsJob = request.DataStore.GetValue("nameStreamAnalyticsJob");

            string aliasOutput = parameters.Name;

            string database = request.DataStore.GetValue("Database");
            string password = request.DataStore.GetValue("Password");
            string server = request.DataStore.GetValue("Server");
            string user = request.DataStore.GetValue("Username");

            string url = $"https://management.azure.com/subscriptions/{ba.IdSubscription}/resourceGroups/{ba.NameResourceGroup}/providers/Microsoft.StreamAnalytics/streamingjobs/{nameStreamAnalyticsJob}/outputs/{aliasOutput}?api-version=2015-10-01";

            StreamAnalyticsOutputSqlPropertiesWrapper body = new StreamAnalyticsOutputSqlPropertiesWrapper(server, database, user, password, parameters.Table);

            string error = await ahc.Test(HttpMethod.Put, url, JsonUtility.Serialize(body));

            return error == null
                ? new ActionResponse(ActionStatus.Success)
                : new ActionResponse(ActionStatus.Failure, new ActionResponseExceptionDetail("StreamAnalyticsSetOutputFailure", error));
        }
    }
}