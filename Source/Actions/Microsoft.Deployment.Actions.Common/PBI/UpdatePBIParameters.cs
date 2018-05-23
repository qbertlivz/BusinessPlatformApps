using Microsoft.Deployment.Common.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Deployment.Common.Helpers;
using System.Net;
using Microsoft.Deployment.Common;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.Actions.Common.PBI
{
    [Export(typeof(IAction))]
    public class UpdatePBIParameters : BaseAction
    {
        private const string PBI_UPDATE_PARAMETERS = "v1.0/myorg/groups/{0}/datasets/{1}/UpdateParameters";
        private const string DATAPOOLID_PARAMETER = "DatapoolId";
        private const string WORKSPACEID_PARAMETER = "WorkspaceId";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string pbiToken = request.DataStore.GetJson("PBIToken", "access_token").ToString();
            string pbiWorkspaceId = request.DataStore.GetValue("PBIWorkspaceId");
            string pbiDatasetId = request.DataStore.GetValue("PBIXDatasetId");
            string pbiDatapoolId = request.DataStore.GetValue("PBIDatapoolId");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(request.DataStore.GetValue("PBIClusterUri"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pbiToken.ToString());

                dynamic payload = new ExpandoObject();
                payload.updateDetails = new JArray() as dynamic;

                dynamic parameter = new JObject();
                parameter.name = DATAPOOLID_PARAMETER;
                parameter.newValue = pbiDatapoolId;
                payload.updateDetails.Add(parameter);

                parameter = new JObject();
                parameter.name = WORKSPACEID_PARAMETER;
                parameter.newValue = pbiWorkspaceId;
                payload.updateDetails.Add(parameter);

                var response = await client.PostAsync(string.Format(PBI_UPDATE_PARAMETERS, pbiWorkspaceId, pbiDatasetId), new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return new ActionResponse(ActionStatus.Success);
                }
                return new ActionResponse(ActionStatus.Failure, JsonUtility.GetJsonObjectFromJsonString(response.Content.ReadAsStringAsync().Result), null, "AxWrongPlatform", response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}