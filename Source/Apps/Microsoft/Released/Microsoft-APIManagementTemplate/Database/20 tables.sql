SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

CREATE TABLE [dbo].[Request] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate] DATETIME         NULL,
    [ServiceName] VARCHAR (200)    NULL,
    [RequestId]   VARCHAR (50)     NULL,
    [IPAddress]   VARCHAR (20)     NULL,
    [Operation]   VARCHAR (200)    NULL,
    [OperationID] VARCHAR (200)    NULL,
    [Api]         VARCHAR (200)    NULL,
    [ApiID]       VARCHAR (200)    NULL,
    [Product]     VARCHAR (200)    NULL,
    [ProductID]   VARCHAR (200)    NULL,
    [SubscriptionName]  VARCHAR (200)    NULL,
    [SubscriptionId]    VARCHAR (200)    NULL,
    [Length]      int               NULL,
    [Latitude] [decimal](9, 6) NULL,
    [Longitude] [decimal](9, 6) NULL,
    [City] [varchar](30) NULL,
 CONSTRAINT [PK_Request_1] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)
)

ALTER TABLE [dbo].[Request] ADD DEFAULT (newsequentialid()) FOR [Id]
CREATE NONCLUSTERED INDEX [IX_Request_RequestId_1] ON [dbo].[Request]
(
    [RequestId] ASC
)
CREATE NONCLUSTERED INDEX IX_Request_ApiID_1 ON dbo.Request
    (
    ApiID
    ) 
CREATE NONCLUSTERED INDEX IX_Request_OperationID_1 ON dbo.Request
    (
    OperationID
    ) 
CREATE NONCLUSTERED INDEX IX_Request_ProductID_1 ON dbo.Request
    (
    ProductID
    ) 
CREATE NONCLUSTERED INDEX IX_Request_SubscriptionId_1 ON dbo.Request
    (
    SubscriptionId
    ) 


CREATE NONCLUSTERED INDEX [IX_Request_IPAddress] ON [dbo].[Request]
(
    [IPAddress] ASC
)

CREATE NONCLUSTERED INDEX [IX_Request_Latitude] ON [dbo].[Request]
(
    [Latitude] ASC
)

CREATE NONCLUSTERED INDEX [IX_Request_Longitude] ON [dbo].[Request]
(
    [Longitude] ASC
)

CREATE NONCLUSTERED INDEX [nci_wi_Request_IPAddressApi] ON [dbo].[Request] ([IPAddress], [Api]) INCLUDE ([CreatedDate], [Latitude], [Longitude], [Operation], [Product], [RequestId]) WITH (ONLINE = ON)

CREATE TABLE [dbo].[Response] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate] DATETIME         NULL,
    [ServiceName] VARCHAR (200)    NULL,
    [RequestId]   VARCHAR (50)     NULL,
    [StatusCode]  int               NULL,
    [StatusReason] [varchar](200)  NULL,
    [Length]      int               NULL,    
 CONSTRAINT [PK_Response_1] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)
)

ALTER TABLE [dbo].[Response] ADD  DEFAULT (newsequentialid()) FOR [Id]

CREATE NONCLUSTERED INDEX [IX_Response_RequestId_1] ON [dbo].[Response]
(
    [RequestId] ASC
)

CREATE TABLE [dbo].[Error] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate] DATETIME         NULL,
    [ServiceName] VARCHAR (200)    NULL,
    [RequestId]   VARCHAR (50)     NULL,
    [Source]      VARCHAR (200)    NULL,
    [Reason]      VARCHAR (200)    NULL,
    [Message] [varchar](200) NULL,
 CONSTRAINT [PK_Error_1] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)
)

ALTER TABLE [dbo].[Error] ADD  DEFAULT (newsequentialid()) FOR [Id]

CREATE NONCLUSTERED INDEX [IX_Error_RequestId_1] ON [dbo].[Error]
(
    [RequestId] ASC
)

CREATE TABLE [dbo].[Date](
    [date_key] [int] NOT NULL,
    [full_date] [date] NOT NULL,
    [day_of_week] [tinyint] NOT NULL,
    [day_num_in_month] [tinyint] NOT NULL,
    [day_name] [nvarchar](50) NOT NULL,
    [day_abbrev] [nvarchar](10) NOT NULL,
    [weekday_flag] [char](1) NOT NULL,
    [week_num_in_year] [tinyint] NOT NULL,
    [week_begin_date] [date] NOT NULL,
    [month] [tinyint] NOT NULL,
    [month_name] [nvarchar](50) NOT NULL,
    [month_abbrev] [nvarchar](10) NOT NULL,
    [quarter] [tinyint] NOT NULL,
    [year] [smallint] NOT NULL,
    [yearmo] [int] NOT NULL,
    [same_day_year_ago_date] [date] NOT NULL,
 CONSTRAINT [pk_dim_date] PRIMARY KEY CLUSTERED 
(
    [date_key] ASC
)
)

CREATE TABLE [dbo].[GeoLite2-City-Blocks-IPv4](
    [network] [varchar](50) NULL,
    [geoname_id] [varchar](50) NULL,
    [registered_country_geoname_id] [varchar](50) NULL,
    [represented_country_geoname_id] [varchar](50) NULL,
    [is_anonymous_proxy] [varchar](50) NULL,
    [is_satellite_provider] [varchar](50) NULL,
    [postal_code] [varchar](50) NULL,
    [latitude] [varchar](50) NULL,
    [longitude] [varchar](50) NULL,
    [accuracy_radius] [varchar](50) NULL,
    [IPpart] [nvarchar](3) NULL
)

CREATE NONCLUSTERED INDEX [IX_GeoLite2-City-Blocks-IPv4] ON [dbo].[GeoLite2-City-Blocks-IPv4]
(
    [network] ASC
)

CREATE NONCLUSTERED INDEX [IX_GeoLite2-City-Blocks-IPv4_IPPart] ON [dbo].[GeoLite2-City-Blocks-IPv4]
(
    [IPPart] ASC
)

CREATE NONCLUSTERED INDEX [IX_GeoLite2-City-Blocks-IPv4_58445F005250DAF33A2632E6B88C5216] ON [dbo].[GeoLite2-City-Blocks-IPv4]
(
    [IPpart] ASC
)
INCLUDE (     [latitude],
    [longitude],
    [network])

CREATE TABLE [dbo].[CallExtendedEdgeList](
    [RequestId] [varchar](50) NOT NULL,
    [Product] [varchar](200) NOT NULL,
    [Api] [varchar](200) NOT NULL,
    [Operation] [varchar](200) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [RelatedRequestId] [varchar](50) NOT NULL,
    [RelatedProduct] [varchar](200) NOT NULL,
    [RelatedApi] [varchar](200) NOT NULL,
    [RelatedOperation] [varchar](200) NOT NULL,
    [RelatedCreatedDate] [datetime] NOT NULL,
    [IPAddress] [varchar](20) NOT NULL
)

CREATE TABLE [dbo].[CallExtendedEdgeList_STAGE](
    [RequestId] [varchar](50) NOT NULL,
    [Product] [varchar](200) NOT NULL,
    [Api] [varchar](200) NOT NULL,
    [Operation] [varchar](200) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [RelatedRequestId] [varchar](50) NOT NULL,
    [RelatedProduct] [varchar](200) NOT NULL,
    [RelatedApi] [varchar](200) NOT NULL,
    [RelatedOperation] [varchar](200) NOT NULL,
    [RelatedCreatedDate] [datetime] NOT NULL,
    [IPAddress] [varchar](20) NOT NULL
)

CREATE TABLE [dbo].[CallProbabilityEdgeList](
    [Product] [varchar](200) NOT NULL,
    [Api] [varchar](200) NOT NULL,
    [Operation] [varchar](200) NOT NULL,
    [RelatedProduct] [varchar](200) NOT NULL,
    [RelatedApi] [varchar](200) NOT NULL,
    [RelatedOperation] [varchar](200) NOT NULL,
    [IPAddress] [varchar](20) NOT NULL,
    [CallRelationshipCount] [int] NOT NULL,
    [StartingCallTotalCount] [int] NOT NULL
)

CREATE TABLE [dbo].[CallProbabilityEdgeList_STAGE](
    [Product] [varchar](200) NOT NULL,
    [Api] [varchar](200) NOT NULL,
    [Operation] [varchar](200) NOT NULL,
    [RelatedProduct] [varchar](200) NOT NULL,
    [RelatedApi] [varchar](200) NOT NULL,
    [RelatedOperation] [varchar](200) NOT NULL,
    [IPAddress] [varchar](20) NOT NULL,
    [CallRelationshipCount] [int] NOT NULL,
    [StartingCallTotalCount] [int] NOT NULL
)

CREATE TABLE [dbo].[FFT](
    [Id] [uniqueidentifier] NOT NULL,
    [IPAddress] [varchar](20) NOT NULL,
    [TimeUnit] [varchar](50) NOT NULL,
    [CallFreq] [decimal](18, 0) NOT NULL,
    [Position] [int] NOT NULL
)

ALTER TABLE [dbo].[FFT] ADD  DEFAULT (newsequentialid()) FOR [Id]

CREATE TABLE [dbo].[FFT_STAGE](
    [Id] [uniqueidentifier] NOT NULL,
    [IPAddress] [varchar](20) NOT NULL,
    [TimeUnit] [varchar](50) NOT NULL,
    [CallFreq] [decimal](18, 0) NOT NULL,
    [Position] [int] NOT NULL
)

ALTER TABLE [dbo].[FFT_STAGE] ADD  DEFAULT (newsequentialid()) FOR [Id]
