using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.WebServiceClient;

namespace Microsoft.Deployment.Common.Actions.MsCrm
{
    [Export(typeof(IAction))]
    public class CrmGetEntityInitialCounts : BaseAction
    {
        public async override Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string refreshToken = request.DataStore.GetJson("MsCrmToken")["refresh_token"].ToString();
            string organizationUrl = request.DataStore.GetValue("OrganizationUrl");
            string[] entities = request.DataStore.GetValue("Entities").Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            var crmToken = CrmTokenUtility.RetrieveCrmOnlineToken(refreshToken, request.Info.WebsiteRootUrl, request.DataStore, organizationUrl);

            Dictionary<string, int> initialCounts = new Dictionary<string, int>();

            var proxy = new OrganizationWebProxyClient(new Uri($"{organizationUrl}XRMServices/2011/Organization.svc/web"), true)
            {
                HeaderToken = crmToken["access_token"].ToString()
            };

            foreach(var entry in entities)
            {
                var xml = $@"
                        <fetch distinct='false' mapping='logical' aggregate='true'> 
                            <entity name='{entry}'> 
                                <attribute name='{entry}id' alias='{entry}_count' aggregate='count'/> 
                            </entity> 
                        </fetch>";


                var resultEntity = proxy.RetrieveMultiple(
                new FetchExpression(xml)).Entities.First();
                var count = resultEntity
                  .GetAttributeValue<Microsoft.Xrm.Sdk.AliasedValue>($"{entry}_count").Value;

                initialCounts.Add(entry, Convert.ToInt16(count));
            }

            return new ActionResponse(ActionStatus.Success);
        }
    }
}
