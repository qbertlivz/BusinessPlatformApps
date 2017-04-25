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

CREATE PROCEDURE dbo.sp_get_pull_status
AS
BEGIN
	
	--InitialPullComplete statuses
	-- 1 -> Data pull is complete
	-- 0 -> Data is present but not complete
	-- -1 -> No data is present

	SET NOCOUNT ON;
    SELECT UPPER(LEFT(ta.name, 1)) + LOWER(SUBSTRING(ta.name, 2, 100)) AS EntityName, SUM(pa.rows) AS [Count], '' As [Status] INTO #counts
    FROM sys.tables ta INNER JOIN sys.partitions pa ON pa.OBJECT_ID = ta.OBJECT_ID
				       INNER JOIN sys.schemas sc ON ta.schema_id = sc.schema_id
    WHERE
	    sc.name='dbo' AND ta.is_ms_shipped = 0 AND pa.index_id IN (0,1) AND
	    ta.name IN ('Account', 'Lead', 'Opportunity', 'OpportunityLineItem', 'OpportunityStage', 'UserRole', 'User', 'Product2')
    GROUP BY ta.name

	SELECT (100.0 * nullif(c.Count, 0) / i.initialCount) AS [Percentage], c.EntityName as EntityName INTO #percentages
	FROM #counts c
	INNER JOIN dbo.entityinitialcount i ON i.entityname = c.entityname

	DECLARE @RowsWithSomeData int;
	SET @RowsWithSomeData = (SELECT COUNT(*) FROM #counts
						            WHERE [Count] > 0 )

	IF EXISTS (SELECT * FROM notifier n
			   WHERE DATEDIFF(MINUTE, n.deploymenttimestamp, CURRENT_TIMESTAMP) > 24 AND @RowsWithSomeData > 0)
	UPDATE n
	SET n.initialpullcomplete = 0 --Data pull is complete
	FROM notifier n

	IF NOT EXISTS(SELECT p.[Percentage], p.[EntityName], i.lasttimestamp,  DATEDIFF(MINUTE, i.lasttimestamp, CURRENT_TIMESTAMP) AS [TimeDifference] FROM #percentages p
			  INNER JOIN dbo.entityinitialcount i ON i.entityName = p.EntityName
			  WHERE p.[Percentage] <= 80 AND DATEDIFF(MINUTE, i.lasttimestamp, CURRENT_TIMESTAMP) > 5
			  OR p.[Percentage] <= 80)
	UPDATE n
	SET n.initialpullcomplete = 1 --Data pull is complete
	FROM notifier n
	
	MERGE entityinitialcount AS TARGET
	USING #counts AS SOURCE
	ON (TARGET.entityname = SOURCE.entityname)
	WHEN MATCHED
	THEN UPDATE SET 
	TARGET.lastcount = SOURCE.[Count],
	TARGET.lasttimestamp = CURRENT_TIMESTAMP;
	
	DECLARE @RowsWithoutData int;
	SET @RowsWithoutData = (SELECT COUNT(*) FROM #counts
						            WHERE [Count] = 0 )

	IF EXISTS (SELECT * FROM notifier n
			   WHERE DATEDIFF(MINUTE, n.deploymenttimestamp, CURRENT_TIMESTAMP) > 24 AND @RowsWithoutData > 0)
	UPDATE n
	SET n.initialpullcomplete = -1 --Data pull is complete
	FROM notifier n			   


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