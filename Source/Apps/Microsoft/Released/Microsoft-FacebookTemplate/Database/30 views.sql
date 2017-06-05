SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

CREATE VIEW [fb].[CommentsPostsView] AS
(

SELECT 
      [Original Id]
      ,[Created Date]
	  ,CONVERT(date, [Created Date]) AS [Created Date Only]
      ,[Message]
      ,[From Id]
      ,[From Name]
	  ,[Page]
	  ,[media]
      ,[Original Id] AS [Original Post Id]
	  ,'Post' AS [Type]
  FROM [fb].[Posts]

  UNION ALL 

  SELECT 
      [Original Id]
      ,[Created Date]
	  ,CONVERT(date, [Created Date]) AS [Created Date Only]
      ,[Message]
      ,[From Id]
      ,[From Name]
	  ,[Page]
	  ,null AS [media]
      ,[Original Post Id]
	  ,'Comment' AS [Type]
  FROM [fb].[Comments]
  )

GO

CREATE VIEW [fb].[HashTagView] AS (

SELECT
	[Original Id]
    ,[HashTags]
FROM [fb].[HashTags]
)
GO


CREATE VIEW [fb].[KeyPhraseView] AS (
SELECT 
       [Original Id]
      ,[KeyPhrase]
FROM [fb].[KeyPhrase]
)
GO


CREATE VIEW [fb].[PostsView] AS (
SELECT 
      [Original Id]
      ,[Total Likes]
      ,[Total Shares]
      ,[Total Reactions]
      ,[Total Comments]
  FROM [fb].[Posts]
  )


GO

CREATE VIEW [fb].[ReactionsView] AS (
SELECT [Original Id]
      ,[Reaction Type]
      ,[From Id]
      ,[From Name]
FROM [fb].[Reactions]
)
GO 

CREATE VIEW [fb].[SentimentView] AS (
SELECT [Original Id]
      ,[Sentiment]
FROM [fb].[Sentiment]
)
GO
