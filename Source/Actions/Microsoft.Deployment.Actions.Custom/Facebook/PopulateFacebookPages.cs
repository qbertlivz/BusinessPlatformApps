using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Deployment.Common.ActionModel;
using Microsoft.Deployment.Common.Actions;
using Microsoft.Deployment.Common.Helpers;

namespace Microsoft.Deployment.Actions.Custom.Facebook
{
    [Export(typeof(IAction))]
    public class PopulateFacebookPages : BaseAction
    {
        public override async Task<ActionResponse> ExecuteActionAsync(ActionRequest request)
        {
            string page = request.DataStore.GetValue("FacebookPageId");
            string permanentPageToken = request.DataStore.GetValue("FacebookPageToken");
            string connString = request.DataStore.GetValue("SqlConnectionString");

            string pageName = await GetPageName(page, permanentPageToken);

            SqlUtility.RunCommand(connString, GetInsertCommand(page, pageName), Common.Enums.SqlCommandType.ExecuteWithoutData);

            return new ActionResponse(ActionStatus.Success);

        }

        public string GetInsertCommand(string pageId, string pageName)
        {
            var cmd = $"INSERT fbpa.PageTable(idpage, name) VALUES('{pageId}','{pageName}')";
            return cmd;
        }

        public async Task<string> GetPageName(string pageId, string token)
        {
            string pageRequestUri = $"https://graph.facebook.com/v2.10/{pageId}/?access_token={token}";
            HttpResponseMessage response;
            string responseObj;
            string name = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                response = await client.GetAsync(pageRequestUri);
                responseObj = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    name = JsonUtility.GetJObjectFromJsonString(responseObj)["name"].ToString();
                }

                else name = string.Empty;
            }

            return name;
        }
    }
}
