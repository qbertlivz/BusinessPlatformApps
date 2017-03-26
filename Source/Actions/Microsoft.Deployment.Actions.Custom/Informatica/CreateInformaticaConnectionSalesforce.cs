using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Informatica;

namespace Microsoft.Deployment.Actions.Custom.Informatica
{
    [Export(typeof(IAction))]
    public class CreateInformaticaConnectionSalesforce : BaseAction
    {
        private const string URL_CONNECTIONS = "api/v2/connection";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = await InformaticaUtility.Initialize(request.DataStore.GetValue("InformaticaUsername"), request.DataStore.GetValue("InformaticaPassword"));

            InformaticaConnection ic = new InformaticaConnection
            {
                username = request.DataStore.GetValue("SalesforceUser"),
                password = request.DataStore.GetValue("SalesforcePassword"),
                securityToken = request.DataStore.GetValue("SalesforceToken"),
                serviceUrl = $"https://{request.DataStore.GetValue("SalesforceUrl")}/services/Soap/u/34.0",
                Name = InformaticaUtility.BPST_SOURCE_NAME,
                OrgId = rc.ID,
                ConnectionType = "Salesforce",
                RuntimeEnvironmentId = await InformaticaUtility.GetRuntimeEnvironmentId(rc)
            };

            await rc.Post(URL_CONNECTIONS, JsonConvert.SerializeObject(ic));

            return new ActionResponse(ActionStatus.Success, JsonUtility.GetEmptyJObject());
        }
    }
}