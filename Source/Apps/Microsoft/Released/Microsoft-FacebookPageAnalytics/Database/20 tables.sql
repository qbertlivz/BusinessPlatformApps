SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;


CREATE TABLE fbpa.PagePostStoriesAndPeopleTalkingAboutThis
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.PageImpressions
(
			   EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.PageEngagement
(
			   EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.PageReactions
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.Clicks
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.PageUserDemographics
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.PageContent
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.PageViews
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.PageVideoViews
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [nvarchar](MAX),
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.PagePost
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.PagePostImpressions
(
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.PagePostEngagement
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.PagePostReactions
(
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.PageVideoPosts
(
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.PagePostsInfo
(
	[Id] [nvarchar](MAX),
	[PageId] [nvarchar](MAX),
	[Message] [nvarchar](MAX),
	[Created Time] [datetime],
	[Updated Time] [datetime],
	[Icon] [nvarchar](MAX),
	[Story] [nvarchar](MAX),
	[Link] [nvarchar](MAX),
	[Status Type] [nvarchar](MAX),
	[Is Hidden] [nvarchar](MAX),
	[Is Published] [nvarchar](MAX),
	[Name] [nvarchar](MAX),
	[Object] [nvarchar](MAX),
	[Permalink URL] [nvarchar](MAX),
	[Picture] [nvarchar](MAX),
	[Source] [nvarchar](MAX),
	[Shares] [nvarchar](MAX),
	[To Id] [nvarchar](MAX),
	[To Name] [nvarchar](MAX),
	[Type] [nvarchar](MAX)
)

CREATE TABLE fbpa.[PageTable]
(
	[idpage][nvarchar](100),
	[name][nvarchar](MAX)	
)

CREATE TABLE fbpa.STAGING_PagePostStoriesAndPeopleTalkingAboutThis
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.STAGING_PageImpressions
(
			   EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.STAGING_PageEngagement
(
			   EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.STAGING_PageReactions
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)
CREATE TABLE fbpa.STAGING_Clicks
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.STAGING_PageUserDemographics
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.STAGING_PageContent
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.STAGING_PageViews
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.STAGING_PageVideoViews
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [nvarchar](MAX),
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.STAGING_PagePost
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.STAGING_PagePostImpressions
(
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.STAGING_PagePostEngagement
(
               EndTime [datetime],
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.STAGING_PagePostReactions
(
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.STAGING_PageVideoPosts
(
               [Name] [nvarchar](MAX),
               [Entry Name] [nvarchar] (MAX),
               [Value] [decimal] NULL,
               [Period] [nvarchar](MAX),
               [Title] [nvarchar](MAX),
			   [Description][nvarchar](MAX),
               [Id] [nvarchar](MAX),
               PageId [nvarchar](30)
)

CREATE TABLE fbpa.STAGING_PagePostsInfo
(
	[Id] [nvarchar](MAX),
	[PageId] [nvarchar](MAX),
	[Message] [nvarchar](MAX),
	[Created Time] [datetime],
	[Updated Time] [datetime],
	[Icon] [nvarchar](MAX),
	[Story] [nvarchar](MAX),
	[Link] [nvarchar](MAX),
	[Status Type] [nvarchar](MAX),
	[Is Hidden] [nvarchar](MAX),
	[Is Published] [nvarchar](MAX),
	[Name] [nvarchar](MAX),
	[Object] [nvarchar](MAX),
	[Permalink URL] [nvarchar](MAX),
	[Picture] [nvarchar](MAX),
	[Source] [nvarchar](MAX),
	[Shares] [nvarchar](MAX),
	[To Id] [nvarchar](MAX),
	[To Name] [nvarchar](MAX),
	[Type] [nvarchar](MAX)
)


CREATE TABLE [fb].[Date](
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
)

CREATE TABLE fbpa.page_metric_unit 
(
	[metric][nvarchar](150) NULL,
	[description][nvarchar](MAX) NULL,
	[name][nvarchar](150) NULL,
	[name_sort][int] NULL,
	[image][nvarchar](MAX) NULL
)

CREATE TABLE [fbpa].[post_metric_unit]
(
	[metric][nvarchar](50) NULL,
	[name][nvarchar](50) NULL,
	[sort][int] NULL,
	[sortname][int] NULL
)

CREATE TABLE [fbpa].[page_unit](
	[unit] [nvarchar](30) NULL,
	[key_id] [nvarchar](100) NOT NULL,
	[metric] [nvarchar](45) NULL,
	[dim] [nvarchar](40) NULL,
	[metricsort] [int] NULL,
	[image_1] [nvarchar](255) NULL,
	[image_2] [nvarchar](255) NULL,
	[image_3] [nvarchar](255) NULL,
	[image_4] [nvarchar](255) NULL,
	[image_5] [nvarchar](255) NULL,
	[image_6] [nvarchar](255) NULL,
	[image_7] [nvarchar](255) NULL,
	[image_8] [nvarchar](255) NULL
)

CREATE TABLE [fbpa].[post_unit](
	[key_id] [nvarchar](100) NOT NULL,
	[metric] [nvarchar](45) NULL,
	[unit] [nvarchar](30) NULL,
	[dim] [nvarchar](20) NULL,
	[metricsort] [int] NULL,
	[image_1] [nvarchar](255) NULL,
	[image_2] [nvarchar](255) NULL,
	[image_3] [nvarchar](255) NULL,
	[image_4] [nvarchar](255) NULL,
	[image_5] [nvarchar](255) NULL,
	[image_6] [nvarchar](255) NULL,
	[image_7] [nvarchar](255) NULL,
	[image_8] [nvarchar](255) NULL
)

CREATE TABLE [fbpa].[period_post]
(
	[period][nvarchar](50),
	[periodsort][int],
	[period_name][nvarchar](50),
	[period_last][nvarchar](50)
)

CREATE TABLE [fbpa].[period_page]
(
	[period][nvarchar](50),
	[periodsort][int],
	[period_name][nvarchar](50),
	[period_last][nvarchar](50)
)

CREATE TABLE [fbpa].[period_page]
(
	[period][nvarchar](50),
	[periodsort][int],
	[period_name][nvarchar](50),
	[period_last][nvarchar](50)
)

CREATE TABLE [fbpa].[type]
(
	[type][nvarchar](10),
	[type_sort] [int],
	[name][nvarchar](10)
)

CREATE TABLE fbpa.[gender_age](
	[metric] [nvarchar](50) NULL,
	[metric_sort] [decimal](28, 0) NULL,
	[age] [nvarchar](50) NULL,
	[age_sort] [decimal](28, 0) NULL,
	[gender] [nvarchar](50) NULL,
	[gender_sort] [decimal](28, 0) NULL,
	[description] [nvarchar](250) NULL,
	[image] [nvarchar](300) NULL
)

CREATE TABLE [fbpa].[time](
	[metric] [varchar](50) NULL,
	[post_time] [varchar](50) NULL,
	[metric_sort] [decimal](28, 0) NULL,
	[-12] [time](7) NULL,
	[-11] [time](7) NULL,
	[-10] [time](7) NULL,
	[-9] [time](7) NULL,
	[-8] [time](7) NULL,
	[-7] [time](7) NULL,
	[-6] [time](7) NULL,
	[-5] [time](7) NULL,
	[-4] [time](7) NULL,
	[-3] [time](7) NULL,
	[-2] [time](7) NULL,
	[-1] [time](7) NULL,
	[+0] [time](7) NULL,
	[+1] [time](7) NULL,
	[+2] [time](7) NULL,
	[+3] [time](7) NULL,
	[+4] [time](7) NULL,
	[+5] [time](7) NULL,
	[+6] [time](7) NULL,
	[+7] [time](7) NULL,
	[+8] [time](7) NULL,
	[+9] [time](7) NULL,
	[+10] [time](7) NULL,
	[+11] [time](7) NULL,
	[+12] [time](7) NULL
)