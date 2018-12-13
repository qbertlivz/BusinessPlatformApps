using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

using Microsoft.AnalysisServices.Tabular;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;

namespace Microsoft.Deployment.Actions.AzureCustom.AzureAS
{
    [Export(typeof(IAction))]
    public class AssignPermissionsForUser : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            var azureToken = request.DataStore.GetJson("AzureTokenAS");
            string serverUrl = request.DataStore.GetValue("ASServerUrl");
            string asDatabase = request.DataStore.GetValue("ASDatabase");
            string user = request.DataStore.GetValue("UserToAdd");
            string connectionString = ValidateConnectionToAS.GetASConnectionString(request, azureToken, serverUrl);

            Server server = null;
            try
            {
                server = new Server();
                server.Connect(connectionString);

                Database db = server.Databases.FindByName(asDatabase);
                if (db == null)
                {
                    return new ActionResponse(ActionStatus.Failure, string.Empty, null, null, "Unable to find model");
                }

                var role = new ModelRole()
                {
                    Name = "ProcessOnlyRole"
                };

                role.ModelPermission = ModelPermission.Administrator;
                ExternalModelRoleMember member = new ExternalModelRoleMember();
                member.IdentityProvider = "AzureAD";
                member.MemberName = user;
                //AzureAD
                role.Members.Add(member);

                db.Model.Roles.Add(role);
                db.Model.SaveChanges();
                server.Disconnect(true);

                return new ActionResponse(ActionStatus.Success);
            }
            catch (Exception e)
            {
                return new ActionResponse(ActionStatus.Failure, string.Empty, e, null, "Unable to assign permissions");
            }
            finally
            {
                server?.Dispose();
            }

        }
    }
}