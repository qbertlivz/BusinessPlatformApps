SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;

CREATE TABLE [adtlog].[audit_data](
    [Id] [varchar](50) NOT NULL,
    [RecordType] [varchar](max) NULL,
    [CreationTime] [datetime] NOT NULL,
    [UserType] [varchar](max) NULL,
    [UserKey] [varchar](max) NULL,
    [JSONPayload] [varchar](max) NULL,
    [BatchId] [varchar](14) NULL,
    PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

CREATE TABLE [adtlog].[batch_log](
	[BatchID] [varchar](14) NOT NULL,
	[LogText] [nvarchar](max) NULL,
	[ExecutedBy] [nvarchar](50) NULL,
	[RecordCount] [int] NULL,
	[EventStart] [datetime] NULL,
	[EventEnd] [datetime] NULL,
	[Status] [varchar](50) NULL,
	[BatchResponse] [nvarchar](max) NULL,
	[IndividualResponse] [nvarchar](max) NULL,
	[APIContentUri] [nvarchar](max) NULL,
	[APIStartDate] [datetime] NULL,
	[APIEndDate] [datetime] NULL
)

CREATE TABLE [adtlog].[datasets](
    [DatasetId] [varchar](50) NOT NULL,
    [DatasetName] [varchar](50) NOT NULL,
    PRIMARY KEY CLUSTERED 
    (
        [DatasetId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

CREATE TABLE [adtlog].[staging_audit_data](
    [Id] [varchar](50) NOT NULL,
    [RecordType] [varchar](max) NULL,
    [CreationTime] [datetime] NOT NULL,
    [UserType] [varchar](max) NULL,
    [UserKey] [varchar](max) NULL,
    [JSONPayload] [varchar](max) NULL,
    [BatchId] [varchar](14) NULL
)

CREATE TABLE [adtlog].[staging_datasets](
    [DatasetId] [varchar](50) NOT NULL,
    [DatasetName] [varchar](50) NOT NULL
)

CREATE TABLE [adtlog].[user](
    [Email] [varchar](max) NULL,
    [Type] [varchar](10) NULL,
    [EntryTime] [datetime] NULL
)
GO

ALTER TABLE [adtlog].[user] ADD  DEFAULT (getdate()) FOR [EntryTime]
GO

CREATE NONCLUSTERED INDEX IDX_stagingAuditDataId
ON [adtlog].[staging_audit_data] ([Id])

CREATE NONCLUSTERED INDEX IDX_stagingDatasetsId
ON [adtlog].[staging_datasets] ([DatasetId])