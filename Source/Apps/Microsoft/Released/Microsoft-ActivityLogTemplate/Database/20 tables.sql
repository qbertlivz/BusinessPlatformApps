﻿SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;

CREATE TABLE bpst_aal.ActivityLogData (
	eventId INT IDENTITY(1, 1) PRIMARY KEY,
	[caller] VARCHAR(MAX),
	correlationId VARCHAR(250),
	[description] VARCHAR(MAX),
	eventCategory VARCHAR(MAX), 
	impact VARCHAR(MAX),
	impactedRegions VARCHAR(MAX),
	impactedServices VARCHAR(MAX),
	jobFailedMessage VARCHAR(MAX),
	[level] VARCHAR(MAX),
	operationCategory VARCHAR(MAX),
	operationId VARCHAR(MAX),
	operationName VARCHAR(MAX),
	resourceGroup VARCHAR(MAX),
	resourceId VARCHAR(MAX),
	[status] VARCHAR(MAX),
	statusCode VARCHAR(MAX),
	subscriptionId VARCHAR(MAX),
	[timestamp] VARCHAR(MAX)
);

CREATE TABLE bpst_aal.[Configuration](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[configuration_group] [varchar](150) NOT NULL,
	[configuration_subgroup] [varchar](150) NOT NULL,
	[name] [varchar](150) NOT NULL,
	[value] [varchar](max) NULL,
	[visible] [bit] NOT NULL
);

CREATE TABLE bpst_aal.[date](
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);
