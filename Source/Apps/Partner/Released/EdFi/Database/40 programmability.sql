SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

CREATE PROCEDURE cc.sp_get_replication_counts
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ta.name AS EntityName, SUM(pa.rows) AS [Count]
    FROM sys.tables ta INNER JOIN sys.partitions pa ON pa.OBJECT_ID = ta.OBJECT_ID
                       INNER JOIN sys.schemas sc ON ta.schema_id = sc.schema_id
    WHERE
        sc.name='edfi' AND ta.is_ms_shipped = 0 AND pa.index_id IN (0,1) AND ta.name <>'configuration'
    GROUP BY ta.name
    ORDER BY ta.name;
END;
go
