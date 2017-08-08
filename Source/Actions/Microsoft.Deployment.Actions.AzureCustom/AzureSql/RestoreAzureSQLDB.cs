using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

using DAC = Microsoft.SqlServer.Dac;

namespace Microsoft.Deployment.Actions.AzureCustom.AzureSql
{
    [Export(typeof(IAction))]
    class RestoreAzureSQLDB : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string targetConnectionString = request.DataStore.GetAllValues("SqlConnectionString")[0];

            DAC.DacServices dacService = new DAC.DacServices(targetConnectionString);
            // DAC.BacPackage backup = DAC.BacPackage.Load(); Stream here
            

            return new ActionResponse(ActionStatus.Success);
        }
    }
}
