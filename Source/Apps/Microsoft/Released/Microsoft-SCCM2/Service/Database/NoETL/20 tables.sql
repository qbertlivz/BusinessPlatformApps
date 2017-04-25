SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ANSI_NULL_DFLT_ON ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER ON;
go

CREATE TABLE pbist_sccm.clienthealthclientinstalledversiondetails
(
     netbios          NVARCHAR(256) NULL,
     domain           NVARCHAR(256) NULL,
     [user]           NVARCHAR(256) NULL,
     [client version] NVARCHAR(256) NULL,
     [creation date]  DATETIME NULL,
     [last ddr]       DATETIME NULL,
     machineid        INT NULL
);

CREATE TABLE pbist_sccm.clienthealthclientinstalledversiondetails_staging
(
     netbios          NVARCHAR(256) NULL,
     domain           NVARCHAR(256) NULL,
     [user]           NVARCHAR(256) NULL,
     [client version] NVARCHAR(256) NULL,
     [creation date]  DATETIME NULL,
     [last ddr]       DATETIME NULL,
     machineid        INT NULL
);

CREATE TABLE pbist_sccm.clienthealthclientsinventorystatisticslast30days
(
     totalrecords               INT NULL,
     clients                    INT NULL,
     clientspct                 NUMERIC(27, 13) NULL,
     noclients                  INT NULL,
     noclientspct               NUMERIC(27, 13) NULL,
     obsolete                   INT NULL,
     obsoletepct                NUMERIC(27, 13) NULL,
     validclients               INT NULL,
     validclientspct            NUMERIC(27, 13) NULL,
     activeclientscurrent       INT NULL,
     activeclientscurrentpct    NUMERIC(27, 13) NULL,
     inactiveclientscurrent     INT NULL,
     inactiveclientscurrentpct  NUMERIC(27, 13) NULL,
     activeclientslast30days    INT NULL,
     activeclientslast30dayspct NUMERIC(27, 13) NULL,
     hwsuccesslast30days        INT NULL,
     hwsuccesslast30dayspct     NUMERIC(27, 13) NULL,
     hwnotsuccesslast30days     INT NULL,
     hwnotsuccesslast30dayspct  NUMERIC(27, 13) NULL,
     swsuccesslast30days        INT NULL,
     swsuccesslast30dayspct     NUMERIC(27, 13) NULL,
     swnotsuccesslast30days     INT NULL,
     swnotsuccesslast30dayspct  NUMERIC(27, 13) NULL,
     hwinvmissing               INT NULL,
     hwinvmissingpct            NUMERIC(27, 13) NULL,
     swinvmissing               INT NULL,
     swinvmissingpct            NUMERIC(27, 13) NULL
);

CREATE TABLE pbist_sccm.clienthealthclientsinventorystatisticslast30days_staging
(
     totalrecords               INT NULL,
     clients                    INT NULL,
     clientspct                 NUMERIC(27, 13) NULL,
     noclients                  INT NULL,
     noclientspct               NUMERIC(27, 13) NULL,
     obsolete                   INT NULL,
     obsoletepct                NUMERIC(27, 13) NULL,
     validclients               INT NULL,
     validclientspct            NUMERIC(27, 13) NULL,
     activeclientscurrent       INT NULL,
     activeclientscurrentpct    NUMERIC(27, 13) NULL,
     inactiveclientscurrent     INT NULL,
     inactiveclientscurrentpct  NUMERIC(27, 13) NULL,
     activeclientslast30days    INT NULL,
     activeclientslast30dayspct NUMERIC(27, 13) NULL,
     hwsuccesslast30days        INT NULL,
     hwsuccesslast30dayspct     NUMERIC(27, 13) NULL,
     hwnotsuccesslast30days     INT NULL,
     hwnotsuccesslast30dayspct  NUMERIC(27, 13) NULL,
     swsuccesslast30days        INT NULL,
     swsuccesslast30dayspct     NUMERIC(27, 13) NULL,
     swnotsuccesslast30days     INT NULL,
     swnotsuccesslast30dayspct  NUMERIC(27, 13) NULL,
     hwinvmissing               INT NULL,
     hwinvmissingpct            NUMERIC(27, 13) NULL,
     swinvmissing               INT NULL,
     swinvmissingpct            NUMERIC(27, 13) NULL
);

CREATE TABLE pbist_sccm.clienthealthcountassignedclientsbysite
(
     [configmgr site]         NVARCHAR(446) NOT NULL,
     [total clients assigned] INT NULL
);

CREATE TABLE pbist_sccm.clienthealthcountassignedclientsbysite_staging
(
     [configmgr site]         NVARCHAR(446) NOT NULL,
     [total clients assigned] INT NULL
);

CREATE TABLE pbist_sccm.clienthealthcountclientsinstalledversion
(
     versionnumber NVARCHAR(256) NULL,
     [description] NVARCHAR(256) NULL,
     total         INT NULL
);

CREATE TABLE pbist_sccm.clienthealthcountclientsinstalledversion_staging
(
     versionnumber NVARCHAR(256) NULL,
     [description] NVARCHAR(256) NULL,
     total         INT NULL
);

CREATE TABLE pbist_sccm.clienthealthcountclientsinstalledversionpersite
(
     description   NVARCHAR(256) NULL,
     versionnumber NVARCHAR(256) NULL,
     [site code]   NVARCHAR(446) NOT NULL,
     total         INT NULL
);

CREATE TABLE pbist_sccm.clienthealthcountclientsinstalledversionpersite_staging
(
     [description] NVARCHAR(256) NULL,
     versionnumber NVARCHAR(256) NULL,
     [site code]   NVARCHAR(446) NOT NULL,
     total         INT NULL
);

CREATE TABLE pbist_sccm.clienthealthevaluationcount
(
     total                 INT NULL,
     health                VARCHAR(16) NULL,
     lastevaluationhealthy INT NOT NULL
);

CREATE TABLE pbist_sccm.clienthealthevaluationcount_staging
(
     total                 INT NULL,
     health                VARCHAR(16) NULL,
     lastevaluationhealthy INT NOT NULL
);

CREATE TABLE pbist_sccm.clienthealthsummary
(
     status   VARCHAR(8) NULL,
     nclients INT NULL,
     pct      DECIMAL(5, 4) NULL
);

CREATE TABLE pbist_sccm.clienthealthsummary_staging
(
     status   VARCHAR(8) NULL,
     nclients INT NULL,
     pct      DECIMAL(5, 4) NULL
);

CREATE TABLE pbist_sccm.clienthealthwuaversionallclients
(
     computername        NVARCHAR(256) NULL,
     [wua agent version] NVARCHAR(255) NULL,
     [operating system]  NVARCHAR(255) NULL,
     [service pack]      NVARCHAR(255) NULL,
     total               INT NULL,
	 machineid           INT NULL
);

CREATE TABLE pbist_sccm.clienthealthwuaversionallclients_staging
(
     computername        NVARCHAR(256) NULL,
     [wua agent version] NVARCHAR(255) NULL,
     [operating system]  NVARCHAR(255) NULL,
     [service pack]      NVARCHAR(255) NULL,
     total               INT NULL,
	 machineid           INT NULL
);

CREATE TABLE pbist_sccm.collection
(
     collectionid      NVARCHAR(8) NOT NULL,
     [collection name] NVARCHAR(255) NOT NULL
);

CREATE TABLE pbist_sccm.collection_staging
(
     collectionid      NVARCHAR(8) NOT NULL,
     [collection name] NVARCHAR(255) NOT NULL
);

CREATE TABLE pbist_sccm.compliancesettingcompliancebycomputers
(
     machinename                 NVARCHAR(256) NULL,
     domain                      NVARCHAR(256) NULL,
     adsite                      NVARCHAR(256) NULL,
     errorcode                   NVARCHAR(max) NULL,
     baselinename                NVARCHAR(512) NULL,
     baselinecontentversion      INT NULL,
     compliancestate             NVARCHAR(512) NULL,
     maxnoncompliancecriticality INT NULL,
     lastcompliancemessagetime   DATETIME NULL,
     baseline_uniqueid           NVARCHAR(512) NULL,
	 machineid					 INT NULL
);

CREATE TABLE pbist_sccm.compliancesettingcompliancebycomputers_staging
(
     machinename                 NVARCHAR(256) NULL,
     domain                      NVARCHAR(256) NULL,
     adsite                      NVARCHAR(256) NULL,
     errorcode                   NVARCHAR(max) NULL,
     baselinename                NVARCHAR(512) NULL,
     baselinecontentversion      INT NULL,
     compliancestate             NVARCHAR(512) NULL,
     maxnoncompliancecriticality INT NULL,
     lastcompliancemessagetime   DATETIME NULL,
     baseline_uniqueid           NVARCHAR(512) NULL,
	 machineid					 INT NULL
);

CREATE TABLE pbist_sccm.compliancesettingcomplianceciforallbaseline
(
     baselinecontentversion      INT NULL,
     parentbaselinedisplayname   NVARCHAR(512) NULL,
     parentbaselineversion       INT NULL,
     isbaseline                  INT NOT NULL,
     subbaselinedisplayname      NVARCHAR(4000) NULL,
     bl_ci_id                    INT NULL,
     ci_id                       INT NULL,
     subbaselinename             NVARCHAR(512) NULL,
     finalorder                  NVARCHAR(1057) NULL,
     subbaselinecontentversion   INT NULL,
     baselinepolicy              INT NULL,
     configurationitemtype       INT NULL,
     configurationitemname       NVARCHAR(512) NULL,
     cicontentversion            INT NULL,
     countcompliant              INT NULL,
     countnoncompliant           INT NULL,
     failurecount                INT NULL,
     countenforced               INT NULL,
     countnotapplicable          INT NULL,
     countnotdetected            INT NULL,
     countunknown                INT NULL,
     countreported               INT NULL,
     counttargeted               INT NULL,
     maxnoncompliancecriticality INT NULL,
     compliancepercentage        NUMERIC(5, 2) NULL,
     categories                  NVARCHAR(512) NULL,
     subbaseline_uniqueid        NVARCHAR(512) NULL,
     ci_uniqueid                 NVARCHAR(512) NULL
);

CREATE TABLE pbist_sccm.compliancesettingcomplianceciforallbaseline_staging
(
     baselinecontentversion      INT NULL,
     parentbaselinedisplayname   NVARCHAR(512) NULL,
     parentbaselineversion       INT NULL,
     isbaseline                  INT NOT NULL,
     subbaselinedisplayname      NVARCHAR(4000) NULL,
     bl_ci_id                    INT NULL,
     ci_id                       INT NULL,
     subbaselinename             NVARCHAR(512) NULL,
     finalorder                  NVARCHAR(1057) NULL,
     subbaselinecontentversion   INT NULL,
     baselinepolicy              INT NULL,
     configurationitemtype       INT NULL,
     configurationitemname       NVARCHAR(512) NULL,
     cicontentversion            INT NULL,
     countcompliant              INT NULL,
     countnoncompliant           INT NULL,
     failurecount                INT NULL,
     countenforced               INT NULL,
     countnotapplicable          INT NULL,
     countnotdetected            INT NULL,
     countunknown                INT NULL,
     countreported               INT NULL,
     counttargeted               INT NULL,
     maxnoncompliancecriticality INT NULL,
     compliancepercentage        NUMERIC(5, 2) NULL,
     categories                  NVARCHAR(512) NULL,
     subbaseline_uniqueid        NVARCHAR(512) NULL,
     ci_uniqueid                 NVARCHAR(512) NULL
);

CREATE TABLE pbist_sccm.componentstatus
(
     nbcompstatus INT NULL,
     status       VARCHAR(8) NULL
);

CREATE TABLE pbist_sccm.componentstatus_staging
(
     nbcompstatus INT NULL,
     status       VARCHAR(8) NULL
);

CREATE TABLE pbist_sccm.computer
(
     machineid          INT NOT NULL,
     sitecode           NVARCHAR(3) NULL,
     NAME               NVARCHAR(256) NULL,
     [operating system] NVARCHAR(256) NULL,
     [client type]      TINYINT NULL,
     manufacturer       NVARCHAR(255) NULL,
     model              NVARCHAR(255) NULL,
     platform           NVARCHAR(255) NULL,
     [physical memory]  BIGINT NULL,
     [deleted date]     DATETIME NULL
);

CREATE TABLE pbist_sccm.computer_staging
(
     machineid          INT NOT NULL,
     sitecode           NVARCHAR(3) NULL,
     NAME               NVARCHAR(256) NULL,
     [operating system] NVARCHAR(256) NULL,
     [client type]      TINYINT NULL,
     manufacturer       NVARCHAR(255) NULL,
     model              NVARCHAR(255) NULL,
     platform           NVARCHAR(255) NULL,
     [physical memory]  BIGINT NULL
);

CREATE TABLE pbist_sccm.computercollection
(
     collectionid NVARCHAR(8) NOT NULL,
     resourceid   INT NOT NULL
);

CREATE TABLE pbist_sccm.computercollection_staging
(
     collectionid NVARCHAR(8) NOT NULL,
     resourceid   INT NOT NULL
);

CREATE TABLE pbist_sccm.computermalware
(
     threatid                     BIGINT NULL,
     machineid                    INT NOT NULL,
     [detection date]             DATETIME NULL,
     [observer product name]      NVARCHAR(32) NOT NULL,
     [observer product version]   NVARCHAR(255) NULL,
     [observer detection]         NVARCHAR(8) NULL,
     [remediation type]           NVARCHAR(11) NULL,
     [remediation result]         NVARCHAR(5) NOT NULL,
     [remediation error code]     INT NULL,
     [remediation pending action] NVARCHAR(16) NOT NULL,
     [is active malware]          NVARCHAR(5) NOT NULL
);

CREATE TABLE pbist_sccm.computermalware_staging
(
     threatid                     BIGINT NULL,
     machineid                    INT NOT NULL,
     [detection date]             DATETIME NULL,
     [observer product name]      NVARCHAR(32) NOT NULL,
     [observer product version]   NVARCHAR(255) NULL,
     [observer detection]         NVARCHAR(8) NULL,
     [remediation type]           NVARCHAR(11) NULL,
     [remediation result]         NVARCHAR(5) NOT NULL,
     [remediation error code]     INT NULL,
     [remediation pending action] NVARCHAR(16) NOT NULL,
     [is active malware]          NVARCHAR(5) NOT NULL
);

CREATE TABLE pbist_sccm.computerprogram
(
     machineid      INT NOT NULL,
     [program name] NVARCHAR(255) NOT NULL,
     publisher      NVARCHAR(255) NULL,
     version        NVARCHAR(255) NULL,
     timestamp      DATETIME NULL
);

CREATE TABLE pbist_sccm.computerprogram_staging
(
     machineid      INT NOT NULL,
     [program name] NVARCHAR(255) NOT NULL,
     publisher      NVARCHAR(255) NULL,
     version        NVARCHAR(255) NULL,
     timestamp      DATETIME NULL
);

CREATE TABLE pbist_sccm.computerupdate
(
     machineid            INT NOT NULL,
     ci_id                INT NOT NULL,
     laststatuschangetime DATETIME NULL,
     laststatuschecktime  DATETIME NULL,
     status               TINYINT NOT NULL
);

CREATE TABLE pbist_sccm.computerupdate_staging
(
     machineid            INT NOT NULL,
     ci_id                INT NOT NULL,
     laststatuschangetime DATETIME NULL,
     laststatuschecktime  DATETIME NULL,
     status               TINYINT NOT NULL
);

CREATE TABLE pbist_sccm.configmgrserverhealthbackupstatus
(
     sitecode      NVARCHAR(3) NOT NULL,
     [backup task] VARCHAR(8) NULL,
     time          DATETIME NULL
);

CREATE TABLE pbist_sccm.configmgrserverhealthbackupstatus_staging
(
     sitecode      NVARCHAR(3) NOT NULL,
     [backup task] VARCHAR(8) NULL,
     time          DATETIME NULL
);

CREATE TABLE pbist_sccm.configmgrserverhealthcomponentstatusdetails
(
     status               VARCHAR(8) NULL,
     [component name]     NVARCHAR(295) NOT NULL,
     [site system]        NVARCHAR(255) NOT NULL,
     [component type]     NVARCHAR(64) NULL,
     [site code]          NVARCHAR(3) NOT NULL,
     [availability state] VARCHAR(7) NULL,
     state                VARCHAR(13) NULL,
     type                 VARCHAR(13) NULL,
     [infos counter]      INT NULL,
     [warnings counter]   INT NULL,
     [errors counter]     INT NULL,
     [last contacted]     DATETIME NULL
);

CREATE TABLE pbist_sccm.configmgrserverhealthcomponentstatusdetails_staging
(
     status               VARCHAR(8) NULL,
     [component name]     NVARCHAR(295) NOT NULL,
     [site system]        NVARCHAR(255) NOT NULL,
     [component type]     NVARCHAR(64) NULL,
     [site code]          NVARCHAR(3) NOT NULL,
     [availability state] VARCHAR(7) NULL,
     state                VARCHAR(13) NULL,
     type                 VARCHAR(13) NULL,
     [infos counter]      INT NULL,
     [warnings counter]   INT NULL,
     [errors counter]     INT NULL,
     [last contacted]     DATETIME NULL
);

CREATE TABLE pbist_sccm.configmgrserverhealthcountcomponentstatusmessagesbyseverity
(
     totalmessages INT NULL,
     component     NVARCHAR(128) NOT NULL,
     severity      VARCHAR(13) NOT NULL
);

CREATE TABLE pbist_sccm.configmgrserverhealthcountcomponentstatusmessagesbyseverity_staging
(
     totalmessages INT NULL,
     component     NVARCHAR(128) NOT NULL,
     severity      VARCHAR(13) NOT NULL
);

CREATE TABLE pbist_sccm.[configuration]
(
     id                     INT IDENTITY(1, 1) NOT NULL,
     configuration_group    VARCHAR(150) NOT NULL,
     configuration_subgroup VARCHAR(150) NOT NULL,
     [name]                 VARCHAR(150) NOT NULL,
     [value]                VARCHAR(max) NULL,
     visible                BIT NOT NULL DEFAULT ((0))
);

CREATE TABLE pbist_sccm.[date]
(
     date_key               INT NOT NULL,
     full_date              DATE NOT NULL,
     day_of_week            TINYINT NOT NULL,
     day_num_in_month       TINYINT NOT NULL,
     day_name               NVARCHAR(50) NOT NULL,
     day_abbrev             NVARCHAR(10) NOT NULL,
     weekday_flag           CHAR(1) NOT NULL,
     week_num_in_year       TINYINT NOT NULL,
     week_begin_date        DATE NOT NULL,
     week_begin_date_key    INT NOT NULL,
     [month]                TINYINT NOT NULL,
     month_name             NVARCHAR(50) NOT NULL,
     month_abbrev           NVARCHAR(10) NOT NULL,
     [quarter]              TINYINT NOT NULL,
     [year]                 SMALLINT NOT NULL,
     yearmo                 INT NOT NULL,
     fiscal_month           TINYINT NOT NULL,
     fiscal_quarter         TINYINT NOT NULL,
     fiscal_year            SMALLINT NOT NULL,
     last_day_in_month_flag CHAR(1) NOT NULL,
     same_day_year_ago_date DATE NOT NULL,
     same_day_year_ago_key  INT NOT NULL,
     day_num_in_year AS ( Datepart(dayofyear, full_date) ),
     quarter_name AS 'Q' + CAST([quarter] AS NCHAR(1)),
     fiscal_quarter_name AS 'Q' + CAST(fiscal_quarter AS NCHAR(1)),
     fiscalquartercompletename AS ( Concat('FY', Substring(CONVERT(VARCHAR, fiscal_year), ( 3 ), ( 2 )), ' Q', fiscal_quarter) ),
     fiscalyearcompletename AS ( Concat('FY', Substring(CONVERT(VARCHAR, fiscal_year), ( 3 ), ( 2 ))) ),
     fiscalmonthcompletename AS ( Concat(month_abbrev, ' ', Substring(CONVERT(VARCHAR, fiscal_year), ( 3 ), ( 2 ))) )
);

CREATE TABLE pbist_sccm.distributionpointscompliancebydpstatus
(
     distributionpoint NVARCHAR(128) NOT NULL,
     failed            INT NOT NULL,
     inprogress        INT NOT NULL,
     success           INT NOT NULL
);

CREATE TABLE pbist_sccm.distributionpointscompliancebydpstatus_staging
(
     distributionpoint NVARCHAR(128) NOT NULL,
     failed            INT NOT NULL,
     inprogress        INT NOT NULL,
     success           INT NOT NULL
);

CREATE TABLE pbist_sccm.distributionpointscompliancebypackagesstatus
(
     servername    NVARCHAR(256) NULL,
     sitecode      NVARCHAR(3) NOT NULL,
     packageid     NVARCHAR(8) NOT NULL,
     [name]        NVARCHAR(256) NULL,
     installstatus NVARCHAR(255) NOT NULL,
     summarydate   DATETIME NOT NULL
);

CREATE TABLE pbist_sccm.distributionpointscompliancebypackagesstatus_staging
(
     servername    NVARCHAR(256) NULL,
     sitecode      NVARCHAR(3) NOT NULL,
     packageid     NVARCHAR(8) NOT NULL,
     [name]        NVARCHAR(256) NULL,
     installstatus NVARCHAR(255) NOT NULL,
     summarydate   DATETIME NOT NULL
);

CREATE TABLE pbist_sccm.distributionpointsconfigurationstatus
(
     servername        NVARCHAR(255) NULL,
     [dp description]  NVARCHAR(256) NULL,
     sitecode          NVARCHAR(3) NULL,
     [branch dp]       VARCHAR(1) NOT NULL,
     [bits enabled]    VARCHAR(1) NOT NULL,
     allowfallback     VARCHAR(1) NOT NULL,
     pulldp            VARCHAR(1) NOT NULL,
     pxe               VARCHAR(1) NOT NULL,
     multicast         VARCHAR(1) NOT NULL,
     transferrate      VARCHAR(1) NOT NULL,
     prestagingallowed VARCHAR(1) NOT NULL,
     contentvalidation VARCHAR(1) NOT NULL,
     dpgroupcount      INT NOT NULL,
     state             VARCHAR(11) NOT NULL,
     messagecount      INT NULL,
     laststatustime    DATETIME NULL
);

CREATE TABLE pbist_sccm.distributionpointsconfigurationstatus_staging
(
     servername        NVARCHAR(255) NULL,
     [dp description]  NVARCHAR(256) NULL,
     sitecode          NVARCHAR(3) NULL,
     [branch dp]       VARCHAR(1) NOT NULL,
     [bits enabled]    VARCHAR(1) NOT NULL,
     allowfallback     VARCHAR(1) NOT NULL,
     pulldp            VARCHAR(1) NOT NULL,
     pxe               VARCHAR(1) NOT NULL,
     multicast         VARCHAR(1) NOT NULL,
     transferrate      VARCHAR(1) NOT NULL,
     prestagingallowed VARCHAR(1) NOT NULL,
     contentvalidation VARCHAR(1) NOT NULL,
     dpgroupcount      INT NOT NULL,
     state             VARCHAR(11) NOT NULL,
     messagecount      INT NULL,
     laststatustime    DATETIME NULL
);

CREATE TABLE pbist_sccm.distributionpointsstatus
(
     failed        INT NOT NULL,
     [in progress] INT NOT NULL,
     success       INT NOT NULL
);

CREATE TABLE pbist_sccm.distributionpointsstatus_staging
(
     failed        INT NOT NULL,
     [in progress] INT NOT NULL,
     success       INT NOT NULL
);

CREATE TABLE pbist_sccm.malware
(
     threatid           BIGINT NOT NULL,
     [malware name]     NVARCHAR(128) NOT NULL,
     [malware severity] NVARCHAR(128) NOT NULL,
     [malware category] NVARCHAR(128) NOT NULL
);

CREATE TABLE pbist_sccm.malware_staging
(
     threatid           BIGINT NOT NULL,
     [malware name]     NVARCHAR(128) NOT NULL,
     [malware severity] NVARCHAR(128) NOT NULL,
     [malware category] NVARCHAR(128) NOT NULL
);

CREATE TABLE pbist_sccm.operatingsystem
(
     osid                INT IDENTITY(1, 1) NOT NULL,
     [operating  system] NVARCHAR(256) NULL
);

CREATE TABLE pbist_sccm.overallcompliancebaseline
(
     compliancestate     TINYINT NOT NULL,
     compliancestatename NVARCHAR(255) NULL,
     totperstate         INT NULL,
     ptot                NUMERIC(23, 13) NULL,
     tot                 INT NULL
);

CREATE TABLE pbist_sccm.overallcompliancebaseline_staging
(
     compliancestate     TINYINT NOT NULL,
     compliancestatename NVARCHAR(255) NULL,
     totperstate         INT NULL,
     ptot                NUMERIC(23, 13) NULL,
     tot                 INT NULL
);

CREATE TABLE pbist_sccm.program
(
     [program name] NVARCHAR(255) NOT NULL,
     publisher      NVARCHAR(255) NULL,
     version        NVARCHAR(255) NULL
);

CREATE TABLE pbist_sccm.program_staging
(
     [program name] NVARCHAR(255) NOT NULL,
     publisher      NVARCHAR(255) NULL,
     version        NVARCHAR(255) NULL
);

CREATE TABLE pbist_sccm.replicationstatus
(
     overalllinkstatus   VARCHAR(14) NULL,
     [site status count] INT NULL
);

CREATE TABLE pbist_sccm.replicationstatus_staging
(
     overalllinkstatus   VARCHAR(14) NULL,
     [site status count] INT NULL
);

CREATE TABLE pbist_sccm.replicationstatusdetails
(
     parentsitecode                VARCHAR(16) NULL,
     childsitecode                 VARCHAR(16) NULL,
     overalllinkstatus             VARCHAR(16) NULL,
     globalparenttochildlinkstatus VARCHAR(16) NULL,
     globalchildtoparentlinkstatus VARCHAR(16) NULL,
     sitechildtoparentlinkstatus   VARCHAR(16) NULL,
     lastsendtimeparenttochild     DATETIME NULL,
     lastsendtimechildtoparent     DATETIME NULL,
     lastsitesynctime              DATETIME NULL,
     serverrole                    VARCHAR(16) NULL
);

CREATE TABLE pbist_sccm.replicationstatusdetails_staging
(
     parentsitecode                VARCHAR(16) NULL,
     childsitecode                 VARCHAR(16) NULL,
     overalllinkstatus             VARCHAR(16) NULL,
     globalparenttochildlinkstatus VARCHAR(16) NULL,
     globalchildtoparentlinkstatus VARCHAR(16) NULL,
     sitechildtoparentlinkstatus   VARCHAR(16) NULL,
     lastsendtimeparenttochild     DATETIME NULL,
     lastsendtimechildtoparent     DATETIME NULL,
     lastsitesynctime              DATETIME NULL,
     serverrole                    VARCHAR(16) NULL
);

CREATE TABLE pbist_sccm.replicationstatusreplicationgroup
(
     siterequesting      NVARCHAR(3) NOT NULL,
     sitefulfilling      NVARCHAR(3) NOT NULL,
     replicationpattern  NVARCHAR(20) NOT NULL,
     replgroupscompleted INT NULL,
     replgroupspending   INT NULL,
     replgroupsfailed    INT NULL,
     percentcomplete     FLOAT NULL
);

CREATE TABLE pbist_sccm.replicationstatusreplicationgroup_staging
(
     siterequesting      NVARCHAR(3) NOT NULL,
     sitefulfilling      NVARCHAR(3) NOT NULL,
     replicationpattern  NVARCHAR(20) NOT NULL,
     replgroupscompleted INT NULL,
     replgroupspending   INT NULL,
     replgroupsfailed    INT NULL,
     percentcomplete     FLOAT NULL
);

CREATE TABLE pbist_sccm.scanhistory
(
     machineid                         INT NOT NULL,
     date_key                          INT NOT NULL,
     sitecode                          NVARCHAR(3) NULL,
     [client type]                     SMALLINT NULL,
     enabled                           INT NULL,
     [client version]                  NVARCHAR(255) NULL,
     [real time protection enabled]    BIT NULL,
     [on access protection enabled]    BIT NULL,
     [input/output protection enabled] BIT NULL,
     [behavior monitor enabled]        BIT NULL,
     [antivirus enabled]               BIT NULL,
     [antispyware enabled]             BIT NULL,
     [nis enabled]                     BIT NULL,
     [quick scan age (days)]           INT NULL,
     [full scan age (days)]            INT NULL,
     [signature age (days)]            INT NULL,
     [engine version]                  NVARCHAR(255) NULL,
     [antivirus signature version]     NVARCHAR(255) NULL,
     [missing important update count]  INT NULL,
     [missing critical update count]   INT NULL,
     [client active status]            BIT NOT NULL,
     [health evaluation result]        SMALLINT NULL,
     [health evaluation]               DATETIME NULL,
     [last online]                     DATETIME NULL,
     [health status message]           DATETIME NULL,
     [client state]                    SMALLINT NULL,
     [last ddr]                        DATETIME NULL,
     [last hw]                         DATETIME NULL,
     [last sw]                         DATETIME NULL,
     [last status message]             DATETIME NULL,
     [last policy request]             DATETIME NULL,
     [last scan time]                  DATETIME NULL
);

CREATE TABLE pbist_sccm.scanhistory_staging
(
     machineid                         INT NOT NULL,
     date_key                          INT NOT NULL,
     sitecode                          NVARCHAR(3) NULL,
     [client type]                     SMALLINT NULL,
     enabled                           INT NULL,
     [client version]                  NVARCHAR(255) NULL,
     [real time protection enabled]    BIT NULL,
     [on access protection enabled]    BIT NULL,
     [input/output protection enabled] BIT NULL,
     [behavior monitor enabled]        BIT NULL,
     [antivirus enabled]               BIT NULL,
     [antispyware enabled]             BIT NULL,
     [nis enabled]                     BIT NULL,
     [quick scan age (days)]           INT NULL,
     [full scan age (days)]            INT NULL,
     [signature age (days)]            INT NULL,
     [engine version]                  NVARCHAR(255) NULL,
     [antivirus signature version]     NVARCHAR(255) NULL,
     [missing important update count]  INT NULL,
     [missing critical update count]   INT NULL,
     [client active status]            BIT NOT NULL,
     [health evaluation result]        SMALLINT NULL,
     [health evaluation]               DATETIME NULL,
     [last online]                     DATETIME NULL,
     [health status message]           DATETIME NULL,
     [client state]                    SMALLINT NULL,
     [last ddr]                        DATETIME NULL,
     [last hw]                         DATETIME NULL,
     [last sw]                         DATETIME NULL,
     [last status message]             DATETIME NULL,
     [last policy request]             DATETIME NULL,
     [last scan time]                  DATETIME NULL
);

CREATE TABLE pbist_sccm.site
(
     sitecode                                    NVARCHAR(3) NOT NULL,
     [site name]                                 NVARCHAR(200) NULL,
     version                                     NVARCHAR(32) NULL,
     [server name]                               NVARCHAR(256) NULL,
     availability                                VARCHAR(9) NOT NULL,
     location                                    NVARCHAR(256) NULL,
     overalllinkstatus                           NVARCHAR(12) NULL,
     [client successfully communicating with mp] INT NULL,
     [client failing to communicate with mp]     INT NULL,
     [health check count]                        INT NULL,
     [ok health check count]                     INT NULL,
     [av definition compliance count]            INT NULL,
     [scep count]                                INT NULL,
     [av reach count]                            INT NULL
);

CREATE TABLE pbist_sccm.site_staging
(
     sitecode                                    NVARCHAR(3) NOT NULL,
     [site name]                                 NVARCHAR(200) NULL,
     version                                     NVARCHAR(32) NULL,
     [server name]                               NVARCHAR(256) NULL,
     availability                                NVARCHAR(20) NOT NULL,
     location                                    NVARCHAR(256) NULL,
     overalllinkstatus                           NVARCHAR(12) NULL,
     [client successfully communicating with mp] INT NULL,
     [client failing to communicate with mp]     INT NULL,
     [health check count]                        INT NULL,
     [ok health check count]                     INT NULL,
     [av definition compliance count]            INT NULL,
     [scep count]                                INT NULL,
     [av reach count]                            INT NULL
);

CREATE TABLE pbist_sccm.softwaredistribution
(
     success    INT NULL,
     inprogress INT NULL,
     unknown    INT NULL,
     error      INT NULL,
     other      INT NULL
);

CREATE TABLE pbist_sccm.softwaredistribution_staging
(
     success    INT NULL,
     inprogress INT NULL,
     unknown    INT NULL,
     error      INT NULL,
     other      INT NULL
);

CREATE TABLE pbist_sccm.softwaredistributionoverallapplicationdeployment
(
     collectionid     NVARCHAR(8) NULL,
     collectionname   NVARCHAR(255) NULL,
     offerid          NVARCHAR(8) NOT NULL,
     softwarename     NVARCHAR(359) NULL,
     application      NVARCHAR(359) NULL,
     numbersuccess    INT NULL,
     numberinprogress INT NULL,
     numberunknown    INT NULL,
     numbererrors     INT NULL,
     numberother      INT NULL,
     numbertotal      INT NULL,
     packageid        NVARCHAR(8) NULL
);

CREATE TABLE pbist_sccm.softwaredistributionoverallapplicationdeployment_staging
(
     collectionid     NVARCHAR(8) NULL,
     collectionname   NVARCHAR(255) NULL,
     offerid          NVARCHAR(8) NOT NULL,
     softwarename     NVARCHAR(359) NULL,
     application      NVARCHAR(359) NULL,
     numbersuccess    INT NULL,
     numberinprogress INT NULL,
     numberunknown    INT NULL,
     numbererrors     INT NULL,
     numberother      INT NULL,
     numbertotal      INT NULL,
     packageid        NVARCHAR(8) NULL
);

CREATE TABLE pbist_sccm.softwaredistributionoverallpackagedeployment
(
     collectionid     NVARCHAR(8) NULL,
     collectionname   NVARCHAR(255) NULL,
     offerid          NVARCHAR(8) NOT NULL,
     softwarename     NVARCHAR(359) NULL,
     programname      NVARCHAR(100) NULL,
     numbersuccess    INT NULL,
     numberinprogress INT NULL,
     numberunknown    INT NULL,
     numbererrors     INT NULL,
     numberother      INT NULL,
     numbertotal      INT NULL,
     packageid        NVARCHAR(8) NULL
);

CREATE TABLE pbist_sccm.softwaredistributionoverallpackagedeployment_staging
(
     collectionid     NVARCHAR(8) NULL,
     collectionname   NVARCHAR(255) NULL,
     offerid          NVARCHAR(8) NOT NULL,
     softwarename     NVARCHAR(359) NULL,
     programname      NVARCHAR(100) NULL,
     numbersuccess    INT NULL,
     numberinprogress INT NULL,
     numberunknown    INT NULL,
     numbererrors     INT NULL,
     numberother      INT NULL,
     numbertotal      INT NULL,
     packageid        NVARCHAR(8) NULL
);

CREATE TABLE pbist_sccm.softwaredistributionoverallpckappdpdistribution
(
     dpserver      NVARCHAR(255) NULL,
     packageid     NVARCHAR(8) NOT NULL,
     NAME          NVARCHAR(256) NULL,
     sitecode      NVARCHAR(3) NULL,
     installstatus NVARCHAR(255) NOT NULL
);

CREATE TABLE pbist_sccm.softwaredistributionoverallpckappdpdistribution_staging
(
     dpserver      NVARCHAR(255) NULL,
     packageid     NVARCHAR(8) NOT NULL,
     NAME          NVARCHAR(256) NULL,
     sitecode      NVARCHAR(3) NULL,
     installstatus NVARCHAR(255) NOT NULL
);

CREATE TABLE pbist_sccm.tasksequencedeployment
(
     tasksequencename NVARCHAR(256) NULL,
     success          FLOAT NULL,
     running          FLOAT NULL,
     failed           FLOAT NULL,
     unknown          FLOAT NULL
);

CREATE TABLE pbist_sccm.tasksequencedeployment_staging
(
     tasksequencename NVARCHAR(256) NULL,
     success          FLOAT NULL,
     running          FLOAT NULL,
     failed           FLOAT NULL,
     unknown          FLOAT NULL
);

CREATE TABLE pbist_sccm.tasksequencedpcontentstatusreferencedpackagestasksequence
(
     sitecode       NVARCHAR(3) NULL,
     servername     NVARCHAR(255) NULL,
     packageid      NVARCHAR(8) NULL,
     packagename    NVARCHAR(256) NULL,
     version        NVARCHAR(64) NULL,
     status         VARCHAR(12) NULL,
     summarydate    DATETIME NULL,
     tasksequenceid NVARCHAR(16) NULL
);

CREATE TABLE pbist_sccm.tasksequencedpcontentstatusreferencedpackagestasksequence_staging
(
     sitecode       NVARCHAR(3) NULL,
     servername     NVARCHAR(255) NULL,
     packageid      NVARCHAR(8) NULL,
     packagename    NVARCHAR(256) NULL,
     version        NVARCHAR(64) NULL,
     status         VARCHAR(12) NULL,
     summarydate    DATETIME NULL,
     tasksequenceid NVARCHAR(16) NULL
);

CREATE TABLE pbist_sccm.tasksequenceoveralltasksequencebycomputerdeployment
(
     netbios_name0   NVARCHAR(256) NULL,
     NAME            NVARCHAR(256) NULL,
     resourceid      INT NOT NULL,
     tasksequenceid  NVARCHAR(8) NULL,
     advertisementid NVARCHAR(8) NOT NULL,
     date            DATETIME NULL,
     state           VARCHAR(10) NULL
);

CREATE TABLE pbist_sccm.tasksequenceoveralltasksequencebycomputerdeployment_staging
(
     netbios_name0   NVARCHAR(256) NULL,
     NAME            NVARCHAR(256) NULL,
     resourceid      INT NOT NULL,
     tasksequenceid  NVARCHAR(8) NULL,
     advertisementid NVARCHAR(8) NOT NULL,
     date            DATETIME NULL,
     state           VARCHAR(10) NULL
);

CREATE TABLE pbist_sccm.tasksequenceoveralltasksequencebytasksequencedeployment
(
     tasksequenceid       NVARCHAR(8) NULL,
     tasksequencename     NVARCHAR(256) NULL,
     date                 VARCHAR(30) NULL,
     total                INT NULL,
     running              INT NULL,
     successful           INT NULL,
     failed               INT NULL,
     tsid                 NVARCHAR(8) NULL,
     fullydistributed     INT NULL,
     partiallydistributed INT NULL,
     notassignedpackage   INT NULL,
     totalpkg             INT NULL
);

CREATE TABLE pbist_sccm.tasksequenceoveralltasksequencebytasksequencedeployment_staging
(
     tasksequenceid       NVARCHAR(8) NULL,
     tasksequencename     NVARCHAR(256) NULL,
     date                 VARCHAR(30) NULL,
     total                INT NULL,
     running              INT NULL,
     successful           INT NULL,
     failed               INT NULL,
     tsid                 NVARCHAR(8) NULL,
     fullydistributed     INT NULL,
     partiallydistributed INT NULL,
     notassignedpackage   INT NULL,
     totalpkg             INT NULL
);

CREATE TABLE pbist_sccm.[update]
(
     ci_id        INT NOT NULL,
     articleid    NVARCHAR(64) NULL,
     bulletinid   NVARCHAR(64) NULL,
     title        NVARCHAR(513) NULL,
     severity     INT NULL,
     severityname NVARCHAR(64) NULL,
     infourl      NVARCHAR(512) NULL
);

CREATE TABLE pbist_sccm.update_staging
(
     ci_id           INT NOT NULL,
     articleid       NVARCHAR(64) NULL,
     bulletinid      NVARCHAR(64) NULL,
     title           NVARCHAR(513) NULL,
     severity        INT NULL,
     [severity name] NVARCHAR(64) NULL,
     infourl         NVARCHAR(512) NULL
);

CREATE TABLE pbist_sccm.[user]
(
     username    NVARCHAR(256) NULL,
     [full name] NVARCHAR(255) NULL
);

CREATE TABLE pbist_sccm.user_staging
(
     username    NVARCHAR(256) NULL,
     [full name] NVARCHAR(255) NULL
);

CREATE TABLE pbist_sccm.usercomputer
(
     machineid   INT NOT NULL,
     username    NVARCHAR(256) NULL,
     [full name] NVARCHAR(255) NULL
);

CREATE TABLE pbist_sccm.usercomputer_staging
(
     machineid   INT NOT NULL,
     username    NVARCHAR(256) NULL,
     [full name] NVARCHAR(255) NULL
);

CREATE TABLE  pbist_sccm.SCEPDefinition (
	machinedid INT NULL,
	[name] NVARCHAR(256) null,
	signatureupto1dayold INT NULL,
	signatureupto3daysold INT NULL,
	signatureupto7daysold INT NULL,
	signatureolderthan7days INT NULL,
	nosignature INT NULL,
	epinstalled INT NULL
);

CREATE TABLE  pbist_sccm.SCEPDefinition_staging (
	machinedid INT NULL,
	[name] NVARCHAR(256) null,
	signatureupto1dayold INT NULL,
	signatureupto3daysold INT NULL,
	signatureupto7daysold INT NULL,
	signatureolderthan7days INT NULL,
	nosignature INT NULL,
	epinstalled INT NULL
);
