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
                    requestUri = GetRequestUrlForPage(page, accessToken, until, since, after);

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

        #region Page Analytics
        public static async Task<List<JObject>> GetPageMetricAnalytics(string page, string untilDateTime, string accessToken, string metricsGroup)
        {
            List<JObject> posts = new List<JObject>();
            JObject post = null;
            string until = DateUtility.GetUnixFromDate(untilDateTime);
            string since = DateUtility.GetUnixFromDate(DateUtility.GetDateTimeRelativeFromNow(untilDateTime, -1));
            string requestUri = string.Empty;

            switch (metricsGroup)
            {
                case FacebookPageAnalyticsMetricGroups.PageUserDemographics:
                    requestUri = GetPageUserDemographics(page, accessToken, until, since);
                    break;
                case FacebookPageAnalyticsMetricGroups.PageEngagement:
                    requestUri = GetPageEngagement(page, accessToken, until, since);
                    break;
                case FacebookPageAnalyticsMetricGroups.PageImpressions:
                    requestUri = GetPageImpressions(page, accessToken, until, since);
                    break;
                case FacebookPageAnalyticsMetricGroups.PageViews:
                    requestUri = GetPageViews(page, accessToken, until, since);
                    break;
                case FacebookPageAnalyticsMetricGroups.PageCtaClicks:
                    requestUri = GetPageCtaClicks(page, accessToken, until, since);
                    break;
                case FacebookPageAnalyticsMetricGroups.PagePostStoriesAndPeopleTalkingAboutThis:
                    requestUri = GetPagePostStoriesAndPeopleTalkingAboutThis(page, accessToken, until, since);
                    break;
                case FacebookPageAnalyticsMetricGroups.PageReactions:
                    requestUri = GetPageReactions(page, accessToken, until, since);
                    break;
                case FacebookPageAnalyticsMetricGroups.PagePosts:
                    requestUri = GetPagePosts(page, accessToken, until, since);
                    break;
                case FacebookPageAnalyticsMetricGroups.PagePostReactions:
                    requestUri = GetPagePostReactions(page, accessToken, until, since);
                    break;
                case FacebookPageAnalyticsMetricGroups.PagePostIds:
                    requestUri = GetPagePostIds(page, accessToken, untilDateTime);
                    break;
                default: break;
            }

            do
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(requestUri);
                    string responseObj = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }

                    post = JObject.Parse(responseObj);

                    if (!posts.Contains(post) && (post?["data"] as JArray).Count > 0)
                    {
                        posts.Add(post);
                    }

                    if (post?["paging"] != null && post["paging"]?["cursors"] != null && post["paging"]["cursors"]?["after"] != null)
                    {
                        string after = post["paging"]["cursors"]["after"].ToString();
                        requestUri = GetRequestUrlForPage(page, accessToken, until, since, after);

                    }
                    else if (post?["paging"] != null && post["paging"]?["next"] != null)
                    {
                        requestUri = post["paging"]?["next"].ToString();
                    }
                }

                if (post?["data"] != null && (post?["data"] as JArray).Count > 0)
                {
                    var endTime = post?["data"]?[0]?["values"]?[0]?["end_time"];

                    if (endTime != null)
                    {
                        var et = DateTime.Parse(endTime.ToString());
                        var untilTime = DateTime.Parse(untilDateTime);
                        if (untilTime.Subtract(et).TotalDays < 1)
                        {
                            break;
                        }
                    }
                }

                if (post?["data"]?[0]?["period"] != null && post?["data"]?[0]?["period"].ToString().ToLower() == "lifetime")
                {
                    break;
                }
            }
            while (post != null && post?["paging"] != null && post["paging"]?["next"] != null);

            return posts;
        }

        private static string GetPagePostIds(string page, string accessToken, string untilDateTime, string after = "")
        {
            string until = DateUtility.GetUnixFromDate(untilDateTime);
            string since = DateUtility.GetUnixFromDate(DateUtility.GetDateTimeRelativeFromNow(untilDateTime, -2));
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("access_token", accessToken);
            param.Add("fields", "id");
            param.Add("until", until);

            param.Add("since", since);

            if (!string.IsNullOrEmpty(after))
            {
                param.Add("after", after);
            }

            return $"https://graph.facebook.com/v2.10/{page}/posts?" + GetQueryParameters(param);
        }

        private static string GetPagePostReactions(string page_postId, string accessToken, string until, string since, string after = "")
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("access_token", accessToken);
            param.Add("metric", "post_reactions_by_type_total");
            param.Add("until", until);
            param.Add("since", since);

            if (!string.IsNullOrEmpty(after))
            {
                param.Add("after", after);
            }

            return $"https://graph.facebook.com/v2.10/{page_postId}/insights?" + GetQueryParameters(param);
        }

        private static string GetPagePosts(string page, string accessToken, string until, string since, string after = "")
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("access_token", accessToken);
            param.Add("metric", "page_stories, page_storytellers, page_impressions_frequency_distribution, page_impressions_unique, page_fan_adds_unique, page_consumptions_unique");

            param.Add("until", until);
            param.Add("since", since);

            if (!string.IsNullOrEmpty(after))
            {
                param.Add("after", after);
            }

            return $"https://graph.facebook.com/v2.10/{page}/insights?" + GetQueryParameters(param);
        }

        private static string GetPageReactions(string page, string accessToken, string until, string since, string after = "")
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("access_token", accessToken);
            param.Add("metric", "page_actions_post_reactions_total");

            param.Add("until", until);
            param.Add("since", since);

            if (!string.IsNullOrEmpty(after))
            {
                param.Add("after", after);
            }

            return $"https://graph.facebook.com/v2.10/{page}/insights?" + GetQueryParameters(param);
        }

        private static string GetPagePostStoriesAndPeopleTalkingAboutThis(string page, string accessToken, string until, string since, string after = "")
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("access_token", accessToken);
            param.Add("metric", "page_stories, page_storytellers, page_stories_by_story_type, page_storytellers_by_story_type");

            param.Add("until", until);
            param.Add("since", since);

            if (!string.IsNullOrEmpty(after))
            {
                param.Add("after", after);
            }

            return $"https://graph.facebook.com/v2.10/{page}/insights?" + GetQueryParameters(param);
        }

        private static string GetPageCtaClicks(string page, string accessToken, string until, string since, string after = "")
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("access_token", accessToken);
            param.Add("metric", "page_total_actions, page_call_phone_clicks_logged_in_unique, page_get_directions_clicks_logged_in_unique, page_website_clicks_logged_in_unique");

            param.Add("until", until);
            param.Add("since", since);

            if (!string.IsNullOrEmpty(after))
            {
                param.Add("after", after);
            }

            return $"https://graph.facebook.com/v2.10/{page}/insights?" + GetQueryParameters(param);
        }

        private static string GetPageViews(string page, string accessToken, string until, string since, string after = "")
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("access_token", accessToken);
            param.Add("metric", "page_views_total, page_views_external_referrals, page_views_by_profile_tab_total, page_views_logged_in_unique, page_views_by_profile_tab_logged_in_unique, page_views_by_site_logged_in_unique, page_views_logout");

            param.Add("until", until);
            param.Add("since", since);

            if (!string.IsNullOrEmpty(after))
            {
                param.Add("after", after);
            }

            return $"https://graph.facebook.com/v2.10/{page}/insights?" + GetQueryParameters(param);
        }

        private static string GetPageImpressions(string page, string accessToken, string until, string since, string after = "")
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("access_token", accessToken);
            param.Add("metric", "page_impressions_unique, page_impressions_paid_unique, page_impressions_organic_unique, page_impressions_viral_unique, page_impressions_by_story_type_unique, page_impressions_viral_frequency_distribution, page_impressions_by_paid_non_paid_unique, page_impressions_frequency_distribution");

            param.Add("until", until);
            param.Add("since", since);

            if (!string.IsNullOrEmpty(after))
            {
                param.Add("after", after);
            }

            return $"https://graph.facebook.com/v2.10/{page}/insights?" + GetQueryParameters(param);
        }

        private static string GetPageEngagement(string page, string accessToken, string until, string since, string after = "")
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("access_token", accessToken);
            param.Add("metric", "page_engaged_users,page_fans_online,page_consumptions_by_consumption_type_unique,page_consumptions_unique,page_positive_feedback_by_type_unique,page_negative_feedback_unique,page_negative_feedback_by_type_unique,page_places_checkin_total_unique,page_places_checkin_mobile_unique");

            param.Add("until", until);
            param.Add("since", since);

            if (!string.IsNullOrEmpty(after))
            {
                param.Add("after", after);
            }

            return $"https://graph.facebook.com/v2.10/{page}/insights?" + GetQueryParameters(param);
        }

        public static string GetPageUserDemographics(string page, string accessToken, string until, string since, string after = "")
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("access_token", accessToken);
            param.Add("metric", "page_fans,page_fan_adds_unique,page_fan_removes_unique,page_fans_by_like_source_unique, page_fans_by_unlike_source_unique");

            param.Add("until", until);
            param.Add("since", since);

            if (!string.IsNullOrEmpty(after))
            {
                param.Add("after", after);
            }

            return $"https://graph.facebook.com/v2.10/{page}/insights?" + GetQueryParameters(param);
        }
        #endregion

        private static string GetQueryParameters(Dictionary<string, string> queryParams)
        {
            string str = "";

            foreach (var queryParam in queryParams)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str += "&";
                }

                str += queryParam.Key + "=" + HttpUtility.UrlEncode(queryParam.Value);
            }

            return str;
        }
    }
}