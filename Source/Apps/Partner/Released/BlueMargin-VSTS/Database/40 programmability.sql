SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

CREATE Proc [vs].[Merge]
AS
BEGIN
---------------------------Delete and Insert Person-------------------------
BEGIN TRAN

Delete t
FROM [vs].[Person] AS t
INNER JOIN [vs].[stg_WorkItem] AS s
	ON t.NativeId = s.PersonId;

INSERT INTO [vs].[Person]
(
 [NativeId]
 ,[UserName]
 ,[UserEmail]
 ,[Name_Email]
)
SELECT Distinct
    [PersonId]													as NativeId
	,Left([AuthorizedAs],CharIndex('<',[AuthorizedAs])-2)		as [UserName]
	,SUBSTRING([AuthorizedAs],
	CharIndex('<',[AuthorizedAs])+1,
		Len([AuthorizedAs])-CharIndex('<',[AuthorizedAs])-1)	as [UserEmail]
	,[AuthorizedAs]												as [Name_Email]
FROM [vs].[stg_WorkItem]

COMMIT

---------------------------Delete and Insert Project------------------------
BEGIN TRAN

DELETE t
FROM [vs].[Project] AS t
INNER JOIN [vs].[stg_Project] AS s
	ON t.NativeId = s.id;

Insert Into [vs].[Project]
(
      [NativeId]
      ,[Name]
      ,[Description]
      ,[url]
      ,[State]
      ,[Revision]
      ,[Visibility]
)
SELECT 
	  [id]				as [NativeId]
      ,[name]			as [Name]
      ,[description]	as [Description]
      ,[url]			as [url]
      ,[state]			as [State]
      ,[revision]		as [Revision]
      ,[visibility]		as [Visibility]
FROM [vs].[stg_Project];

TRUNCATE TABLE [vs].[stg_Project];

COMMIT

-------------------Delete and Insert Iteration-------------------
BEGIN TRAN

DELETE t
FROM [vs].[Iteration] as t
INNER Join [vs].[stg_Iteration] s
	ON t.NativeId = s.id;

Insert into [vs].[Iteration]
(
	[NativeId]
    ,[Name]
    ,[Path]
    ,[StartDate]
    ,[FinishDate]
    ,[url]
)
SELECT 
	[id]			as [NativeId]
    ,[name]			as [Name]
    ,[path]			as [Path]
    ,[startDate]	as [StartDate]
    ,[finishDate]	as [FinishDate]
    ,[url]			as [url]
From [vs].[stg_Iteration]

TRUNCATE TABLE [vs].[stg_Iteration];

COMMIT

--TODO: Work item.  Replace work item id's with Destination table id's
----------------Delete and Insert Work Item Revision-----------------

BEGIN TRAN

DELETE t
FROM [vs].[WorkItemRevision] as t
INNER Join [vs].[stg_WorkItem] s
	ON t.NativeId = s.id
	AND t.[RevisionNumber] = s.[Rev]
	AND t.[IterationId] = s.[IterationId];

Insert into [vs].[WorkItemRevision]
(
	  [NativeId]
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
	)

SELECT 
	[Id]
      ,[AreaId]
      ,[AreaPath]
      ,[TeamProject]
      ,[NodeName]
      ,[AreaLevel1]
      ,[Rev]
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
      ,[PersonId]
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
FROM [vs].[stg_WorkItem]

TRUNCATE TABLE [vs].[stg_WorkItem];

COMMIT
END
GO
