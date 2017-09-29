SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go


CREATE PROCEDURE [adtlog].[uspMoveStagingToAuditLog]
	
AS
BEGIN

BEGIN TRAN
DELETE t 
FROM [adtlog].[audit_data] as t
INNER JOIN [adtlog].[staging_audit_data] s 
ON t.Id = s.Id;


INSERT INTO [adtlog].[audit_data]
    ([Id]
    ,[RecordType]
    ,[CreationTime]
    ,[UserType]
    ,[UserKey]
    ,[JSONPayload]
    ,[BatchId])
SELECT DISTINCT 
	[Id]
    ,[RecordType]
    ,[CreationTime]
    ,[UserType]
    ,[UserKey]
    ,[JSONPayload]
    ,[BatchId]
		   
FROM [adtlog].[staging_audit_data]
    
TRUNCATE TABLE [adtlog].[staging_datasets]
COMMIT

END
go