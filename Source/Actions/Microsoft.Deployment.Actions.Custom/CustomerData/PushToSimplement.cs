using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

using Microsoft.Deployment.Common;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;

namespace Microsoft.Deployment.Actions.Custom.CustomerData
{
    [Export(typeof(IAction))]
    public class PushToSimplement : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string fName  = request.DataStore.GetValue("FirstName");
            string lName  = request.DataStore.GetValue("LastName");
            string company = request.DataStore.GetValue("CompanyName");
            string email = request.DataStore.GetValue("RowKey");

            StorageCredentials sasCredentials = new StorageCredentials(Constants.SimplementSasToken);
            CloudTableClient tableClient = new CloudTableClient(new Uri(Constants.SimplementBlobStorage), sasCredentials);
            CloudTable table = tableClient.GetTableReference("contact");

            CustomerInfoSimplement customerInfoSimplement = new CustomerInfoSimplement(email)
            {
                FirstName = fName,
                LastName = lName,
                CompanyName = company
            };

            // Cannot do insert_OR_replace since the policy won't allow it. Just do an insert
            try
            {
                table.Execute(TableOperation.Insert(customerInfoSimplement));
            }
            catch (Exception e)
            {
                // This call should never show an error to the user. We are mostly expecting a StorageException for duplicate values
                //if (e.Message.IndexOf("(409) Conflict", StringComparison.OrdinalIgnoreCase) == -1)
                    request.Logger.LogException(e);
            }

            return new ActionResponse(ActionStatus.Success);
        }
    }
}