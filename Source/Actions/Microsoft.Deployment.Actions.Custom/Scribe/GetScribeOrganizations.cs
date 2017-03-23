using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Scribe;

namespace Microsoft.Deployment.Actions.Custom.Scribe
{
    [Export(typeof(IAction))]
    public class GetScribeOrganizations : BaseAction
    {
        private const string REPLICATION_SERVICES = "Replication Services (RS)";
        private const string URL_ORGANIZATIONS = "/v1/orgs";
        private const string URL_PROVISION_CLOUD_AGENT = "/v1/orgs/{0}/agents/provision_cloud_agent";
        private const string URL_SUBSCRIPTIONS = "/v1/orgs/{0}/subscriptions";

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetLastValue("ScribeUsername"), request.DataStore.GetLastValue("ScribePassword"));

            List<ScribeOrganization> orgs = JsonConvert.DeserializeObject<List<ScribeOrganization>>(await rc.Get(URL_ORGANIZATIONS));
            List<ScribeOrganization> configuredOrgs = new List<ScribeOrganization>();

            if (orgs != null && orgs.Count > 0)
            {
                foreach(ScribeOrganization org in orgs)
                {
                    if (await IsConfigured(rc, org))
                    {
                        await ProvisionCloudAgent(rc, org);
                        configuredOrgs.Add(org);
                    }
                }
            }

            return configuredOrgs.Count == 0
                ? new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), null, "Scribe_No_Organizations")
                : new ActionResponse(ActionStatus.Success, JsonUtility.GetJObjectFromStringValue(JsonConvert.SerializeObject(configuredOrgs)));
        }

        private async Task<bool> IsConfigured(RestClient rc, ScribeOrganization org)
        {
            bool isConfigured = false;

            try
            {
                string response = await rc.Get(string.Format(CultureInfo.InvariantCulture, URL_SUBSCRIPTIONS, org.Id));
                List<ScribeSubscription> subscriptions = JsonConvert.DeserializeObject<List<ScribeSubscription>>(response);

                if (subscriptions != null)
                {
                    for (int i = 0; i < subscriptions.Count && !isConfigured; i++)
                    {
                        isConfigured = subscriptions[i].Name.Equals(REPLICATION_SERVICES, StringComparison.OrdinalIgnoreCase) &&
                            DateTime.Now.CompareTo(Convert.ToDateTime(subscriptions[i].ExpirationDate)) < 0;
                    }
                }
            }
            catch
            {
                // Failed to get subscriptions - IP address wasn't safe listed
            }

            return isConfigured;
        }

        private async Task ProvisionCloudAgent(RestClient rc, ScribeOrganization org)
        {
            try
            {
                await rc.Post(string.Format(CultureInfo.InvariantCulture, URL_PROVISION_CLOUD_AGENT, org.Id), string.Empty);
            }
            catch (Exception)
            {
                // Silently ignore exception if the cloud agent was already provisioned
            }
        }
    }
}