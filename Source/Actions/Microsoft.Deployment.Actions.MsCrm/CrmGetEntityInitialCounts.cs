using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Helpers;
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

            Dictionary<string, int> cutouts = new Dictionary<string, int>()
            {
                {"account",1000 },
                {"leads",1000 },
                {"opportunities",1000 }
            };
            //"account,lead,opportunity,opportunityproduct,product,systemuser,systemusermanagermap,territory",
            Dictionary<string, int> initialCounts = new Dictionary<string, int>();

            var proxy = new OrganizationWebProxyClient(new Uri($"{organizationUrl}XRMServices/2011/Organization.svc/web"), true)
            {
                HeaderToken = crmToken["access_token"].ToString()
            };

            foreach (var entry in entities)
            {
                var xml = $@"
                        <fetch distinct='false' mapping='logical' aggregate='true'> 
                            <entity name='{entry}'> 
                                <attribute name='{entry}id' alias='{entry}_count' aggregate='count'/> 
                            </entity> 
                        </fetch>";

                var fetchRequest = new ExecuteFetchRequest() { FetchXml = xml };

                var result = (ExecuteFetchResponse)proxy.Execute(fetchRequest);

                var xdoc = XDocument.Parse(result.FetchXmlResult);

                var count = xdoc.Descendants().First(e => e.Name == $"{entry}_count").Value;

                initialCounts.Add(entry, Convert.ToInt16(count));
            }

            request.DataStore.AddToDataStore("InitialCounts", JsonUtility.GetJObjectFromObject(initialCounts));
            return new ActionResponse(ActionStatus.Success);
        }
    }
}
