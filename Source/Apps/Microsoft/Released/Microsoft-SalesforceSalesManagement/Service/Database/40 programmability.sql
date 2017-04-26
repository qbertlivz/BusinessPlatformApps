SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

CREATE PROCEDURE dbo.sp_get_replication_counts
AS
BEGIN
	SET NOCOUNT ON;

    SELECT UPPER(LEFT(ta.name, 1)) + LOWER(SUBSTRING(ta.name, 2, 100)) AS EntityName, SUM(pa.rows) AS [Count], '' As [Status]
    FROM sys.tables ta INNER JOIN sys.partitions pa ON pa.OBJECT_ID = ta.OBJECT_ID
				       INNER JOIN sys.schemas sc ON ta.schema_id = sc.schema_id
    WHERE
	    sc.name='dbo' AND ta.is_ms_shipped = 0 AND pa.index_id IN (0,1) AND
	    ta.name IN ('Account', 'Lead', 'Opportunity', 'OpportunityLineItem', 'OpportunityStage', 'UserRole', 'User', 'Product2')
    GROUP BY ta.name
    ORDER BY ta.name
END;
GO

CREATE PROCEDURE [dbo].[sp_get_pull_status]
AS
BEGIN
		
	--InitialPullComplete statuses
	-- -1 -> Initial State
	-- 1 -> Data is present but not complete
	-- 2 -> Data pull is complete
	-- 3 -> No data is present

	DECLARE @StatusCode int;
	SET @StatusCode = -1;

	SET NOCOUNT ON;
    SELECT UPPER(LEFT(ta.name, 1)) + LOWER(SUBSTRING(ta.name, 2, 100)) AS EntityName, SUM(pa.rows) AS [Count], '' As [Status] into #counts
    FROM sys.tables ta INNER JOIN sys.partitions pa ON pa.OBJECT_ID = ta.OBJECT_ID
				       INNER JOIN sys.schemas sc ON ta.schema_id = sc.schema_id
    WHERE
	    sc.name='dbo' AND ta.is_ms_shipped = 0 AND pa.index_id IN (0,1) AND
	    ta.name IN ('Account', 'Lead', 'Opportunity', 'OpportunityLineItem', 'OpportunityStage', 'UserRole', 'User', 'Product2')
    GROUP BY ta.name

	SELECT (CASE WHEN c.Count = 0 AND i.initialcount = 0 
			THEN 100 
			ELSE (
				  CASE WHEN (100.0 * c.Count / nullif(i.initialCount,0)) IS NULL 
				  THEN 0 
				  ELSE 100.0 * c.Count/i.initialCount 
				  END) 
			END) AS [Percentage], 
		    c.EntityName as EntityName INTO #percentages
	FROM #counts c 
	INNER JOIN dbo.entityinitialcount i ON i.entityname = c.entityname


	DECLARE @DeploymentTimestamp datetime2;
	SET @DeploymentTimestamp = CAST((SELECT [value] from smgt.[configuration] config
								WHERE config.configuration_group = 'SolutionTemplate' AND config.configuration_subgroup = 'Notifier' AND config.[name] = 'DeploymentTimestamp') AS datetime2)

	IF EXISTS (SELECT *
			   FROM #counts
			   WHERE [Count] > 0 AND DATEDIFF(HOUR, @DeploymentTimestamp, CURRENT_TIMESTAMP) > 24)
	SET @StatusCode = 1 --Data pull is partially complete
		
	
	DECLARE @CompletePercentage decimal;
	SET @CompletePercentage = CAST((SELECT [value] from smgt.[configuration] config
								WHERE config.configuration_group = 'SolutionTemplate' AND config.configuration_subgroup = 'Notifier' AND config.[name] = 'DataPullCompleteThreshold') AS decimal)

	IF NOT EXISTS(SELECT p.[Percentage], p.[EntityName], i.lasttimestamp,  DATEDIFF(MINUTE, i.lasttimestamp, CURRENT_TIMESTAMP) AS [TimeDifference] FROM #percentages p
			  INNER JOIN dbo.entityinitialcount i ON i.entityName = p.EntityName
			  WHERE (p.[Percentage] <= @CompletePercentage OR p.[Percentage] IS NULL) AND DATEDIFF(MINUTE, i.lasttimestamp, CURRENT_TIMESTAMP) > 5
			  OR (p.[Percentage] <= @CompletePercentage  OR p.[Percentage] IS NULL))
	SET @StatusCode = 2 --Data pull complete

	DECLARE @EntitiesWithNoData int;
	SET @EntitiesWithNoData = (SELECT COUNT(*) FROM #counts WHERE [Count] = 0)	

	IF (@EntitiesWithNoData = (SELECT COUNT(*) FROM #counts) AND DATEDIFF(HOUR, @DeploymentTimestamp, CURRENT_TIMESTAMP) > 24)
	SET @StatusCode = 3 --No data is present
	
	DECLARE @ASDeployment bit;	 
	SET @ASDeployment = 0;

	IF EXISTS (SELECT * FROM smgt.[configuration] 
			   WHERE [configuration].configuration_group = 'SolutionTemplate' AND 
					 [configuration].configuration_subgroup = 'Notifier' AND 
					 [configuration].[name] = 'ASDeployment' AND
					 [configuration].[value] ='true')
	SET @ASDeployment = 1;

	IF NOT EXISTS (SELECT * FROM smgt.ssas_jobs WHERE [statusMessage] = 'Success') AND @ASDeployment = 1 AND DATEDIFF(HOUR, @DeploymentTimestamp, CURRENT_TIMESTAMP) < 24
	SET @StatusCode = -1;

	UPDATE smgt.[configuration] 
	SET [configuration].[value] = @StatusCode
	WHERE [configuration].configuration_group = 'SolutionTemplate' AND [configuration].configuration_subgroup = 'Notifier' AND [configuration].[name] = 'DataPullStatus'

	MERGE entityinitialcount AS TARGET
	USING #counts AS SOURCE
	ON (TARGET.entityname = SOURCE.entityname)
	WHEN MATCHED AND SOURCE.[Count] > TARGET.lastcount
	THEN UPDATE SET 
	TARGET.lastcount = SOURCE.[Count],
	TARGET.lasttimestamp = CURRENT_TIMESTAMP;
END;
GO


CREATE PROCEDURE dbo.sp_get_prior_content
AS
BEGIN
	SET NOCOUNT ON;

    SELECT Count(*) AS ExistingObjectCount
    FROM   information_schema.tables
    WHERE  ( table_schema = 'dbo' AND
             table_name IN ('account', 'lead', 'opportunity', 'opportunitylineitem', 'opportunitystage', 'product2', 'user', 'userrole')
           ) OR
           ( table_schema = 'smgt' AND
             table_name IN ('accountview', 'actualsales', 'actualsalesview', 'businessunitview', 'configuration', 'configurationview', 'date', 'dateview',
                            'leadview', 'measuresview', 'opportunityproductview', 'opportunityview', 'productview', 'quotas', 'quotaview', 'targets',
                            'targetview', 'tempuserview', 'territoryview', 'userascendantsview', 'usermapping', 'userview'
                           )
           );
END;