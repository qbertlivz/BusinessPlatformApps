using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Hyak.Common.Internals;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.WebServiceClient;
using Newtonsoft.Json.Linq;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Controller;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Common.Actions.MsCrm
{
    [Export(typeof(IAction))]
    class CrmValidateEntities : BaseAction
    {
        private int maxRetries = 3;

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string refreshToken = request.DataStore.GetJson("MsCrmToken")["refresh_token"].ToString();
            string organizationUrl = request.DataStore.GetValue("OrganizationUrl");
            string[] entities = request.DataStore.GetValue("Entities").Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            var crmToken = CrmTokenUtility.RetrieveCrmOnlineToken(refreshToken, request.Info.WebsiteRootUrl, request.DataStore, organizationUrl);

            var proxy = new OrganizationWebProxyClient(new Uri($"{organizationUrl}XRMServices/2011/Organization.svc/web"), true)
            {
                HeaderToken = crmToken["access_token"].ToString()
            };

            Dictionary<string, bool> entitiesToReprocess = new Dictionary<string, bool>();
            
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    Parallel.ForEach(entities, (entity) =>
                    {
                        try
                        {
                            entitiesToReprocess.Add(entity, this.CheckAndUpdateEntity(entity, proxy, request.Logger));
                        }
                        catch (Exception e)
                        {
                            e.Data.Add("entity", entity);
                            throw;
                        }
                    });
                }
                catch(Exception e)
                {
                    if (e.GetType() == typeof(AggregateException))
                    {
                        string output = string.Empty;
                        foreach (var ex in (e as AggregateException).InnerExceptions)
                        {
                            output += ex.Message + $"[{ex.Data["entity"]}]. ";
                        }
                        
                        return new ActionResponse(ActionStatus.Failure, JsonUtility.GetJObjectFromObject(e), e, "DefaultErrorCode", output);
                    }
                    else
                        return new ActionResponse(ActionStatus.Failure, JsonUtility.GetJObjectFromObject(e), e, "DefaultErrorCode", e.Message);
                }

                if (!entitiesToReprocess.Values.Contains(false))
                {
                    break;
                }
            }

            return new ActionResponse(ActionStatus.Success);
        }

        public bool CheckAndUpdateEntity(string entity, OrganizationWebProxyClient proxy, Logger logger)
        {
            var checkRequest = new RetrieveEntityRequest()
            {
                LogicalName = entity,
                EntityFilters = EntityFilters.Entity
            };

            var checkResponse = new RetrieveEntityResponse();
            checkResponse = (RetrieveEntityResponse)proxy.Execute(checkRequest);

            if (checkResponse.EntityMetadata.ChangeTrackingEnabled != null &&
                !(bool)checkResponse.EntityMetadata.ChangeTrackingEnabled &&
                checkResponse.EntityMetadata.CanChangeTrackingBeEnabled.Value)
            {
                var updateRequest = new UpdateEntityRequest()
                {
                    Entity = checkResponse.EntityMetadata
                };

                updateRequest.Entity.ChangeTrackingEnabled = true;
                var updateResponse = new UpdateEntityResponse();
                updateResponse = (UpdateEntityResponse)proxy.Execute(updateRequest);

                return true;
            }

            if (checkResponse.EntityMetadata.ChangeTrackingEnabled != null &&
                (bool)checkResponse.EntityMetadata.ChangeTrackingEnabled)
            {
                return true;
            }

            return false;
        }
    }
}