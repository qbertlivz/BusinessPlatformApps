using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.WindowsAzure.Storage;
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
            CloudStorageAccount csAcc = null;

            /*




                                const string odsAssetsStorageBaseUrl = "https://odsassets.blob.core.windows.net/public/release";
                    const string odsLatestVersionUrl = odsAssetsStorageBaseUrl + "/LatestVersion.txt";
                    const string targetConnectionString = "Data Source=pbisttest.database.windows.net;Initial Catalog=raymondb;User ID=pbiadmin;Password=Arius.325";


                    static void Main(string[] args)
                    {
                        CloudBlobClient blobClient = new CloudBlobClient(new Uri(odsAssetsStorageBaseUrl));
                        CloudBlob blob = (CloudBlob)blobClient.GetBlobReferenceFromServer(new Uri(odsLatestVersionUrl));

                        string latestVersion = string.Empty;
                        using (StreamReader sr = new StreamReader(blob.OpenRead()))
                        {
                            latestVersion = sr.ReadLine();
                        }

                        string sqlbacPack = $"{odsAssetsStorageBaseUrl}/{latestVersion}/EdFi_Ods_Populated_Template.bacpac";
                        blob = (CloudBlob)blobClient.GetBlobReferenceFromServer(new Uri(sqlbacPack));
                        DAC.DacServices dacService = new DAC.DacServices(targetConnectionString);
                        using (DAC.BacPackage bp = DAC.BacPackage.Load(blob.OpenRead()))
                        {
                            dacService.ImportBacpac(bp, "raymondb");
                        }
             */


            return new ActionResponse(ActionStatus.Success);
        }
    }
}
