using Microsoft.Deployment.Common.Actions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Actions.Salesforce.SalesforceSOAP;
using Microsoft.Deployment.Actions.Salesforce.Helpers;
using System.Dynamic;
using System.ServiceModel;
using System.Diagnostics;

namespace Microsoft.Deployment.Actions.Salesforce
{
    [Export(typeof(IAction))]
    public class SalesforceGetEntities : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string sfUsername = request.DataStore.GetValue("SalesforceUser");
            string sfPassword = request.DataStore.GetValue("SalesforcePassword");
            string sfToken = request.DataStore.GetValue("SalesforceToken");
            string sfTestUrl = request.DataStore.GetValue("SalesforceUrl");
            var requiredObjects = request.DataStore.GetValue("ObjectTables").Split(',');

            SoapClient binding = new SoapClient("Soap");

            if (!string.IsNullOrEmpty(sfTestUrl) && sfTestUrl.Contains("test"))
            {
                binding.Endpoint.Address = new System.ServiceModel.EndpointAddress(binding.Endpoint.Address.ToString().Replace("login", "test"));
            }

            LoginResult lr;

            SecurityHelper.SetTls12();

            binding.ClientCredentials.UserName.UserName = sfUsername;
            binding.ClientCredentials.UserName.Password = sfPassword;

            lr =
               binding.login(null, null,
               sfUsername,
               string.Concat(sfPassword, sfToken));

            var sfObjects = new List<string>();

            binding = new SoapClient("Soap");
            SessionHeader sheader = new SessionHeader();
            BasicHttpBinding bind = new BasicHttpBinding();
            bind = (BasicHttpBinding)binding.Endpoint.Binding;
            bind.MaxReceivedMessageSize = int.MaxValue;
            bind.MaxBufferPoolSize = int.MaxValue;
            bind.MaxBufferSize = int.MaxValue;
            bind.CloseTimeout = new TimeSpan(0, 0, 5, 0);
            bind.OpenTimeout = new TimeSpan(0, 0, 5, 0);
            bind.SendTimeout = new TimeSpan(0, 0, 5, 0);
            bind.ReaderQuotas.MaxArrayLength = int.MaxValue;
            bind.ReaderQuotas.MaxDepth = int.MaxValue;
            bind.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            bind.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            bind.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            bind.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;

            binding.Endpoint.Binding = bind;
            binding.Endpoint.Address = new EndpointAddress(lr.serverUrl);

            sheader.sessionId = lr.sessionId;

            binding.Endpoint.ListenUri = new Uri(lr.metadataServerUrl);

            DescribeGlobalResult result;
            try
            {
                binding.describeGlobal(sheader, null, null, out result);
            }
            catch (Exception e)
            {
                return new ActionResponse(ActionStatus.FailureExpected, JsonUtility.GetEmptyJObject(), e, "FailedToGetAdditionalEntities");
            }

            foreach (var obj in result.sobjects)
            {
                if (!requiredObjects.Contains(obj.name))
                {
                    sfObjects.Add(obj.name);
                }
            }

            return new ActionResponse(ActionStatus.Success, JsonUtility.Serialize(sfObjects));
        }
    }
}