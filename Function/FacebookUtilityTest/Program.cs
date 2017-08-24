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

            var pageContentTable = DataTableUtility.GetPageContentTable();
            var pageEngagementTable = DataTableUtility.GetPageEngagementTable();
            var pageImpressionsTable = DataTableUtility.GePageImpressionsTable();
            var pagePostsTable = DataTableUtility.GetPagePostsTable();
            var pagePostEngagement = DataTableUtility.GetPagePostEngagementTable();
            var pagePostImpressions = DataTableUtility.GetPagePostImpressionsTable();
            var pagePostReactionsTable = DataTableUtility.GetPagePostReactionsTable();
            var pagePostStoriesAndPeopleTalkingAboutThisTable = DataTableUtility.GetPagePostStoriesAndPeopleTalkingAboutThisTable();
            var pageReactionsTable = DataTableUtility.GetPageReactionsTable();
            var pageUserDemographicsTable = DataTableUtility.GetPageUserDemographicsTable();
            var pageVideoPosts = DataTableUtility.GetPageVideoPostsTable();
            var pageVideoViews = DataTableUtility.GetPageVideoViewsTable();
            var pageViewsTable = DataTableUtility.GetPageViewsTable();
            var clicksTable = DataTableUtility.GetPageClicksDataTable();
            var postsInfoTable = DataTableUtility.GetPostsInfoTable();

            var content = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageContent).Result;
            PageAnalyticsETL.PopulateNestedValues(pageContentTable, content, page);

            var engagement = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageEngagement).Result;
            PageAnalyticsETL.PopulateNestedValues(pageEngagementTable, engagement, page);

            var impressions = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageImpressions).Result;
            PageAnalyticsETL.PopulateNestedValues(pageImpressionsTable, impressions, page);

            var pagePosts = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PagePosts).Result;
            PageAnalyticsETL.PopulateNestedValues(pagePostsTable, pagePosts, page);

            var pagePostsEngagement = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PagePostEngagement).Result;
            PageAnalyticsETL.PopulateNestedValues(pagePostEngagement, pagePostsEngagement, page);

            var pagePostsReactions = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PagePostReactions).Result;
            PageAnalyticsETL.PopulateNestedValues(pagePostReactionsTable, pagePostsReactions, page);

            var pageReactions = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageReactions).Result;
            PageAnalyticsETL.PopulateNestedValues(pageReactionsTable, pageReactions, page);

            var pagePostStories = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PagePostStoriesAndPeopleTalkingAboutThis).Result;
            PageAnalyticsETL.PopulateNestedValues(pagePostStoriesAndPeopleTalkingAboutThisTable, pagePostStories, page);

            var pageUserDemographics = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageUserDemographics).Result;
            PageAnalyticsETL.PopulateNestedValues(pageUserDemographicsTable, pageUserDemographics, page);

            var pageVideoViewsObj = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageVideoViews).Result;
            PageAnalyticsETL.PopulateNestedValues(pageVideoViews, pageVideoViewsObj, page);

            var pageViews = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageViews).Result;
            PageAnalyticsETL.PopulateNestedValues(pageViewsTable, pageViews, page);

            var clicks = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageCtaClicks).Result;
            PageAnalyticsETL.PopulateNestedValues(clicksTable, clicks, page);

            var pagePostIds = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PagePostIds).Result;
            PageAnalyticsETL.PopulatePostsInfo(postsInfoTable, pagePostIds, page);
                        
            List<JObject> pagePostReactions = new List<JObject>();
            List<JObject> pageVideoPostsObj = new List<JObject>();
            List<JObject> pagePostsImpressions = new List<JObject>();
            if (pagePostIds != null)
            {
                foreach (var entry in pagePostIds)
                {
                    if (entry?["data"] != null)
                    {
                        foreach (var obj in entry["data"])
                        {
                            pagePostsImpressions.AddRange(FacebookUtility.GetPageMetricAnalytics(obj["id"].ToString(), until, accessToken, FacebookPageAnalyticsMetricGroups.PagePostImpressions).Result);
                            pageVideoPostsObj.AddRange(FacebookUtility.GetPageMetricAnalytics(obj["id"].ToString(), until, accessToken, FacebookPageAnalyticsMetricGroups.PageVideoPosts).Result);
                            pagePostReactions.AddRange(FacebookUtility.GetPageMetricAnalytics(obj["id"].ToString(), until, accessToken, FacebookPageAnalyticsMetricGroups.PagePostReactions).Result);
                        }
                    }
                }
            }
            PageAnalyticsETL.PopulateNestedValues(pagePostReactionsTable, pagePostReactions, page);
            PageAnalyticsETL.PopulateNestedValues(pageVideoPosts, pageVideoPostsObj, page);
            PageAnalyticsETL.PopulateNestedValues(pagePostImpressions, pagePostsImpressions, page);

            SqlUtility.BulkInsert(sqlConn, pageContentTable, schema + "." + "PageContent");
            SqlUtility.BulkInsert(sqlConn, pageEngagementTable, schema + "." + "PageEngagement");
            SqlUtility.BulkInsert(sqlConn, pageImpressionsTable, schema + "." + "PageImpressions");
            SqlUtility.BulkInsert(sqlConn, pagePostsTable, schema + "." + "PagePost");
            SqlUtility.BulkInsert(sqlConn, pagePostEngagement, schema + "." + "PagePostEngagement");
            SqlUtility.BulkInsert(sqlConn, pagePostImpressions, schema + "." + "PagePostImpressions");
            SqlUtility.BulkInsert(sqlConn, pagePostReactionsTable, schema + "." + "PagePostReactions");
            SqlUtility.BulkInsert(sqlConn, pagePostStoriesAndPeopleTalkingAboutThisTable, schema + "." + "PagePostStoriesAndPeopleTalkingAboutThis");
            SqlUtility.BulkInsert(sqlConn, pageReactionsTable, schema + "." + "PageReactions");
            SqlUtility.BulkInsert(sqlConn, pageUserDemographicsTable, schema + "." + "PageUserDemographics");
            SqlUtility.BulkInsert(sqlConn, pageVideoPosts, schema + "." + "PageVideoPosts");
            SqlUtility.BulkInsert(sqlConn, pageVideoViews, schema + "." + "PageVideoViews");
            SqlUtility.BulkInsert(sqlConn, pageViewsTable, schema + "." + "PageViews");
            SqlUtility.BulkInsert(sqlConn, clicksTable, schema + "." + "Clicks");
            SqlUtility.BulkInsert(sqlConn, postsInfoTable, schema + "." + "PagePostsInfo");
        }
    }
}
