SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

--CREATE PROCEDURE pbst.sp_get_pull_status
--AS
--BEGIN
--    SET NOCOUNT ON;
    
--    --InitialPullComplete statuses
--    -- -1 -> Initial State
--    -- 1 -> Data is present but not complete
--    -- 2 -> Data pull is complete
--    -- 3 -> No data is present

--    DECLARE @StatusCode INT = -1;

--    SELECT ta.[name] AS EntityName, SUM(pa.[rows]) AS [Count] INTO #counts
--    FROM sys.tables ta INNER JOIN sys.partitions pa ON pa.OBJECT_ID = ta.OBJECT_ID
--                       INNER JOIN sys.schemas sc ON ta.schema_id = sc.schema_id
--    WHERE
--            sc.name='dbo' AND ta.is_ms_shipped = 0 AND pa.index_id IN (0,1) AND
--        ta.name LIKE '%CustCollectionsBIMeasurements%'
--    GROUP BY ta.[name];
	   
--    DECLARE @DeploymentTimestamp DATETIME2;
--    SELECT @DeploymentTimestamp = Convert(DATETIME2, [value], 126)
--    FROM pbst.[configuration] WHERE configuration_group = 'SolutionTemplate' AND configuration_subgroup = 'Notifier' AND [name] = 'DeploymentTimestamp';

--    IF EXISTS (SELECT *
--               FROM #counts
--               WHERE [Count] > 0 AND DATEDIFF(HOUR, @DeploymentTimestamp, SYSUTCDATETIME()) > 24)
--	       SET @StatusCode = 1 --Data pull is partially complete
    

--    UPDATE pbst.[configuration] 
--    SET [configuration].[value] = @StatusCode
--    WHERE [configuration].configuration_group = 'SolutionTemplate' AND [configuration].configuration_subgroup = 'Notifier' AND [configuration].[name] = 'DataPullStatus'
--END;
