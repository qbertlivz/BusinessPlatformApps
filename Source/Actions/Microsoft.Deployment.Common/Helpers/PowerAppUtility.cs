using Microsoft.Deployment.Common.ActionModel;

namespace Microsoft.Deployment.Common.Helpers
{
    public class PowerAppUtility
    {
        public const int SQL_CONNECTION_ID_LENGTH = 32;

        public const string URL_POWERAPPS_ENVIRONMENTS = "https://management.azure.com/providers/Microsoft.PowerApps/environments?api-version=2016-11-01&$filter=minimumAppPermission%20eq%20%27CanEdit%27&$expand=Permissions&_poll=true";
        public const string URL_POWERAPPS_GENERATE_RESOURCE_STORAGE = "https://management.azure.com/providers/Microsoft.PowerApps/objectIds/{0}/generateResourceStorage?api-version=2017-05-01";
        public const string URL_POWERAPPS_SQL_CONNECTION = "https://management.azure.com/providers/Microsoft.PowerApps/apis/shared_sql/connections/{0}?api-version=2016-11-01&$filter=environment%20eq%20%27{1}%27";

        public static void SkipPowerApp(DataStore ds)
        {
            if (ds.GetValue("SkipPowerApp") == null)
            {
                ds.AddToDataStore("SkipPowerApp", "true", DataStoreType.Public);
            }
        }
    }
}