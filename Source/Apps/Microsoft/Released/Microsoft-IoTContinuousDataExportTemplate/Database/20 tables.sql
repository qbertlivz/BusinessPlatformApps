SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;

CREATE TABLE [analytics].[Devices](
	[deviceId] [nvarchar](200) NOT NULL,
	[model] [nvarchar](101) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
	[description] [text] NULL,
	[simulated] [bit] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[deviceId] ASC
	)
);

CREATE TABLE [analytics].[MeasurementDefinitions](
	[id] [nvarchar](357) NOT NULL,
	[model] [nvarchar](101) NOT NULL,
	[field] [nvarchar](255) NOT NULL,
	[kind] [nvarchar](50) NOT NULL,
	[dataType] [nvarchar](100) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
);

CREATE TABLE [analytics].[Measurements](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[deviceId] [nvarchar](200) NOT NULL,
	[model] [nvarchar](101) NOT NULL,
	[definition] [nvarchar](357) NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[numericValue] [decimal](30, 10) NULL,
	[stringValue] [nvarchar](max) NULL,
	[booleanValue] [bit] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
);

CREATE TABLE [analytics].[Models](
	[id] [nvarchar](101) NOT NULL,
	[modelId] [nvarchar](50) NOT NULL,
	[modelVersion] [nvarchar](50) NOT NULL,
	[name] [nvarchar](1000) NOT NULL,
	[description] [text] NULL,
	[thumbnail] [nvarchar](1000) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
);

CREATE TABLE [analytics].[Properties](
	[id] [nvarchar](507) NOT NULL,
	[deviceId] [nvarchar](200) NOT NULL,
	[model] [nvarchar](101) NOT NULL,
	[definition] [nvarchar](408) NOT NULL,
	[lastUpdated] [datetime] NOT NULL,
	[numericValue] [decimal](30, 10) NULL,
	[stringValue] [nvarchar](max) NULL,
	[booleanValue] [bit] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
);

CREATE TABLE [analytics].[PropertyDefinitions](
	[id] [nvarchar](408) NOT NULL,
	[model] [nvarchar](101) NOT NULL,
	[field] [nvarchar](255) NOT NULL,
	[kind] [nvarchar](50) NOT NULL,
	[dataType] [nvarchar](100) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
	[optional] [bit] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
);

CREATE TABLE [dbo].[ChangeTracking](
	[SYS_CHANGE_VERSION] [bigint] NULL
);

CREATE TABLE [stage].[Devices](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[deviceId] [nvarchar](200) NOT NULL,
	[modelId] [nvarchar](50) NOT NULL,
	[modelVersion] [nvarchar](50) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
	[description] [text] NULL,
	[simulated] [bit] NOT NULL,
	CONSTRAINT [Pk_Dim_Devices] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	),
	UNIQUE NONCLUSTERED 
	(
		[deviceId] ASC
	)
);

ALTER TABLE [stage].[Devices]
ENABLE CHANGE_TRACKING  
WITH (TRACK_COLUMNS_UPDATED = ON);

CREATE TABLE [stage].[MeasurementDefinitions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[modelId] [nvarchar](50) NOT NULL,
	[modelVersion] [nvarchar](50) NOT NULL,
	[field] [nvarchar](255) NOT NULL,
	[kind] [nvarchar](50) NOT NULL,
	[dataType] [nvarchar](100) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
	CONSTRAINT [Pk_Dim_MeasurementDefinitions] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
);

ALTER TABLE [stage].[MeasurementDefinitions]
ENABLE CHANGE_TRACKING  
WITH (TRACK_COLUMNS_UPDATED = ON);

CREATE TABLE [stage].[Measurements](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[deviceId] [nvarchar](200) NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[field] [nvarchar](255) NOT NULL,
	[numericValue] [decimal](30, 10) NULL,
	[stringValue] [nvarchar](max) NULL,
	[booleanValue] [bit] NULL,
	CONSTRAINT [Pk_Dim_Measurements] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
);

ALTER TABLE [stage].[Measurements]
ENABLE CHANGE_TRACKING  
WITH (TRACK_COLUMNS_UPDATED = ON);

CREATE TABLE [stage].[Models](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[modelId] [nvarchar](50) NOT NULL,
	[modelVersion] [nvarchar](50) NOT NULL,
	[name] [nvarchar](1000) NOT NULL,
	[description] [text] NULL,
	[thumbnail] [nvarchar](1000) NULL,
	CONSTRAINT [Pk_Dim_Models] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
);

ALTER TABLE [stage].[Models]
ENABLE CHANGE_TRACKING  
WITH (TRACK_COLUMNS_UPDATED = ON);

CREATE TABLE [stage].[Properties](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[deviceId] [nvarchar](200) NOT NULL,
	[lastUpdated] [datetime] NOT NULL,
	[field] [nvarchar](255) NOT NULL,
	[kind] [nvarchar](50) NOT NULL,
	[numericValue] [decimal](30, 10) NULL,
	[stringValue] [nvarchar](max) NULL,
	[booleanValue] [bit] NULL,
	CONSTRAINT [Pk_Dim_Properties] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
);

ALTER TABLE [stage].[Properties]
ENABLE CHANGE_TRACKING  
WITH (TRACK_COLUMNS_UPDATED = ON);

CREATE TABLE [stage].[PropertyDefinitions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[modelId] [nvarchar](50) NOT NULL,
	[modelVersion] [nvarchar](50) NOT NULL,
	[field] [nvarchar](255) NOT NULL,
	[kind] [nvarchar](50) NOT NULL,
	[dataType] [nvarchar](100) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
	[optional] [bit] NULL,
	CONSTRAINT [Pk_Dim_PropertyDefinitions] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
);

ALTER TABLE [stage].[PropertyDefinitions]
ENABLE CHANGE_TRACKING  
WITH (TRACK_COLUMNS_UPDATED = ON);

CREATE TABLE [dbo].[date](
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
);
