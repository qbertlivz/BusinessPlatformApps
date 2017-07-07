SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;

CREATE TABLE aal.LiveData (
	authorizationAction VARCHAR(250),
	authorizationScope VARCHAR(250),
	[caller] VARCHAR(50),
	callerIpAddress VARCHAR(50),
	category VARCHAR(50),
	correlationId VARCHAR(50),
	eventCategory VARCHAR(25),
	eventPropertyStatusCode VARCHAR(25),
	jobFailedMessage VARCHAR(MAX),
	[level] VARCHAR(25),
	operationName VARCHAR(250),
	resourceId VARCHAR(MAX),
	[status] VARCHAR(50),
	statusCode VARCHAR(25),
	subStatusCode VARCHAR(25),
	[timestamp] VARCHAR(50));

CREATE TABLE aal.HistoricalData (
	authorizationAction VARCHAR(250),
	authorizationScope VARCHAR(1000),
	[caller] VARCHAR(50),
	correlationId VARCHAR(250),
	[description] VARCHAR(MAX),
	eventDataId VARCHAR(250), 
	httpRequestClientId	VARCHAR(150),
	httpRequestClientIpAddr VARCHAR(50),
	httpRequestMethod VARCHAR(20),
	[level] VARCHAR(100),
	resourceGroupName VARCHAR(100),
	resourceId VARCHAR(MAX),
	resourceProviderName VARCHAR(100),
	operationId VARCHAR(250),
	operationName VARCHAR(250),
	statusCode VARCHAR(50),
	[status] VARCHAR(50),
	subStatus VARCHAR(150),
	[timestamp] VARCHAR(50),
	subscriptionId VARCHAR(100));
