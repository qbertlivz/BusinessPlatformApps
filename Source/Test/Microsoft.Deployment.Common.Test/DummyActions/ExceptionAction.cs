using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;

namespace Microsoft.Deployment.Common.Test.DummyActions
{
    [Export(typeof(IAction))]
    public class ExceptionAction : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            SqlException exception = null;
            try
            {
                SqlConnection conn = new SqlConnection(@"Data Source=.;Database=GUARANTEED_TO_FAIL;Connection Timeout=1");
                conn.Open();
            }
            catch (SqlException ex)
            {
                exception = ex;
            }

            throw exception;
        }
    }
}