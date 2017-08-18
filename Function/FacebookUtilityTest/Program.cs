using FacebookETL;
using FacebookUtillity;
using System;

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
            string until = DateTime.UtcNow.AddDays(-2).ToString();

            var clicks = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageCtaClicks).Result;
            PageAnalyticsETL.PopulateSingleValues(DataTableUtility.GetClicksDataTable(), clicks);

            var engagement = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageEngagement).Result;
            PageAnalyticsETL.PopulateNestedValues(DataTableUtility.GetEngagementTable(), engagement);

            var impressions = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageImpressions).Result;
            PageAnalyticsETL.PopulateNestedValues(DataTableUtility.GetImpressionsTable(), impressions);

            //var pagePostReactions = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PagePostReactions).Result;
            //PageAnalyticsETL.PopulateNestedValues(DataTableUtility.GetPagePost(), impressions);

            var pagePosts = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PagePosts).Result;
            PageAnalyticsETL.PopulateSingleValues(DataTableUtility.GetPagePostsTable(), pagePosts);

            var pagePostStories = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PagePostStoriesAndPeopleTalkingAboutThis).Result;
            PageAnalyticsETL.PopulateSingleValues(DataTableUtility.GetPagePostStoriesTable(), pagePostStories);

            var pageReactions = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageReactions).Result;
            PageAnalyticsETL.PopulateNestedValues(DataTableUtility.GetPageReactionsTable(), pageReactions);

            var pageUserDemographics = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageUserDemographics).Result;
            PageAnalyticsETL.PopulateSingleValues(DataTableUtility.GetPageUserDemographicsTable(), pageUserDemographics);

            var pageViews = FacebookUtility.GetPageMetricAnalytics(page, until, accessToken, FacebookPageAnalyticsMetricGroups.PageViews).Result;
            PageAnalyticsETL.PopulateNestedValues(DataTableUtility.GetPageViewsTable(), pageViews);
        }
    }
}
