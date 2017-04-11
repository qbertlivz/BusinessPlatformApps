using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
    class InformaticaLogin : InformaticaObject
    {
        [JsonProperty("username")]
        public string UserName;

        [JsonProperty("password")]
        public string Password;

        public InformaticaLogin()
        {
            Type = "login";
        }
    }
}