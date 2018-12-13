using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ServiceModel.Description;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;

using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;
using Microsoft.Deployment.Common.Model.Scribe;

namespace Microsoft.Deployment.Actions.Custom.Scribe
{
    [Export(typeof(IAction))]
    public class GetD365Organizations : BaseAction
    {
        private string[] discoveryURLs = new string[] {
            "https://disco.crm.dynamics.com",
            "https://disco.crm2.dynamics.com",
            "https://disco.crm3.dynamics.com",
            "https://disco.crm4.dynamics.com",
            "https://disco.crm5.dynamics.com",
            "https://disco.crm6.dynamics.com",
            "https://disco.crm7.dynamics.com",
            "https://disco.crm8.dynamics.com",
            "https://disco.crm9.dynamics.com"
        };

        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            List<D365Organization> d365Organizations = this.SearchForOrganizations(request.DataStore.GetValue("D365Username"), request.DataStore.GetValue("D365Password"));
            return d365Organizations.IsNullOrEmpty()
                ? new ActionResponse(ActionStatus.Failure, new ActionResponseExceptionDetail("MsCrm_No_Organizations"))
                : new ActionResponse(ActionStatus.Success, JsonUtility.Serialize(d365Organizations));
        }

        private AuthenticationCredentials GetCredentials<TService>(string userName, string password, string domain, IServiceManagement<TService> service, AuthenticationProviderType endpointType)
        {
            AuthenticationCredentials authCredentials = new AuthenticationCredentials();

            switch (endpointType)
            {
                case AuthenticationProviderType.ActiveDirectory:
                    authCredentials.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential(userName, password, domain);
                    break;
                case AuthenticationProviderType.LiveId:
                    authCredentials.ClientCredentials.UserName.UserName = userName;
                    authCredentials.ClientCredentials.UserName.Password = password;
                    authCredentials.SupportingCredentials = new AuthenticationCredentials();
                    authCredentials.SupportingCredentials.ClientCredentials = Microsoft.Crm.Services.Utility.DeviceIdManager.LoadOrRegisterDevice();
                    break;
                default: // For Federated and OnlineFederated environments.                    
                    authCredentials.ClientCredentials.UserName.UserName = userName;
                    authCredentials.ClientCredentials.UserName.Password = password;
                    // For OnlineFederated single-sign on, you could just use current UserPrincipalName instead of passing user name and password.
                    // authCredentials.UserPrincipalName = UserPrincipal.Current.UserPrincipalName;  // Windows Kerberos

                    // The service is configured for User Id authentication, but the user might provide Microsoft
                    // account credentials. If so, the supporting credentials must contain the device credentials.
                    if (endpointType == AuthenticationProviderType.OnlineFederation)
                    {
                        IdentityProvider provider = service.GetIdentityProvider(authCredentials.ClientCredentials.UserName.UserName);
                        if (provider != null && provider.IdentityProviderType == IdentityProviderType.LiveId)
                        {
                            authCredentials.SupportingCredentials = new AuthenticationCredentials();
                            authCredentials.SupportingCredentials.ClientCredentials = Microsoft.Crm.Services.Utility.DeviceIdManager.LoadOrRegisterDevice();
                        }
                    }

                    break;
            }

            return authCredentials;
        }

        private OrganizationDetail FindOrganization(string orgUniqueName, OrganizationDetail[] orgDetails)
        {
            if (string.IsNullOrWhiteSpace(orgUniqueName))
                throw new ArgumentNullException("orgUniqueName");
            if (orgDetails == null)
                throw new ArgumentNullException("orgDetails");
            OrganizationDetail orgDetail = null;

            foreach (OrganizationDetail detail in orgDetails)
            {
                if (string.Compare(detail.UniqueName, orgUniqueName, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    orgDetail = detail;
                    break;
                }
            }
            return orgDetail;
        }

        private TProxy GetProxy<TService, TProxy>(IServiceManagement<TService> serviceManagement, AuthenticationCredentials authCredentials) where TService : class where TProxy : ServiceProxy<TService>
        {
            Type classType = typeof(TProxy);

            if (serviceManagement.AuthenticationType != AuthenticationProviderType.ActiveDirectory)
            {
                AuthenticationCredentials tokenCredentials = serviceManagement.Authenticate(authCredentials);

                // Obtain discovery/organization service proxy for Federated, LiveId and OnlineFederated environments. 
                // Instantiate a new class of type using the 2 parameter constructor of type IServiceManagement and SecurityTokenResponse.
                return (TProxy)classType
                    .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(SecurityTokenResponse) })
                    .Invoke(new object[] { serviceManagement, tokenCredentials.SecurityTokenResponse });
            }

            // Obtain discovery/organization service proxy for ActiveDirectory environment.
            // Instantiate a new class of type using the 2 parameter constructor of type IServiceManagement and ClientCredentials.
            return (TProxy)classType
                .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(ClientCredentials) })
                .Invoke(new object[] { serviceManagement, authCredentials.ClientCredentials });
        }

        private OrganizationDetailCollection DiscoverOrganizations(IDiscoveryService service)
        {
            if (service == null) throw new ArgumentNullException("service");
            RetrieveOrganizationsRequest orgRequest = new RetrieveOrganizationsRequest();
            RetrieveOrganizationsResponse orgResponse = (RetrieveOrganizationsResponse)service.Execute(orgRequest);

            return orgResponse.Details;
        }

        private List<D365Organization> RetrieveOrganizations(string userName, string password, string domain, string discoveryUrl)
        {
            List<D365Organization> result = null;
            IServiceManagement<IDiscoveryService> serviceManagement = ServiceConfigurationFactory.CreateManagement<IDiscoveryService>(new Uri(discoveryUrl + "/XRMServices/2011/Discovery.svc"));
            AuthenticationProviderType endpointType = serviceManagement.AuthenticationType;

            // Set the credentials.
            AuthenticationCredentials authCredentials = GetCredentials(userName, password, null, serviceManagement, endpointType);
            try
            {
                // Get the discovery service proxy.
                using (DiscoveryServiceProxy discoveryProxy = GetProxy<IDiscoveryService, DiscoveryServiceProxy>(serviceManagement, authCredentials))
                {
                    // Obtain organization information from the Discovery service. 
                    if (discoveryProxy != null)
                    {
                        // Obtain information about the organizations that the system user belongs to.
                        var orgs = DiscoverOrganizations(discoveryProxy);
                        if (orgs != null)
                        {
                            result = new List<D365Organization>();
                            foreach (OrganizationDetail o in orgs)
                            {
                                if (o.State == OrganizationState.Enabled)
                                    result.Add(new D365Organization
                                    {
                                        ConnectorUrl = discoveryUrl,
                                        Id = o.UniqueName,
                                        Name = o.FriendlyName
                                    });
                            }
                        }
                    }

                }
            }
            catch
            {
                // do nothing
            }

            return result;
        }

        public List<D365Organization> SearchForOrganizations(string userName, string password, string domain = null, string discoveryUrl = null)
        {
            List<D365Organization> result = null;

            if (discoveryUrl != null)
            {
                result = RetrieveOrganizations(userName, password, domain, discoveryUrl);
            }
            else
            {
                foreach (string discoURL in discoveryURLs)
                {
                    result = RetrieveOrganizations(userName, password, domain, discoURL);
                    if (result != null)
                        break;
                }
            }

            return result;
        }
    }
}