using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;

namespace Microsoft.Deployment.Site.Service.Controllers
{
    public class VersionController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetServiceVersion()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent(version, Encoding.UTF8, "text/plain");
            
            return result;
        }
    }
}