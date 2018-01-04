SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

Create View [vs].[rpt_Date]
AS
SELECT [date_key] as DateId
      ,[full_date] as [Date]
      ,[day_of_week] as [DayOfWeek]
      ,[day_num_in_month] as [DayOfMonth]
      ,[day_name] as [DayName]
      ,[day_abbrev] as DayAbbr
      ,[weekday_flag] as IsWeekday
      ,[week_num_in_year] as WeekOfYear
      ,[week_begin_date] as FirstDayOfWeek
      ,[month] as [Month]
      ,[month_name] as [MonthName]
      ,[month_abbrev] as [MonthAbbr]
      ,[quarter] as [Quarter]
      ,[year] as [Year]
      ,[yearmo] as YYYYMM
  FROM [vs].[Date]
GO

Create View [vs].[rpt_Iteration]
AS
SELECT [IterationId]
      ,[NativeId]
      ,[Name]
      ,[Path]
      ,[StartDate]
      ,[FinishDate]
      ,[url]
      ,[ETLImportDate]
  FROM [vs].[Iteration]
GO

Create View [vs].[rpt_Person]
AS
SELECT [PersonId]
      ,[NativeId]
      ,[UserName]
      ,[UserEmail]
      ,[Name_Email]
      ,[ETLImportDate]
  FROM [vs].[Person]
GO

Create View [vs].[rpt_Project]
AS
SELECT [ProjectId]
      ,[NativeId]
      ,[Name]
      ,[Description]
      ,[url]
      ,[State]
      ,[Revision]
      ,[Visibility]
      ,[ETLImportDate]
  FROM [vs].[Project]
GO


CREATE View [vs].[rpt_WorkItemRevision]
AS
SELECT [WorkOrderRevisionId]
      ,[NativeId]
      ,[AreaId]
      ,[AreaPath]
      ,[TeamProject]
      ,[NodeName]
      ,[AreaLevel1]
      ,[RevisionNumber]
      ,[AuthorizedDate]
      ,[RevisedDate]
      ,[IterationId]
      ,[IterationPath]
      ,[IterationLevel1]
      ,[IterationLevel2]
      ,[WorkItemType]
      ,[State]
      ,[Reason]
      ,[AssignedTo]
      ,[CreatedDate]
      ,[CreatedBy]
      ,[ChangedDate]
      ,[ChangedBy]
      ,[AuthorizedAs]
      ,[NativeAuthorizedAsPersonId]
      ,[Watermark]
      ,[AttachedFileCount]
      ,[HyperLinkCount]
      ,[ExternalLinkCount]
      ,[RelatedLinkCount]
      ,[Title]
      ,[BoardColumnDone]
      ,[ActivatedDate]
      ,[ActivatedBy]
      ,[ResolvedDate]
      ,[ResolvedBy]
      ,[Priority]
      ,[StackRank]
      ,[ValueArea]
      ,[BoardColumn]
      ,[StateChangeDate]
      ,[StoryPoints]
      ,[ClosedDate]
      ,[ClosedBy]
      ,[Risk]
      ,[StartDate]
      ,[DueDate]
      ,[Effort]
      ,[Activity]
      ,[RemainingWork]
      ,[OriginalEstimate]
      ,[CompletedWork]
      ,[Severity]
      ,[FinishDate]
      ,[BacklogPriority]
      ,[ETLImportDate]
	  ,CASE WHEN [RevisedDate] = '9998-12-31 17:00:00.000'
			THEN 1
			Else 0	END as IsLastRevision
	  ,CONVERT(int,CONVERT(char(8),[CreatedDate], 112))		as CreatedDateKey
	  ,CASE WHEN [DueDate] < GETDATE() AND [RevisedDate] = '9998-12-31 17:00:00.000'
			THEN 1
			ELSE 0 END				as IsPastDue
  FROM [vs].[WorkItemRevision]
GO

