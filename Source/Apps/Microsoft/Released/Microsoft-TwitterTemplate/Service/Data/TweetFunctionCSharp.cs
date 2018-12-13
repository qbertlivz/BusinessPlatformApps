﻿#r "Newtonsoft.Json"
#r "System.Data"

using System;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web;


public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
    TweetHandler tweetHandler = new TweetHandler(connectionString, log);
    string jsonContent = await req.Content.ReadAsStringAsync();
    var tweets = JsonConvert.DeserializeObject(jsonContent);
    int i = 0;
    if (tweets is JArray)
    {
        foreach (var item in (JArray)tweets)
        {
            var individualtweet = item.ToString();
            //log.Info("********************Run**************************" + individualtweet.ToString());

            await tweetHandler.ParseTweet(individualtweet, log, i);
            i = i + 1;
        }
    }
    else
    {
        //log.Info("********************Run**************************" + jsonContent.ToString());
        await tweetHandler.ParseTweet(jsonContent, log, i);
        i = i + 1;
    }

    // log.Info($"{data}");
    return req.CreateResponse(HttpStatusCode.OK, "");
}

public class TweetHandler
{
    public static string SanitizeSimpleName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        Regex invalidCharacters = new Regex("[^a-zA-Z_0-9]");
        return invalidCharacters.Replace(value, string.Empty);
    }

    public static SqlParameter[] MapValuesToSqlParameters(params object[] list)
    {
        if (list == null)
            return null;

        SqlParameter[] result = new SqlParameter[list.Length];
        for (int i = 0; i < list.Length; i++)
        {
            result[i] = new SqlParameter();
            if (list[i] is int)
                result[i].DbType = DbType.Int32;
            else if (list[i] is uint)
                result[i].DbType = DbType.UInt32;
            else if (list[i] is long)
                result[i].DbType = DbType.Int64;
            else if (list[i] is ulong)
                result[i].DbType = DbType.UInt64;
            else if (list[i] is short)
                result[i].DbType = DbType.Int16;
            else if (list[i] is ushort)
                result[i].DbType = DbType.UInt16;
            else if (list[i] is byte)
                result[i].DbType = DbType.Byte;
            else if (list[i] is float)                     // Floating types
                result[i].DbType = DbType.Single;
            else if (list[i] is double)
                result[i].DbType = DbType.Double;
            else if (list[i] is DateTime)                  // Date and time
                result[i].DbType = DbType.DateTime;
            else if (list[i] is bool)                      // Boolean
                result[i].DbType = DbType.Boolean;
            else if (list[i] is char || list[i] is string) // Character type
                result[i].DbType = DbType.String;
            else
                throw new Exception("Unexpected data type"); // OUR code should not use other types

            result[i].Value = list[i];
            result[i].ParameterName = $"@p{i + 1}"; // SqlClient doesn't accept anonymous parameters (expecting queries to use 1 based numbering for parameter names: @p1, @p2, etc...)
        }


        return result;
    }

    public TweetHandler(string connection, TraceWriter log)
    {
        if (string.IsNullOrEmpty(connection))
        {
            throw new ArgumentNullException("connection", "Connection string is null or empty.");
        }
        this.connectionString = connection;
        this.log = log;
    }

    private dynamic tweet = string.Empty;
    private JObject tweetObj = new JObject();
    private string connectionString;
    private TraceWriter log = null;

    //Create dictionaries for SQL tables
    private Dictionary<string, string> originalTweets = new Dictionary<string, string>()
        {
            {"masterid", null},
            {"tweet", null},
            {"twitterhandle", null},
            {"sentiment", null},
            {"lang", null},
            {"sentimentBin", null},
            {"sentimentPosNeg", null},
            {"accounttag", "Unknown"}
        };

    private Dictionary<string, string> processedTweets = new Dictionary<string, string>()
        {
            {"tweetid", null},
            {"masterid", null},
            {"image_url", null},
            {"dateorig", null},
            {"authorimage_url", null},
            {"username", null},
            {"hourofdate", null},
            {"minuteofdate", null},
            {"direction", "Text"},
            {"favorited", "1"},
            {"retweet", "False"},
            {"user_followers", null},
            {"user_friends", null},
            {"user_favorites", null},
            {"user_totaltweets", null}
        };

    private Dictionary<string, string> hashtagSlicer = new Dictionary<string, string>()
        {
            {"tweetid", null},
            {"facet", null}
        };

    private Dictionary<string, string> mentionSlicer = new Dictionary<string, string>()
        {
            {"tweetid", null},
            {"facet", null}
        };

    private Dictionary<string, string> authorHashtagGraph = new Dictionary<string, string>()
        {
            {"tweetid", null},
            {"author", null},
            {"authorColor", "#01B8AA"},
            {"hashtag", null},
            {"hashtagColor", "#374649"},
        };

    private Dictionary<string, string> authorMentionGraph = new Dictionary<string, string>()
        {
            {"tweetid", null},
            {"author", null},
            {"authorColor", "#01B8AA"},
            {"mention", null},
            {"mentionColor", "#374649"},
        };

    public async Task<bool> ParseTweet(string entireTweet, TraceWriter log, int i)
    {
        SqlParameter[] parameters;
        // Convert JSON to dynamic C# object

        if (string.IsNullOrEmpty(entireTweet))
            return true;

        tweetObj = JObject.Parse(entireTweet);
        tweet = tweetObj;

        //Connect to Azure SQL Database & bring in Twitter Handles & IDs
        string twitterHandles = (string)ExecuteSqlScalar("SELECT [value] FROM pbist_twitter.configuration where name = \'twitterHandle\'");
        string twitterHandleId = (string)ExecuteSqlScalar("SELECT [value] FROM pbist_twitter.configuration where name = \'twitterHandleId\'");
        if (i == 0)
        {
            parameters = MapValuesToSqlParameters(new object[] { tweet["TweetId"].ToString() });
            ExecuteSqlNonQuery($"UPDATE pbist_twitter.twitter_query SET TweetId=@p1 WHERE Id = 1", parameters);
        }

        // Split out all the handles & create dictionary
        String[] handle = null;
        String[] handleId = null;
        var dictionary = new Dictionary<string, string>();

        if (twitterHandles != String.Empty)
        {
            handle = SplitHandles(twitterHandles, ',');
            handleId = SplitHandles(twitterHandleId, ',');
            for (int index = 0; index < handle.Length; index++)
            {
                dictionary.Add(handle[index], handleId[index]);
            }
        }

        //log.Info("********************ParseTweet************************** TweetText: " + tweet.TweetText.ToString());
        //log.Info("********************ParseTweet************************** Handles: " + twitterHandles);
        //log.Info("********************ParseTweet************************** Handle IDs: " + twitterHandleId);
        //log.Info("********************ParseTweet************************** Tweet Language: " + tweet.TweetLanguageCode.ToString());
        // Check if language of tweet is supported for sentiment analysis

        originalTweets["lang"] = tweet.TweetLanguageCode.ToString();
        originalTweets["sentimentPosNeg"] = "Undefined";
        originalTweets["sentiment"] = null;
        originalTweets["sentimentBin"] = null;
        
		string sentiment = await MakeSentimentRequest(tweet);

		
		if (sentiment != null)
        {
            sentiment = (double.Parse(sentiment) * 2 - 1).ToString(CultureInfo.InvariantCulture);
            string sentimentBin = (Math.Floor(double.Parse(sentiment) * 10) / 10).ToString(CultureInfo.InvariantCulture);
            string sentimentPosNeg = String.Empty;
            if (double.Parse(sentimentBin) > 0.1)
            {
                sentimentPosNeg = "Positive";
            }
            else if (double.Parse(sentimentBin) < -0.1)
            {
                sentimentPosNeg = "Negative";
            }
            else
            {
                sentimentPosNeg = "Neutral";
            }
            //Save sentiment and language metadata into dictionary
            originalTweets["sentiment"] = sentiment;
            originalTweets["sentimentBin"] = sentimentBin;
            originalTweets["sentimentPosNeg"] = sentimentPosNeg;
        }

        // Work out account and tweet direction for retweets
        if (tweet.OriginalTweet != null)
        {
            processedTweets["direction"] = "Text Retweet";
            originalTweets["twitterhandle"] = tweet.OriginalTweet.UserDetails.UserName;
            if (dictionary.Count > 0)
            {
                foreach (var entry in dictionary)
                {
                    HashtagDirectionCheck(entry, " Retweet");
                    MessageDirectionCheck(entry, tweetObj.SelectToken("OriginalTweet.UserMentions"), " Retweet");
                }
            }
            // Save retweets into SQL table
            saveTweets(tweet.OriginalTweet);
        }
        // Works out the tweet direction for original tweets (not retweets)
        else
        {
            originalTweets["twitterhandle"] = tweet.UserDetails.UserName;
            if (dictionary.Count > 0)
            {
                foreach (var entry in dictionary)
                {
                    HashtagDirectionCheck(entry);
                    MessageDirectionCheck(entry, tweetObj.SelectToken("UserMentions"));
                }
            }

            // Save original tweets into SQL Table
            saveTweets(tweet);
        }

        processedTweets["tweetid"] = tweet.TweetId;

        //Save time metadata about processed tweets
        string createdat = tweet.CreatedAt.ToString();
        DateTime ts = DateTime.ParseExact(createdat, "ddd MMM dd HH:mm:ss +ffff yyyy", CultureInfo.CurrentCulture);
        processedTweets["dateorig"] = DateTime.Parse(ts.Year.ToString() + " " + ts.Month.ToString() + " " + ts.Day.ToString() + " " + ts.Hour.ToString() + ":" + ts.Minute.ToString() + ":" + ts.Second.ToString()).ToString(CultureInfo.InvariantCulture);
        processedTweets["minuteofdate"] = DateTime.Parse(ts.Year.ToString() + " " + ts.Month.ToString() + " " + ts.Day.ToString() + " " + ts.Hour.ToString() + ":" + ts.Minute.ToString() + ":00").ToString(CultureInfo.InvariantCulture);
        processedTweets["hourofdate"] = DateTime.Parse(ts.Year.ToString() + " " + ts.Month.ToString() + " " + ts.Day.ToString() + " " + ts.Hour.ToString() + ":00:00").ToString(CultureInfo.InvariantCulture);


        //Save media and follower metadata about processed tweets
        processedTweets["authorimage_url"] = tweet.UserDetails.ProfileImageUrl;
        processedTweets["username"] = tweet.UserDetails.UserName;
        processedTweets["user_followers"] = tweet.UserDetails.FollowersCount;
        processedTweets["user_friends"] = tweet.UserDetails.FavouritesCount;
        processedTweets["user_favorites"] = tweet.UserDetails.FriendsCount;
        processedTweets["user_totaltweets"] = tweet.UserDetails.StatusesCount;

        string firstUrl = String.Empty;

        if (tweetObj.SelectToken("MediaUrls") != null && tweetObj.SelectToken("MediaUrls").HasValues)
        {
            firstUrl = tweet.MediaUrls[0];
            if (firstUrl != String.Empty)
            {
                processedTweets["image_url"] = firstUrl;
            }
        }

        if (tweet.favorited != "true")
        {
            processedTweets["favorited"] = "1";
        }

        if (tweet.OriginalTweet != null)
        {
            processedTweets["retweet"] = "True";
        }

        //Save processed tweets into SQL
        parameters = MapValuesToSqlParameters(new object[] { processedTweets["tweetid"].ToString() });
        int response = (int)ExecuteSqlScalar($"SELECT count(*) FROM pbist_twitter.tweets_processed WHERE tweetid = @p1");
        if (response == 0)
        {
            try
            {
                parameters = MapValuesToSqlParameters(processedTweets.Values.ToArray());
                ExecuteSqlNonQuery(generateInsertSQLQuery("pbist_twitter.tweets_processed", processedTweets), parameters);
            }
            catch (Exception e)
            {

            }
        }

        string text = tweet.TweetText.ToString();
        //Populate hashtag slicer table
        if (text.Contains("#"))
        {
            hashtagSlicer["tweetid"] = tweet.TweetId;
            hashtagmentions(text, '#', "facet", "pbist_twitter.hashtag_slicer", hashtagSlicer);
        }

        //Populate author hashtag network table
        if (text.Contains("#"))
        {
            authorHashtagGraph["tweetid"] = tweet.TweetId;
            authorHashtagGraph["author"] = tweet.UserDetails.UserName;
            hashtagmentions(text, '#', "hashtag", "pbist_twitter.authorhashtag_graph", authorHashtagGraph);
        }

        //Populate mention slicer table
        if (text.Contains("@"))
        {
            mentionSlicer["tweetid"] = tweet.TweetId;
            hashtagmentions(text, '@', "facet", "pbist_twitter.mention_slicer", mentionSlicer);
        }

        //Populate author mention network table
        if (text.Contains("@"))
        {
            authorMentionGraph["tweetid"] = tweet.TweetId;
            authorMentionGraph["author"] = tweet.UserDetails.UserName;
            hashtagmentions(text, '@', "mention", "pbist_twitter.authormention_graph", authorMentionGraph);
        }

        return true;
    }

    //Execute SQL query without returning anything
    private void ExecuteSqlNonQuery(string sqlQuery, SqlParameter[] parameters = null)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(sqlQuery, connection) { CommandTimeout = 0 })
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                //log.Info("*********************************ExecuteSqlNonQuery************************** Query: " + sqlQuery);
                command.ExecuteNonQuery();
            }
        }
    }

     //Execute SQL query using a reader
    private object ExecuteSqlScalar(string sqlQuery, SqlParameter[] parameters = null)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(sqlQuery, connection) { CommandTimeout = 0 })
            {
                object result = command.ExecuteScalar();
                return (result != null && result != DBNull.Value) ? result.ToString() : null;
            }
        }
    }

    //Write tweets into SQL
    private void saveTweets(dynamic tweetType)
    {
        originalTweets["masterid"] = tweetType.TweetId.ToString();
        processedTweets["masterid"] = tweetType.TweetId.ToString();
        originalTweets["tweet"] = tweetType.TweetText.ToString();

        SqlParameter[] parameters = MapValuesToSqlParameters(new object[] { originalTweets["masterid"].ToString() });
        int response = (int)ExecuteSqlScalar("Select count(*) FROM pbist_twitter.tweets_normalized WHERE masterid = @p1", parameters);
        if (response == 0)
        {
            try
            {
                parameters = MapValuesToSqlParameters(originalTweets.Values.ToArray());
                ExecuteSqlNonQuery(generateInsertSQLQuery("pbist_twitter.tweets_normalized", originalTweets), parameters);
            }
            catch (Exception e)
            {
                //log.Info("******************Exception******************** Message:" + e);
            }
        }
    }

    //Generate SQL Statement
    private string generateInsertSQLQuery(string tableName, Dictionary<string, string> dictionary)
    {
        
        StringBuilder values = new StringBuilder();
        for (int i = 0; i < dictionary.Keys.Count; i++)
        {
            values.Append("@p"); values.Append(i+1);
            if (i < dictionary.Keys.Count - 2)
                values.Append(", ");
        }
        
        string sqlQueryGenerator = $"insert into " + tableName + "(" +
                                   String.Join(", ", dictionary.Select(x => SanitizeSimpleName(x.Key))) + ")" + " VALUES " + "(" + values.ToString() + ")";
        log.Info("******************Query gen******************** :" + sqlQueryGenerator);
        return sqlQueryGenerator;
    }


    //Split out terms by delimiter
    private String[] SplitHandles(string handle, char delimiter)
    {
        String[] handleId = new string[] { };
        handleId = handle.Split(delimiter);
        return handleId;
    }

    //Figure out if hashtag is in tweet
    private void HashtagDirectionCheck(KeyValuePair<string, string> currentEntry, string retweet = "")
    {
        string hashtag = "#" + currentEntry.Key.ToLower();
        string tweetText = tweet.TweetText.ToString();
        if (tweetText.ToLower().Contains(hashtag))
        {
            processedTweets["direction"] = "Hashtag" + retweet;
            originalTweets["accounttag"] = currentEntry.Key;
        }
    }

    //Figure out direction of tweet
    private void MessageDirectionCheck(KeyValuePair<string, string> currentEntry, JToken userMentions, string retweet = "")
    {
        if (currentEntry.Value.Contains(tweet.UserDetails.Id.ToString()))
        {
            processedTweets["direction"] = "Outbound" + retweet;
            originalTweets["accounttag"] = currentEntry.Key;
            if (tweet.UserDetails.TweetInReplyToUserId != null)
            {
                processedTweets["direction"] = "Outbound Reply" + retweet;
                originalTweets["accounttag"] = currentEntry.Key;
            }
        }
        else if (tweet.UserDetails.TweetInReplyToUserId != null)
        {
            if (currentEntry.Value.Contains(tweet.UserDetails.TweetInReplyToUserId.ToString()))
            {
                processedTweets["direction"] = "Inbound Reply" + retweet;
                originalTweets["accounttag"] = currentEntry.Key;
            }
        }
        else if (retweet != "")
        {
            if (currentEntry.Value.Contains(tweet.OriginalTweet.UserDetails.Id.ToString()))
            {
                processedTweets["direction"] = "Retweet of Outbound";
                originalTweets["accounttag"] = currentEntry.Key;
            }
        }
        if (userMentions != null && userMentions.HasValues)
        {
            foreach (var usermentionItem in userMentions)
            {
                string uid = usermentionItem.SelectToken("Id").ToString();
                if (currentEntry.Value.Contains(uid.ToString()))
                {
                    processedTweets["direction"] = "Inbound" + retweet;
                    originalTweets["accounttag"] = currentEntry.Key;
                }
            }
        }
    }

    private void hashtagmentions(string text, char delimiter, string field, string sqlTable, Dictionary<string, string> dictionary)
    {
        var regex = new Regex(@"(?<=" + delimiter + @")\w+");
        var matches = regex.Matches(text);
        foreach (Match match in matches)
        {
            dictionary[field] = match.ToString();
            SqlParameter[] parameters = MapValuesToSqlParameters(dictionary.Values.ToArray());
            ExecuteSqlNonQuery(generateInsertSQLQuery(sqlTable, dictionary), parameters);
        }
    }

    static async Task<string> MakeSentimentRequest(dynamic tweet)
    {
        var client = new HttpClient();
        Document response = new Document()
        {
            Text = tweet.TweetText.ToString(),
            Language = tweet.TweetLanguageCode.ToString(),
            Id = tweet.TweetId.ToString()
        };

        //Request headers
        string subscriptionKey = System.Configuration.ConfigurationManager.ConnectionStrings["apiKey"].ConnectionString;
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        var uri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";
        //Request body
        Sentiment sentiment = new Sentiment();
        sentiment.documents = new List<Document>();
        sentiment.documents.Add(response);
        string serializedSentiment = JsonConvert.SerializeObject(sentiment);
        StringContent content = new StringContent(serializedSentiment, Encoding.UTF8, "application/json");

        //new request


        var sentimentResponse = await client.PostAsync(uri, content);
        if (!sentimentResponse.IsSuccessStatusCode)
        {
            throw new Exception();
        }

        var sentimentResponseBody = await sentimentResponse.Content.ReadAsStringAsync();
        dynamic sentimentDeserialized = JsonConvert.DeserializeObject(sentimentResponseBody);
		
	    string sentimentScore = null;
        if (sentimentDeserialized.documents.ToString() != "[]")
        {
            sentimentScore = sentimentDeserialized.documents[0].score.ToString();
        }
		
        return sentimentScore;


    }
}

public class Document
{
    public string Language { get; set; }
    public string Id { get; set; }
    public string Text { get; set; }
}

public class Sentiment
{
    [JsonProperty(PropertyName = "documents")]
    public List<Document> documents { get; set; }
}