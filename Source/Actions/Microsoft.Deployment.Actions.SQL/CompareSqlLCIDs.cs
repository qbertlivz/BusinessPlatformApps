using System.ComponentModel.Composition;
using System.Data;
using System.Threading.Tasks;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Enums;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.SQL
{
    [Export(typeof(IAction))]
    public class CompareSqlLCIDs : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string cnSrc = request.DataStore.GetAllValues("SqlConnectionString")[0];
            string cnDest = request.DataStore.GetAllValues("SqlConnectionString")[1];

            DataTable dtSrcLCID = SqlUtility.RunCommand(cnSrc, "SELECT Convert(int, COLLATIONPROPERTY( Convert(nvarchar, DATABASEPROPERTYEX(Db_Name(), 'Collation')) , 'LCID' )) DB_LCID", SqlCommandType.ExecuteWithData);
            DataTable dtDestLCID = SqlUtility.RunCommand(cnDest, "SELECT Convert(int, COLLATIONPROPERTY( Convert(nvarchar, DATABASEPROPERTYEX(Db_Name(), 'Collation')) , 'LCID' )) DB_LCID", SqlCommandType.ExecuteWithData);

            if (dtSrcLCID == null || dtSrcLCID.Rows.Count==0 || dtDestLCID== null || dtDestLCID.Rows.Count==0)
            {
                return new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "SQL_CannotRetrieveCollations");
            }

            int sourceLCID = (int)dtSrcLCID.Rows[0]["DB_LCID"];
            int destLCID = (int)dtDestLCID.Rows[0]["DB_LCID"];

            return sourceLCID==destLCID ? new ActionResponse(ActionStatus.Success) :
                                          new ActionResponse(ActionStatus.Failure, JsonUtility.GetEmptyJObject(), "SQL_TargetCollationDoesntMatch");
        }
    }
}