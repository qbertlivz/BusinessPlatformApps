SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

-- Must be executed inside the target database
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name='edfi')
BEGIN
    EXEC ('CREATE SCHEMA edfi'); -- Avoid batch error
END;

