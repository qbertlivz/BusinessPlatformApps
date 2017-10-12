CREATE VIEW reddit.SubredditDocumentCountView AS
	SELECT
		Subreddit,
		COUNT(Subreddit) AS DocumentCount
	FROM reddit.Documents
	GROUP BY Subreddit
GO

-- All SubReddit Post fields
CREATE VIEW reddit.AllPostsView AS
	SELECT 
		reddit.Documents.*, 
		reddit.Sentiment.sentiment, 
		reddit.Posts.title,
		reddit.Posts.mediaPreviewUrl,
		CASE WHEN t0.documentContainsUserDefinedEntities
			IS NOT NULL THEN t0.documentContainsUserDefinedEntities
			ELSE CAST(0 AS BIT)
		END AS documentContainsUserDefinedEntities
	FROM
		reddit.Documents
		INNER JOIN reddit.Posts 
			ON reddit.Documents.id = reddit.Posts.documentId
		LEFT JOIN reddit.Sentiment 
			ON reddit.Documents.id = reddit.Sentiment.documentId
		LEFT JOIN (SELECT DISTINCT documentId, CAST(1 AS BIT) AS documentContainsUserDefinedEntities FROM reddit.UserDefinedEntities) t0 
			ON t0.documentId = reddit.Documents.id
GO

-- All documents (comments and posts)
CREATE VIEW [reddit].[AllDocumentsView] AS
	SELECT 
		reddit.Documents.*, 
		reddit.Sentiment.sentiment,
		CASE WHEN t0.documentContainsUserDefinedEntities
			IS NOT NULL THEN t0.documentContainsUserDefinedEntities
			ELSE CAST(0 AS BIT)
		END AS documentContainsUserDefinedEntities
	FROM
		reddit.Documents
		LEFT JOIN reddit.Sentiment 
			ON reddit.Documents.id = reddit.Sentiment.documentId
		LEFT JOIN (SELECT DISTINCT documentId, CAST(1 AS BIT) AS documentContainsUserDefinedEntities FROM reddit.UserDefinedEntities) t0 
			ON t0.documentId = reddit.Documents.id
GO

-- All Entities
CREATE VIEW reddit.AllEntitiesView AS
	SELECT 
		reddit.Entities.documentId,
	    entity,
		entityType,
	    entityOffset,
		entityLength,
		'Automatic' AS EntityOrigin,
		CASE WHEN t0.documentContainsUserDefinedEntities
			IS NOT NULL THEN t0.documentContainsUserDefinedEntities
			ELSE CAST(0 AS BIT)
		END AS documentContainsUserDefinedEntities
	FROM
		reddit.Entities
		LEFT JOIN (SELECT DISTINCT documentId, CAST(1 AS BIT) AS documentContainsUserDefinedEntities FROM reddit.UserDefinedEntities) t0 
			ON t0.documentId = reddit.Entities.documentId
UNION ALL
	SELECT 
		documentId,
	    entity,
		entityType,
	    entityOffset,
		entityLength,
		'UserDefined' AS EntityOrigin,
		CAST(1 AS BIT) AS documentContainsUserDefinedEntities
	FROM
		reddit.UserDefinedEntities
GO

-- All Comment fields
CREATE VIEW reddit.AllCommentsView AS
	SELECT 
		reddit.Documents.*, 
		reddit.Sentiment.sentiment, 
		reddit.Comments.parentId,
		reddit.Comments.postId,
		reddit.Comments.parentUrl,
		reddit.Comments.postUrl,
		CASE WHEN t0.documentContainsUserDefinedEntities
			IS NOT NULL THEN t0.documentContainsUserDefinedEntities
			ELSE CAST(0 AS BIT)
		END AS documentContainsUserDefinedEntities
	FROM
		reddit.Documents
		INNER JOIN reddit.Comments 
			ON reddit.Documents.id = reddit.Comments.documentId
		LEFT JOIN reddit.Sentiment 
			ON reddit.Documents.id = reddit.Sentiment.documentId
		LEFT JOIN (SELECT DISTINCT documentId, CAST(1 AS BIT) AS documentContainsUserDefinedEntities FROM reddit.UserDefinedEntities) t0 
			ON t0.documentId = reddit.Documents.id
GO

-- Comment Authors
CREATE VIEW reddit.CommentAuthorCountsView AS
	SELECT 
		author,
		COUNT(id) AS [count]
	FROM reddit.AllCommentsView
	GROUP BY author
GO

-- Post Authors
CREATE VIEW reddit.PostAuthorCountsView AS
	SELECT 
		author, 
		COUNT(id) AS [count]
	FROM reddit.AllPostsView
	GROUP BY author
GO

-- Subreddit co-authors
CREATE VIEW reddit.PostCoauthorsView AS
	WITH AuthorSubredditCounts AS 
	(
		SELECT subreddit, author, count(*) AS CNT 
		FROM reddit.Documents 
		WHERE isComment = 0
		GROUP by subreddit, author
	)
	SELECT A.subreddit AS subreddit1, B.subreddit AS subreddit2,
		SUM(A.CNT + B.CNT) AS CoAuthorScore
	FROM AuthorSubredditCounts A
	INNER JOIN AuthorSubredditCounts B
	ON A.author = B.author AND A.subreddit <> B.subreddit
	GROUP BY A.subreddit, B.subreddit
GO

-- Comment co-authors
CREATE VIEW reddit.CommentCoauthorsView AS
	WITH AuthorSubredditCounts AS 
	(
		SELECT subreddit, author, count(*) AS CNT 
		FROM reddit.Documents 
		WHERE isComment = 1
		GROUP by subreddit, author
	)
	SELECT A.subreddit AS subreddit1, B.subreddit AS subreddit2,
		SUM(A.CNT + B.CNT) AS CoAuthorScore
	FROM AuthorSubredditCounts A
	INNER JOIN AuthorSubredditCounts B
	ON A.author = B.author AND A.subreddit <> B.subreddit
	GROUP BY A.subreddit, B.subreddit
GO

-- Post and comment subreddit co-authors
CREATE VIEW reddit.PostAndCommentCoauthorsView AS
	WITH AuthorSubredditCounts AS 
	(
		SELECT subreddit, author, count(*) AS CNT 
		FROM reddit.Documents group by subreddit, author
	)
	SELECT A.subreddit AS subreddit1, B.subreddit AS subreddit2,
		SUM(A.CNT + B.CNT) AS CoAuthorScore
	FROM AuthorSubredditCounts A
	INNER JOIN AuthorSubredditCounts B
	ON A.author = B.author AND A.subreddit <> B.subreddit
	GROUP BY A.subreddit, B.subreddit
GO

-- Subreddit count
CREATE VIEW reddit.SubRedditPostCountView AS
	SELECT 
		subreddit, 
		COUNT(id) [PostCount] 
	FROM reddit.AllPostsView
	GROUP BY subreddit
GO

-- comment count by subreddit
CREATE VIEW reddit.CommentCountView AS
	SELECT 
		subreddit, 
		count(id) [count]
	FROM reddit.AllCommentsView
	GROUP BY subreddit
GO

-- Comment Co-Authors where authors commented on the same parent post
CREATE VIEW reddit.CoAuthorView AS
	SELECT tbl1.author AS author1, tbl2.author AS author2, COUNT(1) AS CNT 
	FROM reddit.AllCommentsView tbl1 
	INNER JOIN reddit.AllCommentsView tbl2 ON tbl1.parentid = tbl2.parentid AND tbl1.author != tbl2.author 
	GROUP by tbl1.author, tbl2.author HAVING count(1) > 5;
GO

-- Count of first-level comments on each document (post or comment) that has at least one comment.  This
-- does not count every comment in the comment tree, just the top-level comments based on parentId
CREATE VIEW reddit.CommentCountPerDocumentView AS
	WITH CommentCount AS
	(
		SELECT parentId AS documentId, COUNT(parentId) AS FirstLevelCommentCount
		FROM reddit.Comments
		GROUP BY parentId
	) 
	SELECT CommentCount.documentId, CommentCount.FirstLevelCommentCount, reddit.Documents.isComment
	FROM CommentCount
	INNER JOIN reddit.Documents ON CommentCount.documentId = reddit.Documents.id
GO

-- KeyPhrase Counts of total use of keyPhrase
CREATE VIEW reddit.KeyPhraseCountView AS
	SELECT LOWER(keyPhrase) AS keyPhraseLower, COUNT(1) AS DocumentFrequency 
	FROM reddit.KeyPhrases 
	GROUP BY LOWER(keyPhrase);
GO
