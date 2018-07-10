using Microsoft.Deployment.Common;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Deployment.Actions.Custom.Cuna
{
    [Export(typeof(IAction))]
    public class GetCunaApiAccessToken : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            try
            {
                string authority = String.Format(CultureInfo.InvariantCulture, Constants.CunaApiAadInstance, Constants.CunaApiAadTenant);

                X509Certificate2 cert = GetAadCertificate();

                ClientAssertionCertificate certCred = new ClientAssertionCertificate(Constants.CunaApiAadClientId, cert);
                AuthenticationContext authContext = new AuthenticationContext(authority);
                var result = await authContext.AcquireTokenAsync(Constants.CunaApiAadResourceId, certCred);

                request.DataStore.AddToDataStore("CunaApiAccessToken", result.AccessToken, DataStoreType.Private);
                return new ActionResponse(ActionStatus.Success);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Failed to get Cuna access token");
            }
        }

        private X509Certificate2 GetAadCertificate()
        {
            //Note -- The certificate should have private key with it

            X509Certificate2 cert = null;
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = store.Certificates;

            X509Certificate2Collection currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);

            X509Certificate2Collection signingCert = currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, Constants.CunaApiAadCertificateName, false);

            cert = signingCert.OfType<X509Certificate2>().OrderByDescending(c => c.NotBefore).FirstOrDefault();
            store.Close();
            return cert;
        }

    }
}
