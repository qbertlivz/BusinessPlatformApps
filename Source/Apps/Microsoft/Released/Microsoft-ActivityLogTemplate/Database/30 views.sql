SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

CREATE VIEW bpst_aal.VerboseView
AS
	SELECT 
		eventId				AS [Event ID],
		[caller]			AS [Caller],
		correlationId		AS [Correlation ID],
		[description]		AS [Description],
		eventCategory		AS [Event Category], 
		impact				AS [Impact],
		impactedRegions		AS [Impacted Regions],
		impactedServices	AS [Impacted Services],
		jobFailedMessage	AS [Job Failed Message],
		[level]				AS [Level],
		operationCategory	AS [Operation Category],
		operationId			AS [Operation ID],
		operationName		AS [Operation Name],
		resourceGroup		AS [Resource Group],
		resourceId			AS [Resource ID],
		[status]			AS [Status],
		statusCode			AS [Status Code],
		subscriptionId		AS [Subscription ID],
		[timestamp]			AS [Timestamp]
	FROM bpst_aal.ActivityLogData;
GO

CREATE VIEW bpst_aal.DateView
AS
	SELECT * 
	FROM bpst_aal.[date];
GO
