using System;
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
    public class GetScribeOrganizations : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            RestClient rc = ScribeUtility.Initialize(request.DataStore.GetValue("ScribeUsername"), request.DataStore.GetValue("ScribePassword"));

            List<ScribeOrganization> orgs = JsonUtility.Deserialize<List<ScribeOrganization>>(await rc.Get(ScribeUtility.URL_ORGANIZATIONS));
            List<ScribeOrganization> configuredOrgs = new List<ScribeOrganization>();

            if (!orgs.IsNullOrEmpty())
            {
                foreach(ScribeOrganization org in orgs)
                {
                    await SafeList(rc, org);

                    if (await IsConfigured(rc, org))
                    {
                        await ProvisionCloudAgent(rc, org);
                        configuredOrgs.Add(org);
                    }
                }
            }

            return configuredOrgs.Count == 0
                ? new ActionResponse(ActionStatus.Failure, new ActionResponseExceptionDetail("Scribe_No_Organizations"))
                : new ActionResponse(ActionStatus.Success, JsonUtility.Serialize(configuredOrgs));
        }

        private async Task<bool> IsConfigured(RestClient rc, ScribeOrganization org)
        {
            bool isConfigured = false;

            try
            {
                List<ScribeSubscription> subscriptions = JsonUtility.Deserialize<List<ScribeSubscription>>(await rc.Get(string.Format(ScribeUtility.URL_SUBSCRIPTIONS, org.Id)));

                if (subscriptions != null)
                {
                    for (int i = 0; i < subscriptions.Count && !isConfigured; i++)
                    {
                        isConfigured = subscriptions[i].Name.EqualsIgnoreCase(ScribeUtility.REPLICATION_SERVICES) &&
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
                await rc.Post(string.Format(ScribeUtility.URL_PROVISION_CLOUD_AGENT, org.Id), string.Empty);
            }
            catch
            {
                // Silently ignore exception if the cloud agent was already provisioned
            }
        }

        private async Task SafeList(RestClient rc, ScribeOrganization org)
        {
            try
            {
                await rc.Post(string.Format(ScribeUtility.URL_SECURITY_RULES, org.Id), JsonUtility.GetJsonStringFromObject(new ScribeSecurityRule()));
            }
            catch
            {
                // Failed to apply security rule
            }
        }
    }
}