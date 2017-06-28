DROP TABLE eventhubsql;

CREATE TABLE dbo.eventHubSQL (
	time VARCHAR(50),
	resourceId VARCHAR(250),
	operationName VARCHAR(250),
	category VARCHAR(50),
	resultType VARCHAR(50),
	resultSignature VARCHAR(50),
	durationMs VARCHAR(50),
	callerIpAddress VARCHAR(50),
	correlationId VARCHAR(50),
	authorizationScope VARCHAR(50),
	eventInitiatedBy VARCHAR(50)
	);
GO
