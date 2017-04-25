using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
#pragma warning disable 649
    public class InformaticaConnectionAzureSql : InformaticaConnection
    {
        [JsonProperty("connParams", NullValueHandling = NullValueHandling.Ignore)]
        public InformaticaConnectionParameters ConnectionParameters = new InformaticaConnectionParameters();
    }
#pragma warning restore 649
}