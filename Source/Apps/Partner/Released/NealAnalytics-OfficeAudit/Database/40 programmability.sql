SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go


CREATE PROCEDURE [adtlog].[uspMoveStagingToAuditLog]
	
AS
	
	SET NOCOUNT ON;

	DECLARE @TransName varchar(30), @iSaveError Integer
	Select @TransName = 'uspMoveStagingToAuditLog'  
	
	Begin Transaction @TransName 	 

	IF OBJECT_ID('tempdb..#temp_audit') IS NOT NULL DROP TABLE #temp_audit
	Select * , ROW_NUMBER() OVER(Partition by ID ORDER BY ID ) AS Row#
	into #temp_audit
	from [adtlog].[staging_audit_data] 

	Delete from #temp_audit where Row# > 1

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
	FROM #temp_audit WHERE [Id] NOT IN (SELECT ID FROM [adtlog].[audit_data])

	Select @iSaveError = @@Error
	IF @iSaveError <> 0 Goto SaveError 

	TRUNCATE TABLE [adtlog].[staging_audit_data]

	Select @iSaveError = @@Error
	IF @iSaveError <> 0 Goto SaveError

	INSERT INTO [adtlog].[datasets]
           ([DatasetId]
           ,[DatasetName])
	SELECT DISTINCT
		    [DatasetId]
           ,[DatasetName]
	FROM [adtlog].[staging_datasets] WHERE [DatasetId] NOT IN (SELECT [DatasetId] FROM [adtlog].[datasets])


	Select @iSaveError = @@Error
	IF @iSaveError <> 0 Goto SaveError 
	
	TRUNCATE TABLE [adtlog].[staging_datasets]
	
	Select @iSaveError = @@Error
	IF @iSaveError <> 0 Goto SaveError

	Goto SaveSuccess                                
             
SaveError:                     
	Rollback Tran @TransName
	Return @iSaveError                                
       
SaveSuccess:                                
	Commit Tran @TransName                                
	Return 0   
