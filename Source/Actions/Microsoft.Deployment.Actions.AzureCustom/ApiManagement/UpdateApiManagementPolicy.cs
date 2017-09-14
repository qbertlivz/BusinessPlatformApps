using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.ApiManagement;
using Microsoft.Deployment.Common.Model.Bpst;

namespace Microsoft.Deployment.Actions.AzureCustom.APIManagement
{
    [Export(typeof(IAction))]
    public class UpdateApiManagementPolicy : BaseAction
    {
        private const int SIZE_PADDING = 5;

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            BpstAzure ba = new BpstAzure(request.DataStore);

            AzureHttpClient ahc = new AzureHttpClient(ba.TokenAzure);

            string idApimLogger = request.DataStore.GetValue("IdApimLogger");
            string idApimService = request.DataStore.GetValue("IdApimService");

            string policyContent = "<!--               \r\n    IMPORTANT:\r\n    - Policy elements can appear only within the <inbound>, <outbound>, <backend> section elements.\r\n    - Only the <forward-request> policy element can appear within the <backend> section element.\r\n    - To apply a policy to the incoming request (before it is forwarded to the backend service), place a corresponding policy element within the <inbound> section element.\r\n    - To apply a policy to the outgoing response (before it is sent back to the caller), place a corresponding policy element within the <outbound> section element.\r\n    - To add a policy position the cursor at the desired insertion point and click on the round button associated with the policy.\r\n    - To remove a policy, delete the corresponding policy statement from the policy document.\r\n    - Policies are applied in the order of their appearance, from the top down.\r\n               -->\r\n\r\n<!-- \r\n    This policy has been added as part of the Power BI Solution Template for advanced API analytics. For more information go to: http://#\r\n-->\r\n<policies>\r\n    <inbound>\r\n        <log-to-eventhub partition-id=\"0\" logger-id=\"$(idLogger)\">\r\n            @{\r\n                \r\n                var name = \"\";\r\n                if(context.User != null)\r\n                {\r\n                    name = context.User.FirstName + \" \" + context.User.LastName;\r\n                };\r\n                \r\n                var subId = \"0\";\r\n                if(context.Subscription != null)\r\n                {\r\n                    subId = context.Subscription.Id;\r\n                };\r\n                \r\n                var title = \"CreatedDate,ServiceName,RequestId,IPAddress,Operation,OperationId,Api,ApiId,Product,ProductId,SubscriptionName,SubscriptionId,Length,Type\";\r\n                var values = string.Join(\",\", DateTime.UtcNow.ToString(\"o\"), \r\n                                                context.Deployment.ServiceName, \r\n                                                context.RequestId, \r\n                                                context.Request.IpAddress, \r\n                                                context.Operation.Name,\r\n                                                context.Operation.Id,\r\n                                                context.Api.Name,\r\n                                                context.Api.Id,\r\n                                                context.Product.Name,\r\n                                                context.Product.Id,                                                \r\n                                                name, \r\n                                                subId,\r\n                                                context.Request.Headers.GetValueOrDefault(\"Content-Length\", \"0\"),\r\n                                                \"Request\");\r\n                return title + \"\\r\\n\"+ values;\r\n            }\r\n        </log-to-eventhub>\r\n    </inbound>\r\n    <backend>\r\n        <forward-request follow-redirects=\"true\"/>\r\n    </backend>\r\n    <outbound>\r\n        <log-to-eventhub partition-id=\"1\" logger-id=\"$(idLogger)\">\r\n            @{\r\n                var title = \"CreatedDate,ServiceName,RequestId,StatusCode,StatusReason,Length,Type\";\r\n                var values = string.Join(\",\", DateTime.UtcNow.ToString(\"o\"), \r\n                                                context.Deployment.ServiceName, \r\n                                                context.RequestId, \r\n                                                context.Response.StatusCode.ToString(),\r\n                                                context.Response.StatusReason,\r\n                                                context.Response.Headers.GetValueOrDefault(\"Content-Length\", \"0\"),\r\n                                                \"Response\");\r\n                return title + \"\\r\\n\"+ values;\r\n            }\r\n        </log-to-eventhub>\r\n    </outbound>\r\n    <on-error>\r\n        <log-to-eventhub partition-id=\"2\" logger-id=\"$(idLogger)\">\r\n            @{\r\n                var title = \"CreatedDate,ServiceName,RequestId,ErrorSource,ErrorReason,ErrorMessage,Type\";\r\n                var values = string.Join(\",\", DateTime.UtcNow.ToString(\"o\"), \r\n                                                context.Deployment.ServiceName, \r\n                                                context.RequestId, \r\n                                                context.LastError.Source,\r\n                                                context.LastError.Reason,\r\n                                                context.LastError.Message,\r\n                                                \"Error\");\r\n                return title + \"\\r\\n\"+ values;\r\n            }\r\n        </log-to-eventhub>\r\n    </on-error>\r\n</policies>\r\n";
            policyContent = policyContent.Replace("$(idLogger)", idApimLogger);

            ApiManagementPolicy policy = new ApiManagementPolicy(idApimService, policyContent);

            string url = $"https://management.azure.com{idApimService}/policies/policy?api-version=2017-03-01";

            string error = await ahc.Test(HttpMethod.Put, url, JsonUtility.Serialize(policy));

            return error == null
                ? new ActionResponse(ActionStatus.Success)
                : new ActionResponse(ActionStatus.Failure, new ActionResponseExceptionDetail("ApiManagementFailedToUpdatePolicy", error));
        }
    }
}