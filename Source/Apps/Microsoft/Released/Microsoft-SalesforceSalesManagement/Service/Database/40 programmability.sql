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
	-- 1 -> Data is present but not complete
	-- 2 -> Data pull is complete
	-- 3 -> No data is present

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
	UPDATE smgt.[configuration] 
	SET [configuration].[value] = 1 --Data pull is partially complete
	WHERE [configuration].configuration_group = 'SolutionTemplate' AND [configuration].configuration_subgroup = 'Notifier' AND [configuration].[name] = 'DataPullStatus'
	

	IF NOT EXISTS(SELECT p.[Percentage], p.[EntityName], i.lasttimestamp,  DATEDIFF(MINUTE, i.lasttimestamp, CURRENT_TIMESTAMP) AS [TimeDifference] FROM #percentages p
			  INNER JOIN dbo.entityinitialcount i ON i.entityName = p.EntityName
			  WHERE (p.[Percentage] <= 80 OR p.[Percentage] IS NULL) AND DATEDIFF(MINUTE, i.lasttimestamp, CURRENT_TIMESTAMP) > 5
			  OR (p.[Percentage] <= 80  OR p.[Percentage] IS NULL))
	UPDATE smgt.[configuration] 
	SET [configuration].[value] = 2 --Data pull is complete
	WHERE [configuration].configuration_group = 'SolutionTemplate' AND [configuration].configuration_subgroup = 'Notifier' AND [configuration].[name] = 'DataPullStatus'

	DECLARE @EntitiesWithNoData int;
	SET @EntitiesWithNoData = (SELECT COUNT(*) FROM #counts WHERE [Count] = 0)	

	IF (@EntitiesWithNoData = (SELECT COUNT(*) FROM #counts) AND DATEDIFF(HOUR, @DeploymentTimestamp, CURRENT_TIMESTAMP) > 24)
	UPDATE smgt.[configuration] 
	SET [configuration].[value] = 3 --No data is present
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