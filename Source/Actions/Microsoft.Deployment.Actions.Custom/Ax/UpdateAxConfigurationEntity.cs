using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Newtonsoft.Json;

namespace Microsoft.Deployment.Actions.Custom.Ax
{
    [Export(typeof(IAction))]
    public class UpdateAxConfigurationEntity : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var axToken = request.DataStore.GetJson("AxToken", "access_token").ToString();
            string connString = request.DataStore.GetValue("SqlConnectionString");
            string measurementName = request.DataStore.GetValue("AxMeasurementName");
            string reportName = request.DataStore.GetValue("AxReportName");
            string instance = request.DataStore.GetValue("AxInstanceName");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(instance);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", axToken.ToString());

            dynamic payload = new ExpandoObject();
            payload.MeasurementName = measurementName;
            payload.ReportName = reportName;
            payload.SqlConnectionString = connString;

            var resp = await client.PostAsync($"data/BpstConfigurationEntities", new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

            var entry = JsonUtility.GetJObjectFromJsonString(resp.Content.ReadAsStringAsync().Result);

            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new ActionResponse(ActionStatus.Failure, null, null, null, "Token expired or user not authorized. Error:" + resp.ReasonPhrase + "," + resp.Content.ReadAsStringAsync().Result);
            }

            var id = entry["Id"]?.ToString();
            var areaId = entry["dataAreaId"]?.ToString();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(areaId))
            {
                return new ActionResponse(ActionStatus.Failure, "Entity was not created successfully.");
            }

            request.DataStore.AddToDataStore("AxEntityId", id, DataStoreType.Private);
            request.DataStore.AddToDataStore("AxEntityDataAreaId", areaId, DataStoreType.Private);

            return new ActionResponse(ActionStatus.Success);
        }
    }
}
