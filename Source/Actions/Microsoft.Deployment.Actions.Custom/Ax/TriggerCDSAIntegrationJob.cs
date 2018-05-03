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
using Newtonsoft.Json;
using System.Dynamic;

namespace Microsoft.Deployment.Actions.Custom.Ax
{
    [Export(typeof(IAction))]
    public class TriggerCDSAIntegrationJob : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var axToken = request.DataStore.GetJson("AxToken", "access_token").ToString();
            string instance = request.DataStore.GetValue("AxInstanceName");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(instance);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", axToken.ToString());

                dynamic updatePayload = new ExpandoObject();
                updatePayload.MeasurementName = "CustCollectionsBIMeasurements";
                updatePayload.BlobStorageAccount = "AxBlobStorageAccount";

                var msg = new HttpRequestMessage();
                msg.Method = new HttpMethod("POST");
                msg.RequestUri = new Uri(instance.Trim() + $"data/CDSAIntegrationEntities/Microsoft.Dynamics.DataEntities.StartCopyJob");
                msg.Content = new StringContent(JsonConvert.SerializeObject(updatePayload), Encoding.UTF8, "application/json");
                var response = await client.SendAsync(msg);

                // TODO: Uncomment the below code once CDSAIntegrationEntities entity is available
                //if (response.IsSuccessStatusCode)
                //{
                //    return new ActionResponse(ActionStatus.Success);
                //}

                //switch (response.StatusCode)
                //{
                //    case HttpStatusCode.NotFound:
                //        return new ActionResponse(ActionStatus.Failure, JsonUtility.GetJsonObjectFromJsonString(response.Content.ReadAsStringAsync().Result), null, "AxWrongPlatform", response.Content.ReadAsStringAsync().Result);
                //    case HttpStatusCode.Unauthorized:
                //        return new ActionResponse(ActionStatus.Failure, response.ReasonPhrase, null, "AxAuthorizationError", response.ReasonPhrase);
                //    default:
                //        return new ActionResponse(ActionStatus.Failure, JsonUtility.GetJsonObjectFromJsonString(response.Content.ReadAsStringAsync().Result), null, "AxConnectionError", response.Content.ReadAsStringAsync().Result);
                //}

                return new ActionResponse(ActionStatus.Success);
            }
        }
    }
}
