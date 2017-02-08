using System.Diagnostics;
using System.Threading;

namespace Microsoft.Deployment.Common.Actions.MsCrm
{
    using Microsoft.Deployment.Common.ActionModel;
    using Microsoft.Deployment.Common.Actions;
    using Microsoft.Deployment.Common.Helpers;
    using Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.ComponentModel.Composition;
    using System.Dynamic;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    [Export(typeof(IAction))]
    public class CrmGetOrgs : BaseAction
    {

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string token = request.DataStore.GetJson("MsCrmToken")["access_token"].ToString();
            AuthenticationHeaderValue bearer = new AuthenticationHeaderValue("Bearer", token);

            RestClient _rc = new RestClient(MsCrmEndpoints.ENDPOINT, bearer);
            string response = await _rc.Get(MsCrmEndpoints.URL_ORGANIZATIONS);

            MsCrmOrganization[] orgs = JsonConvert.DeserializeObject<MsCrmOrganization[]>(response);
            Task<string>[] resultsList = new Task<string>[orgs.Length];

            for (int i = 0; i < orgs.Length; i++)
                resultsList[i] = _rc.Get(MsCrmEndpoints.URL_ORGANIZATION_METADATA, $"organizationUrl={WebUtility.UrlEncode(orgs[i].OrganizationUrl)}");

            try
            {
                await Task.WhenAll(resultsList);
            }
            catch
            {
                // do nothing
            }
            finally
            {
                for (int i = 0; i < resultsList.Length; i++)
                {
                    if (resultsList[i].IsFaulted && resultsList[i].Exception != null)
                    {
                        orgs[i].ErrorCategory = resultsList[i].Exception.Message.ToLowerInvariant().Contains("failed authorization") ? 1 : 2;
                        orgs[i].ErrorCode = resultsList[i].Exception.HResult;
                        orgs[i].ErrorMessage = resultsList[i].Exception.Message;
                    }
                    else
                    {
                        MsCrmOrganization o = JsonConvert.DeserializeObject<MsCrmOrganization>(resultsList[i].Result);
                        orgs[i].ConnectorUrl = o.ConnectorUrl;
                        if (string.IsNullOrEmpty(o.ConnectorUrl))
                            request.Logger.LogEvent("MSCRM-NoConnectorURL", new System.Collections.Generic.Dictionary<string, string> { { o.OrganizationName, o.OrganizationId } });
                    }

                }

            }


            // This is a bit of a dance to accomodate ActionResponse and its need for a JObject
            response = JsonConvert.SerializeObject(orgs);

            return string.IsNullOrWhiteSpace(response)
                ? new ActionResponse(ActionStatus.Failure, new JObject(), "MsCrm_NoOrgs")
                : new ActionResponse(ActionStatus.Success, JsonUtility.GetJObjectFromStringValue(response));
        }
    }
}