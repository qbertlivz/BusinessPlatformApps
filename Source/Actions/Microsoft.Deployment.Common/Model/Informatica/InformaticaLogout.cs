using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    class InformaticaLogout : InformaticaObject
    {
        [JsonProperty("username")]
        public string UserName;

        [JsonProperty("password")]
        public string Password;

        public InformaticaLogout()
        {
            Type = "logout";
        }
    }
#pragma warning restore 649
}