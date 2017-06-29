SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;

CREATE TABLE [napbi].[O365AuditLog] (
    [id] [int] IDENTITY(1,1) NOT NULL,
    [PSComputerName] [varchar](max) NULL,
    [RunspaceId] [nvarchar](max) NULL,
    [PSShowComputerName] [varchar](max) NULL,
    [RecordType] [varchar](max) NULL,
    [CreationDate] [nvarchar](max) NULL,
    [UserIds] [varchar](max) NULL,
    [Operations] [varchar](max) NULL,
    [AuditData] [varchar](max) NULL,
    [ResultIndex] [int] NULL,
    [ResultCount] [int] NULL,
    [Identity] [nvarchar](max) NULL,
    [IsValid] [varchar](max) NULL,
    [ObjectState] [varchar](max) NULL,
    CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED 
    (
        [id] ASC
    )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
