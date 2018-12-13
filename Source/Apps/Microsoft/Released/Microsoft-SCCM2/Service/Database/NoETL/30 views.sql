SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

CREATE VIEW pbist_sccm.vw_DistributionPointsCompliancebyDPStatus
AS
    SELECT * FROM pbist_sccm.DistributionPointsCompliancebyDPStatus;
go


CREATE VIEW pbist_sccm.vw_DistributionPointsAllDPs
AS
    SELECT DISTINCT DistributionPoint FROM pbist_sccm.vw_DistributionPointsCompliancebyDPStatus;
go

CREATE VIEW pbist_sccm.vw_TasksequenceOverallTaskSequenceByComputerDeployment
AS
    SELECT * FROM pbist_sccm.TasksequenceOverallTaskSequenceByComputerDeployment;
go

CREATE VIEW pbist_sccm.vw_TasksequenceOverallTaskSequencebyTaskSequenceDeployment
AS
    SELECT * FROM pbist_sccm.TasksequenceOverallTaskSequencebyTaskSequenceDeployment;
go


CREATE VIEW pbist_sccm.vw_TaskSequenceAllTaskSequenceIDs
AS
    SELECT DISTINCT TaskSequenceID, Name FROM pbist_sccm.vw_TasksequenceOverallTaskSequenceByComputerDeployment
    UNION
    SELECT DISTINCT TSID TaskSequenceID, TaskSequenceName Name FROM pbist_sccm.vw_TasksequenceOverallTaskSequencebyTaskSequenceDeployment;
go


CREATE VIEW pbist_sccm.vw_SoftwareDistributionOverallApplicationDeployment
AS
    SELECT * FROM pbist_sccm.SoftwareDistributionOverallApplicationDeployment;
go

CREATE VIEW pbist_sccm.vw_SoftwareDistributionOverallPackageDeployment
AS
    SELECT * FROM pbist_sccm.SoftwareDistributionOverallPackageDeployment;
go


CREATE VIEW pbist_sccm.vw_SoftwareDistributionAllPackageIDs
AS
    SELECT Max(SoftwareName) AS [Package Name], packageid AS PackageID, 'Applications' AS [Type]
	FROM pbist_sccm.vw_SoftwareDistributionOverallApplicationDeployment
	WHERE packageid IS NOT NULL
	GROUP BY packageid 
    UNION
    SELECT Max(SoftwareName) AS [Package Name], packageid AS PackageID, 'Package' AS [Type]
	FROM pbist_sccm.vw_SoftwareDistributionOverallPackageDeployment
	WHERE packageid IS NOT NULL -- AND packageid NOT IN (SELECT DISTINCT packageid FROM pbist_sccm.vw_SoftwareDistributionOverallApplicationDeployment)
	GROUP BY packageid;
go


CREATE VIEW pbist_sccm.vw_ComplianceSettingComplianceByComputers
AS
    SELECT * FROM pbist_sccm.ComplianceSettingComplianceByComputers;
go

CREATE VIEW pbist_sccm.vw_ComplianceSettingComplianceCIForAllBaseline
AS
    SELECT * FROM pbist_sccm.ComplianceSettingComplianceCIForAllBaseline;
go


CREATE VIEW pbist_sccm.vw_ComplianceSettingsAllBaselineNames
AS
    SELECT BaselineName AS [Baseline Name] FROM pbist_sccm.vw_ComplianceSettingComplianceByComputers
    UNION
    SELECT ParentBaselineDisplayName AS [Baseline Name] FROM pbist_sccm.vw_ComplianceSettingComplianceCIForAllBaseline;
go


CREATE VIEW pbist_sccm.vw_ClientHealthClientInstalledVersionDetails
AS
    SELECT * FROM pbist_sccm.ClientHealthClientInstalledVersionDetails;
go

CREATE VIEW pbist_sccm.vw_ClientHealthClientsinventorystatisticslast30days
AS
    SELECT * FROM pbist_sccm.ClientHealthClientsinventorystatisticslast30days;
go

CREATE VIEW pbist_sccm.vw_ClientHealthCountAssignedClientsBySite
AS
    SELECT * FROM pbist_sccm.ClientHealthCountAssignedClientsBySite;
go

CREATE VIEW pbist_sccm.vw_ClientHealthCountClientsInstalledVersion
AS
    SELECT * FROM pbist_sccm.ClientHealthCountClientsInstalledVersion;
go

CREATE VIEW pbist_sccm.vw_ClientHealthCountClientsInstalledVersionPerSite
AS
    SELECT * FROM pbist_sccm.ClientHealthCountClientsInstalledVersionPerSite;
go

CREATE VIEW pbist_sccm.vw_ClientHealthEvaluationCount
AS
    SELECT * FROM pbist_sccm.ClientHealthEvaluationCount;
go

CREATE VIEW pbist_sccm.vw_ClientHealthSummary
AS
    SELECT [Status], NClients, PCT FROM pbist_sccm.ClientHealthSummary;
go


CREATE VIEW pbist_sccm.vw_ClientHealthWUAVersionAllClients
AS
    SELECT * FROM pbist_sccm.ClientHealthWUAVersionAllClients;
go

CREATE VIEW pbist_sccm.vw_collection
AS 
  SELECT collectionid, 
         [collection name] 
  FROM   pbist_sccm.collection;
go

CREATE VIEW pbist_sccm.vw_ComponentStatus
AS
SELECT        NbCompStatus, Status
FROM            pbist_sccm.ComponentStatus;
go




CREATE VIEW pbist_sccm.vw_computer
AS 
SELECT machineid
      ,sitecode
      ,name
      ,case [Client Type] when 1 then 'Computer' when 3 then 'Mobile' else 'tbd' end [client type]
      ,CASE
            when [operating system] like 'iOS%' then 'iOS'
            when [operating system] like 'Android%' then 'Android'
            when [operating system] like 'OS X%' then 'OS X'
            when [operating system]='Microsoft Windows NT Advanced Server 6.3' OR [operating system]='Microsoft Windows NT Advanced Server 6.4' OR [operating system]='Windows Technical Preview for Enterprise 6.4'
                then 'Window Server 2012 R2'
            when [operating System]='Microsoft Windows NT Workstation 5.0' then 'Windows 2000 Professional'
            when [operating system] like '%Windows%Workstation 6.1%' then 'Windows 7'
            when [operating system] like '%Windows%Workstation 6.2%' then 'Windows 8'
            when [operating system] like '%Windows%Workstation 6.3%' OR [operating system] like '%Windows%Workstation 6.4%' OR [operating system] like 'Windows 8.1%' then 'Windows 8.1'
            when [operating system] ='Microsoft Windows NT Advanced Server 5.2' then 'Windows Server 2003'
            when [operating system] ='Microsoft Windows NT Advanced Server 6.0' then 'Windows Server 2008'
            when [operating system] ='Microsoft Windows NT Advanced Server 6.1' then 'Windows Server 2008 R2'
            when [operating system] ='Microsoft Windows NT Advanced Server 6.2' then 'Windows Server 2012'
            when [operating system] ='Microsoft Windows NT Advanced Server 10.0'
                then 'Windows Server 2016'
            when [operating system] like 'Windows Phone%' then 'Windows Phone'
            when [operating system] like 'Windows 10%'
                OR [operating system]='Microsoft Windows NT Workstation 10.0'
                OR [operating system]='Microsoft Windows NT Workstation 10.0 (Tablet Edition)'
                OR [operating system]='Microsoft Windows NT Server 10.0'
                then 'Windows 10'
            when  [operating system]='Microsoft Windows NT Advanced Server 10.0'
                OR [operating system] like 'Windows Server 2016%'
                then 'Windows Server 2016'
            when [operating system]='Microsoft Windows NT Server 6.0'
                OR [operating system]='Microsoft Windows NT Workstation 6.0'
                OR [operating system]='Microsoft Windows NT Workstation 6.0 (Tablet Edition)'
                then 'Windows Vista'
            when [operating system]='Microsoft Windows NT Workstation 5.1'
                OR [operating system]='Microsoft Windows NT Server 5.2'
                then 'Windows XP'
            else [operating system]
      end [operating system name]
      ,[operating system] [operating system long name]
      ,manufacturer
      ,model
      ,[platform]
      ,[physical memory]
  FROM pbist_sccm.computer c WHERE c.[deleted date] IS NULL;
go


CREATE VIEW pbist_sccm.vw_computercollection
AS
  SELECT collectionid,
         resourceid
  FROM   pbist_sccm.computercollection;
go

-- Computer malware
CREATE VIEW pbist_sccm.vw_computermalware
AS
    SELECT threatid,
           machineid,
           10000*Year([detection date]) + 100*Month([detection date]) + Day([detection date]) date_key,
           [observer product name],
           [observer product version],
           [observer detection],
           [remediation type],
           [remediation result],
           [remediation error code],
           [remediation pending action],
           [is active malware]
    FROM   pbist_sccm.computermalware
    WHERE DateDiff(day, [Detection Date], GetDate()) <=365;
go

-- Computer program
CREATE VIEW pbist_sccm.vw_computerprogram
AS 
  SELECT machineid, 
         [program name],
         publisher,
         [version]
  FROM   pbist_sccm.computerprogram;
go

-- Computer update
CREATE VIEW pbist_sccm.vw_computerupdate
AS 
  SELECT machineid, 
         ci_id, 
         laststatuschangetime [last status change time], 
         laststatuschecktime  [last status check time], 
         [status]
  FROM   pbist_sccm.computerupdate;
go

CREATE VIEW pbist_sccm.vw_ConfigMgrServerHealthBackupStatus
AS
    SELECT * FROM pbist_sccm.ConfigMgrServerHealthBackupStatus;
go

CREATE VIEW pbist_sccm.vw_ConfigMgrServerHealthComponentStatusDetails
AS
    SELECT * FROM pbist_sccm.ConfigMgrServerHealthComponentStatusDetails;
go

CREATE VIEW pbist_sccm.vw_ConfigMgrServerHealthCountComponentStatusMessagesBySeverity
AS
    SELECT * FROM pbist_sccm.ConfigMgrServerHealthCountComponentStatusMessagesBySeverity;
go

-- ConfigurationView
CREATE VIEW pbist_sccm.vw_configuration
AS
    SELECT [id],
            configuration_group    AS [configuration group],
            configuration_subgroup AS [configuration subgroup],
            [name],
            [value],
			CASE
			   WHEN [name]='lastLoadTimestamp' AND configuration_subgroup='System Center' THEN CONVERT(datetime, [value], 126) -- We need this because Power BI / DAX cannot convert from ISO8601 date
			   ELSE NULL
			END AS value_as_datetime
    FROM   pbist_sccm.[configuration]
    WHERE  visible = 1;
go

-- DateView
CREATE VIEW pbist_sccm.vw_date
AS
    SELECT date_key,
           full_date        AS date,
           day_of_week      AS [day of the week],
           day_num_in_month AS [day number of the month],
           day_name         AS [day name],
           day_abbrev       AS [day abbreviated],
           weekday_flag     AS [weekday flag],
           [month],
           month_name       AS [month name],
           month_abbrev,
           [quarter],
           [year],
           same_day_year_ago_date,
           week_begin_date  AS [week begin date]
    FROM   pbist_sccm.[date];
go

CREATE VIEW pbist_sccm.vw_DistributionPointsCompliancebyPackagesStatus
AS
    SELECT * FROM pbist_sccm.DistributionPointsCompliancebyPackagesStatus;
go

CREATE VIEW pbist_sccm.vw_DistributionPointsConfigurationStatus
AS
    SELECT * FROM pbist_sccm.DistributionPointsConfigurationStatus;
go

CREATE VIEW pbist_sccm.vw_DistributionPointsStatus
AS
    SELECT * FROM pbist_sccm.DistributionPointsStatus;
go

CREATE VIEW pbist_sccm.vw_OverallComplianceBaseline
AS
    SELECT * FROM pbist_sccm.OverallComplianceBaseline;
go

-- Program View
CREATE VIEW pbist_sccm.vw_program
AS 
  SELECT [program name],
         publisher,
         [version]
  FROM   pbist_sccm.program
  WHERE  [program name] IS NOT NULL AND
         publisher IS NOT NULL AND
         [version] IS NOT NULL AND	  
         UNICODE([program name]) <> 127 AND
         UNICODE(RIGHT([version], 1)) > 31;
go

CREATE VIEW pbist_sccm.vw_ReplicationStatus
AS
   SELECT * FROM pbist_sccm.ReplicationStatus;
go


CREATE VIEW pbist_sccm.vw_ReplicationStatusDetails
AS
    SELECT * FROM pbist_sccm.ReplicationStatusDetails;
go


CREATE VIEW pbist_sccm.vw_ReplicationStatusReplicationGroup
AS
    SELECT * FROM pbist_sccm.ReplicationStatusReplicationGroup;
go

CREATE VIEW pbist_sccm.vw_scanhistory
AS
  SELECT machineid,
         date_key,
         sitecode,
         [enabled],
         [client version],
         CONVERT(CHAR(1), [real time protection enabled])    [real time protection enabled],
         CONVERT(CHAR(1), [on access protection enabled])    [on access protection enabled],
         CONVERT(CHAR(1), [input/output protection enabled]) [input/output protection enabled],
         CONVERT(CHAR(1), [behavior monitor enabled])        [behavior monitor enabled],
         CONVERT(CHAR(1), [antivirus enabled])               [antivirus enabled],
         CONVERT(CHAR(1), [antispyware enabled])             [antispyware enabled],
         CONVERT(CHAR(1), [nis enabled])                     [nis enabled],
         [quick scan age (days)],
         [full scan age (days)],
         [signature age (days)],
         [engine version],
         [antivirus signature version],
         [missing critical update count],
         [client active status],
         [health evaluation result],
         [client state]              -- 1: Active/Pass, 2: Active/Fail, 3; Active/Unknown, 4: Inactive/Pass, 5: Inactive/Fail, 6; Inactive/Unknown,

  FROM   pbist_sccm.scanhistory;
go

CREATE VIEW pbist_sccm.vw_SoftwareDistribution
AS
    SELECT * FROM pbist_sccm.SoftwareDistribution;
go

CREATE VIEW pbist_sccm.vw_SoftwareDistributionOverallPckAPPDPdistribution
AS
    SELECT * FROM pbist_sccm.SoftwareDistributionOverallPckAPPDPdistribution;
go

CREATE VIEW pbist_sccm.vw_TaskSequenceDeployment
AS
    SELECT * FROM pbist_sccm.TaskSequenceDeployment;
go

CREATE VIEW pbist_sccm.vw_TasksequenceDPContentStatusReferencedPackagesTaskSequence
AS
    SELECT * FROM pbist_sccm.TasksequenceDPContentStatusReferencedPackagesTaskSequence;
go


CREATE VIEW pbist_sccm.vw_update
AS
  SELECT ci_id,
         articleid,
         bulletinid,
         severity,
         CASE
           WHEN severityname IS NULL OR Len(severityname) = 0 THEN 'None'
           ELSE severityname
         END [severity name],
         title,
         infoURL
  FROM   pbist_sccm.[update];
go

CREATE VIEW pbist_sccm.vw_user
AS 
  SELECT username, 
         [full name] 
  FROM   pbist_sccm.[user];
go

CREATE VIEW pbist_sccm.vw_usercomputer
AS
  SELECT machineid,
         username, 
         [full name] 
  FROM   pbist_sccm.usercomputer;
go

CREATE VIEW [pbist_sccm].[vw_SCEPDefinition]
AS
    SELECT * FROM pbist_sccm.SCEPDefinition;
go
