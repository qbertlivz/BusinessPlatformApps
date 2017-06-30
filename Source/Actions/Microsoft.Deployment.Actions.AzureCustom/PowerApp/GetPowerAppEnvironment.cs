using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.PowerApp;

namespace Microsoft.Deployment.Actions.AzureCustom.PowerApp
{
    [Export(typeof(IAction))]
    public class GetPowerAppEnvironment : BaseAction
    {
        private string BASE_POWER_APPS_URL = "https://management.azure.com/providers/Microsoft.PowerApps";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            AzureHttpClient ahc = new AzureHttpClient(request.DataStore.GetJson("AzureToken", "access_token"));

            string environmentUrl = $"{BASE_POWER_APPS_URL}/environments?api-version=2016-11-01&$filter=minimumAppPermission%20eq%20%27CanEdit%27&$expand=Permissions&_poll=true";

            List<PowerAppEnvironment> environments = await ahc.RequestValue<List<PowerAppEnvironment>>(HttpMethod.Get, environmentUrl);

            bool foundEnvironment = false;

            if (environments.IsNullOrEmpty())
            {
                if (request.DataStore.GetValue("SkipPowerApp") == null)
                {
                    request.DataStore.AddToDataStore("SkipPowerApp", "true", DataStoreType.Public);
                }
                foundEnvironment = true;
            }
            else
            {
                for (int i = 0; i < environments.Count && !foundEnvironment; i++)
                {
                    PowerAppEnvironment environment = environments[i];
                    if (environment.Properties != null && environment.Properties.IsDefault && environment.Properties.Permissions != null && environment.Properties.Permissions.CreatePowerApp != null)
                    {
                        request.DataStore.AddToDataStore("PowerAppEnvironment", environment.Name, DataStoreType.Private);
                        foundEnvironment = true;
                    }
                }
            }

            return foundEnvironment
                ? new ActionResponse(ActionStatus.Success)
                : new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "PowerAppNoEnvironment");
        }
    }
}