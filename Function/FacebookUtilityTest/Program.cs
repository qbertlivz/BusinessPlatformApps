using FacebookETL;
using FacebookUtillity;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace FacebookUtilityTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //string sqlConnectionString = "Server = tcp:modb1.database.windows.net,1433; Initial Catalog = fb; Persist Security Info = False; User ID = pbiadmin; Password = Corp123!; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";
            //string cognitiveKey = "8f5837f6f20c4374b93d171c490fa58c";
            //string schema = "[fb]";
            //string facebookClientId = "421651141539199";
            //string facebookClientSecret = "511941c6bb0aa06afb250fc5c8628f95";

            //string date = DateTime.Now.AddDays(-2).ToString();

            //var test = MainETL.PopulateAll(sqlConnectionString, schema, cognitiveKey, facebookClientId, facebookClientSecret, date).Result;

            string page = "";
            string accessToken = "";
            string sqlConn = "";
            string schema = "";
            string until = DateTime.UtcNow.AddDays(-2).ToString();

            var clicksTable = DataTableUtility.GetClicksDataTable();
            var engagementTable = DataTableUtility.GetEngagementTable();
            var impressionsTable = DataTableUtility.GetImpressionsTable();
            var pagePostReactionsTable = DataTableUtility.GetPagePostReactionsTable();
            var pagePostsTable = DataTableUtility.GetPagePostsTable();
            var pagePostStoriesTable = DataTableUtility.GetPagePostStoriesTable();
            var pageReactionsTable = DataTableUtility.GetPageReactionsTable();
            var pageUserDemographicsTable = DataTableUtility.GetPageUserDemographicsTable();
            var pageViewsTable = DataTableUtility.GetPageViewsTable();

            var clicks = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageCtaClicks).Result;
            PageAnalyticsETL.PopulateSingleValues(clicksTable, clicks, page);

            var engagement = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageEngagement).Result;
            PageAnalyticsETL.PopulateNestedValues(engagementTable, engagement, page);

            var impressions = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageImpressions).Result;
            PageAnalyticsETL.PopulateNestedValues(impressionsTable, impressions, page);

            var pagePostIds = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PagePostIds).Result;
            List<JObject> pagePostReactions = new List<JObject>();
            if (pagePostIds != null)
            {
                foreach (var entry in pagePostIds)
                {
                    if (entry?["data"] != null)
                    {
                        foreach (var obj in entry["data"])
                        {
                            pagePostReactions.AddRange(FacebookUtility.GetPageMetricAnalytics(obj["id"].ToString(), until, accessToken, FacebookPageAnalyticsMetricGroups.PagePostReactions).Result);
                        }
                    }
                }
            }
            PageAnalyticsETL.PopulateNestedValues(pagePostReactionsTable, pagePostReactions, page);

            var pagePosts = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PagePosts).Result;
            PageAnalyticsETL.PopulateNestedValues(pagePostsTable, pagePosts, page);

            var pagePostStories = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PagePostStoriesAndPeopleTalkingAboutThis).Result;
            PageAnalyticsETL.PopulateNestedValues(pagePostStoriesTable, pagePostStories, page);

            var pageReactions = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageReactions).Result;
            PageAnalyticsETL.PopulateNestedValues(pageReactionsTable, pageReactions, page);

            var pageUserDemographics = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageUserDemographics).Result;
            PageAnalyticsETL.PopulateNestedValues(pageUserDemographicsTable, pageUserDemographics, page);

            var pageViews = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageViews).Result;
            PageAnalyticsETL.PopulateNestedValues(pageViewsTable, pageViews, page);

            SqlUtility.BulkInsert(sqlConn, clicksTable, schema + "." + "Clicks");
            SqlUtility.BulkInsert(sqlConn, engagementTable, schema + "." + "Engagement");
            SqlUtility.BulkInsert(sqlConn, impressionsTable, schema + "." + "Impressions");
            SqlUtility.BulkInsert(sqlConn, pagePostReactionsTable, schema + "." + "PagePostReactions");
            SqlUtility.BulkInsert(sqlConn, pagePostsTable, schema + "." + "PagePosts");
            SqlUtility.BulkInsert(sqlConn, pagePostStoriesTable, schema + "." + "PagePostStories");
            SqlUtility.BulkInsert(sqlConn, pageReactionsTable, schema + "." + "PageReactions");
            SqlUtility.BulkInsert(sqlConn, pageUserDemographicsTable, schema + "." + "PageUserDemographics");
            SqlUtility.BulkInsert(sqlConn, pageViewsTable, schema + "." + "PageViews");
        }
    }
}
