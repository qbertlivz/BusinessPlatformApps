SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

/*
CREATE VIEW [dbo].[RequestResponse] AS
SELECT a.RequestId,
		a.CreatedDate,
		DATEDIFF(ms, a.CreatedDate, b.CreatedDate) AS ExecutionTime,
		a.Operation,
		a.OperationId,
		a.Api,
		a.ApiId,
		a.Product,
		a.ProductId,
		a.SubscriptionId,
		a.SubscriptionName,
		a.IPAddress,
		a.Latitude,
		a.Longitude,
		a.City,
		b.StatusCode,
		b.StatusReason,
		a.Length AS RequestLength,
		b.Length AS ResponseLength

FROM dbo.Request a 
LEFT OUTER JOIN dbo.Response b 
ON A.RequestId = B.RequestId
go

CREATE VIEW dbo.APIMErrorDetail AS
SELECT a.[RequestId],
       a.[CreatedDate],
       a.[ServiceName],
       a.[Source],
       a.[Reason],
       a.[Message],
	   b.Operation,
	   b.OperationId,
	   b.Api,
	   b.ApiId,
	   b.Product,
       b.ProductId,
	   b.SubscriptionId,
	   b.SubscriptionName,
	   b.IPAddress
  FROM [dbo].[Error] a 
  LEFT OUTER JOIN dbo.Request b 
  ON A.RequestId = B.RequestId
go

CREATE VIEW dbo.RequestSummary AS
SELECT SubscriptionId,
	   COUNT(a.RequestId) AS RequestNumber,
		SUM( CAST( b.Length AS BIGINT ))  AS TransferredBytes,
	   SUM(DATEDIFF(ms, a.CreatedDate, b.CreatedDate)) AS TotalExecutionTime
  FROM [dbo].[Request] a
  INNER JOIN dbo.Response b 
  ON a.RequestId = b.RequestId
  GROUP BY SubscriptionId
go

CREATE VIEW [dbo].[vw_date]
AS

    SELECT date_key,
           full_date        AS [date],
           day_of_week      AS [day of the week],
           day_num_in_month AS [day number of the month],
           day_name         AS [day name],
           day_abbrev       AS [day abbreviated],
           weekday_flag     AS [weekday flag],
           [month],
           month_name       AS [month name],
           month_abbrev,
           [quarter],
           [year],
           same_day_year_ago_date,
           week_begin_date  AS [week begin date]
    FROM   dbo.date;
go

CREATE VIEW [dbo].ApiSummary
AS
SELECT DISTINCT ApiID, LAST_VALUE(Api) OVER (PARTITION BY OperationID ORDER BY ApiID ASC ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING) AS Api
FROM dbo.Request
go

CREATE VIEW [dbo].OperationSummary
AS
SELECT DISTINCT OperationID, LAST_VALUE(Operation) OVER (PARTITION BY OperationID ORDER BY OperationID ASC ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING) AS Operation
FROM dbo.Request
go

CREATE VIEW [dbo].ProductSummary
AS
SELECT DISTINCT OperationID, LAST_VALUE(Operation) OVER (PARTITION BY OperationID ORDER BY OperationID ASC ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING) AS Operation
FROM dbo.Request
go

CREATE VIEW [dbo].SubscriptionSummary
AS
SELECT DISTINCT SubscriptionID, LAST_VALUE(SubscriptionName) OVER (PARTITION BY SubscriptionID ORDER BY ProductID ASC ROWS BETWEEN CURRENT ROW AND UNBOUNDED FOLLOWING) AS Subscription
FROM dbo.Request
go

CREATE VIEW [dbo].[AllRequestData] AS
SELECT a.RequestId,
		a.CreatedDate,
		DATEDIFF(ms, a.CreatedDate, b.CreatedDate) AS ExecutionTime,
		a.Operation,
		a.OperationId,
		a.Api,
		a.ApiId,
		a.Product,
		a.ProductId,
		a.SubscriptionId,
		a.SubscriptionName,
		a.IPAddress,
		a.Latitude,
		a.Longitude,
		a.City,
		b.StatusCode,
		b.StatusReason,
		a.Length AS RequestLength,
		b.Length AS ResponseLength,
		IsError = 
		CASE   
			WHEN StatusCode >= 200 AND StatusCode < 300 THEN 0
			ELSE 1
		END   

FROM dbo.Request a
INNER JOIN dbo.Response b 
ON a.RequestId = b.RequestId
WHERE a.CreatedDate > DATEADD(day, -90, SYSDATETIME())
GO

CREATE VIEW VisualOperationCallVolume AS
SELECT 
	DATEADD(hour, DATEDIFF(hour, 0, CreatedDate), 0) as datetime_hour
   ,ProductID
   ,ApiID
   ,OperationId
   ,count(1) as operation_counts
From Request
GROUP BY DATEADD(hour, DATEDIFF(hour, 0, CreatedDate), 0),ProductID,ApiID,OperationId;
go

CREATE VIEW VisualCallProbabilityEdgeList AS
	Select
			el.Product
		,	el.Api
		,	el.Operation
		,	el.RelatedProduct
		,	el.RelatedApi
		,	el.RelatedOperation
		,	COUNT(distinct el.IPAddress) as users 
		,   Cast(Cast(100 * SUM(CallRelationshipCount) as Float) / t2.SourceCount as INT) as CallProbability
	From 
		CallProbabilityEdgeList el
		JOIN (
			Select t1.Product, t1.Api, t1.Operation, SUM(t1.StartingCallTotalCount) as SourceCount
			FROM ( 
				Select DISTINCT Product,Api,Operation,IPAddress,StartingCallTotalCount
				From CallProbabilityEdgeList
			) as t1
			GROUP BY  t1.Product, t1.Api, t1.Operation
		) t2 on el.Product = t2.Product AND el.Api  = t2.Api AND el.Operation = t2.Operation
	GROUP BY el.Product,el.Api,el.Operation,el.RelatedProduct,el.RelatedApi,el.RelatedOperation,t2.SourceCount;
go

CREATE VIEW VisualIPEdgeCounts AS
	Select
			Product
		,	Api
		,	Operation
		,	RelatedProduct
		,	RelatedApi
		,	RelatedOperation
		,	DATEADD(hour, DATEDIFF(hour, 0, CreatedDate), 0) as datetime_hour_bin
		,	IPAddress
		, Count(1) as counts
	FROM CallExtendedEdgeList
	GROUP BY 
		Product,Api,Operation,RelatedProduct,RelatedApi,RelatedOperation,
		IPAddress, DATEADD(hour, DATEDIFF(hour, 0, CreatedDate), 0);
go

CREATE VIEW VisualFreqHour AS
	Select
		IPAddress
	,	Position as 'Hour'
	,	CallFreq as SignalStrength
	FROM FFT
	Where TimeUnit = 'h';
go

CREATE VIEW VisualFreqMinute AS
	Select
		IPAddress
	,	Position as 'Minute'
	,	CallFreq as SignalStrength
	FROM FFT
	Where TimeUnit = 'min';
go

CREATE VIEW VisualFreqSecond AS
	Select
		IPAddress
	,	Position as 'Second'
	,	CallFreq as SignalStrength
	FROM FFT
	Where TimeUnit = 's';
go

*/