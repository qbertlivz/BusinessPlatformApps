using FacebookETL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace FacebookUtillity
{
    public class FacebookUtility
    {
        public static async Task<JObject> GetPage(string page, string accessToken)
        {
            string requestUri = $"https://graph.facebook.com/{page}?access_token={accessToken}";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(requestUri);
            string responseObj = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseObj);
        }

        public static async Task<List<JObject>> GetPostsAsync(string page, string untilDateTime, string accessToken)
        {
            List<JObject> posts = new List<JObject>();
            JObject post = null;
            string until = DateUtility.GetUnixFromDate(untilDateTime);
            string since = DateUtility.GetUnixFromDate(DateUtility.GetDateTimeRelativeFromNow(untilDateTime, -1));
            string requestUri = GetRequestUrlForPage(page, accessToken, until, since);
            do
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(requestUri);
                string responseObj = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }


                post = JObject.Parse(responseObj);
                posts.Add(post);

                if (post?["paging"] != null && post["paging"]?["cursors"] != null && post["paging"]["cursors"]?["after"] != null)
                {
                    string after = post["paging"]["cursors"]["after"].ToString();
                    requestUri = GetRequestUrlForPage(page, accessToken, until, since,after);

                }
                else if (post?["paging"] != null && post["paging"]?["next"] != null)
                {

                    requestUri = post["paging"]?["next"].ToString();
                }
            }
            while (post != null && post?["paging"] != null);

            return posts;

        }

        public static async Task<string> GetAccessTokenAsync(string clientId, string clientSecret)
        {
            string requestUri = $"https://graph.facebook.com/oauth/access_token?grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(requestUri);
            string responseObj = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            string accessToken = JObject.Parse(responseObj)["access_token"].ToString();
            return accessToken;
        }

        public static string GetRequestUrlForPage(string page, string accessToken, string until, string since, string after = "")
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("access_token", accessToken);
            param.Add("fields", "message,updated_time,created_time,comments.limit(100).order(chronological).summary(true),from,picture" +
                ",reactions.type(LOVE).limit(0).summary(total_count).as(reactions_love)" +
                ",reactions.type(WOW).limit(0).summary(total_count).as(reactions_wow)" +
                ",reactions.type(HAHA).limit(0).summary(total_count).as(reactions_haha)" +
                ",reactions.type(SAD).limit(0).summary(total_count).as(reactions_sad)" +
                ",reactions.type(ANGRY).limit(0).summary(total_count).as(reactions_angry)" +
                ",reactions.type(LIKE).limit(0).summary(total_count).as(reactions_like)");

            param.Add("until", until);
            param.Add("since", since);
            if (!string.IsNullOrEmpty(after))
            {
                param.Add("after", after);
            }

            return $"https://graph.facebook.com/v2.9/{page}/feed?" + GetQueryParameters(param);
        }

        private static string GetQueryParameters(Dictionary<string, string> queryParams)
        {
            string str = "";

            foreach (var queryParam in queryParams)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str += "&";
                }

                str += queryParam.Key + "="  + HttpUtility.UrlEncode(queryParam.Value);
            }

            return str;
        }
    }
}
