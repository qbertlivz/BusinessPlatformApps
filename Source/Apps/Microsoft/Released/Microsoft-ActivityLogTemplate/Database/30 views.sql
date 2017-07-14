SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

CREATE VIEW bpst_aal.VerboseView
AS
	SELECT *
	FROM bpst_aal.ActivityLogData;
GO

CREATE VIEW bpst_aal.DateView
AS
	SELECT * 
	FROM bpst_aal.[date];
GO
