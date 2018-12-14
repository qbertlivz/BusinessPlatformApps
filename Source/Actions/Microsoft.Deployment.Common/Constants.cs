namespace Microsoft.Deployment.Common
{
    public static class Constants
    {
        // Azure ARM
        public const int ACTION_WAIT_INTERVAL = 5000; // in milliseconds
        // END Azure ARM

        public const string AppsPath = "Apps";
        public const string AppsWebPath = "Web";
        public const string SiteCommonPath = "SiteCommon";
        public const string ServiceUrl = "https://bpstservice.azurewebsites.net/";

        public const string ActionsPath = "Actions";
        public const string InitFile = "init.json";
        public const string BinPath = "bin";

        public const string AzureManagementApi = "https://management.azure.com/";
        public const string AzureManagementCoreApi = "https://management.core.windows.net/";
        public const string AzureKeyVaultApi = "https://vault.azure.net"; // Do not add a trailing slash
        public const string AzureWebSite = ".scm.azurewebsites.net/";
        public const string PowerBIService = "https://analysis.windows.net/PowerBI/api";

        public const string AzureAuthUri = "https://login.microsoftonline.com/{0}/oauth2/authorize?";
        public const string AzureTokenUri = "https://login.microsoftonline.com/{0}/oauth2/token";

        public static string MicrosoftClientId = "";
        public static string ASClientId = "";
        public static string MicrosoftClientIdCrm = "";
        public static string MicrosoftClientIdPowerBI = "";
        public const string MicrosoftClientSecret = "";
        public const string WebsiteRedirectPath = "/redirect.html";
        public const string AppInsightsKey = "app_insights_key";

        // Notifications
        public static string BpstDeploymentIdDatabase = string.Empty;
        public static string BpstNotifierUrl = "";

        public static string Office365ClientId = "";

        // Ax
        // TODO: Temporary workaround to make use of different Ax AAD App
        public static string AxClientId = "";
        public static string AxErpResource = "";
        public static string AxLocatorClientId = string.Empty;
        public static string AxLocatorSecret = string.Empty;
        public const string AxLocatorBaseUrl = "https://infra.locator.dynamics.com";
        public const string AxLocatorLoginAuthority = "https://login.windows.net/{0}";

        // Crm
        public const string CrmProfileName = "bpst-mscrm-profile";
        public const string MsCrmAuthority = "https://login.windows.net/common/oauth2/authorize?";
        public static string MsCrmClientId = "";
        public static string MsCrmResource = "";
        public const string MsCrmToken = "https://login.windows.net/common/oauth2/token";

        // Facebook
        public const string FacebookGraphUrl = "https://graph.facebook.com";
        public static string FacebookClientId = "";
        public static string FacebookClientSecret = string.Empty;

        // Informatica
        public static string InformaticaRegistrationCode = string.Empty;

        // SocialGist (Reddit)
        public const string SocialGistProvisionKeyUrl = "https://api.boardreader.com/v1/Keys/List";
        public static string SocialGistProvisionKeyUserName = string.Empty;
        public static string SocialGistProvisionKeyPassphrase = string.Empty;

        // CDSA BLOB Storage
        public const string CustCollectionsBIMeasurements = "CustCollectionsBIMeasurements";

        // Cuna
        public static string CunaTokenUrl = string.Empty;
        public static string CunaApiUrl = string.Empty;
        public static string CunaApiAadInstance = string.Empty;
        public static string CunaApiAadTenantId = string.Empty;
        public static string CunaApiAadClientId = string.Empty;
        public static string CunaApiAadResourceId = string.Empty;
        public static string CunaApiAadSecret = string.Empty;
        public static string CunaTokenValidateCertificate = string.Empty;

        // PowerBI
        public const string PowerBiApiUrl = "https://api.powerbi.com/";

        // Simplement
        public static string SimplementBlobStorage = string.Empty;
        public static string SimplementSasToken = string.Empty;
    }
}
