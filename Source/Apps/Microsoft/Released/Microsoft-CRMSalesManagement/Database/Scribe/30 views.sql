SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

-- AccountView
CREATE VIEW smgt.accountview
AS
  SELECT accountid                AS [Account Id],
         [name]                   AS [Account Name],
         ownerid                  AS [Owner Id],
         territoryid              AS [Territory Id],
         industrycode_displayname AS [Industry],
         owningbusinessunit       AS [Business Unit Id],
         address1_city            AS [City],
         address1_stateorprovince AS [State],
         address1_country         AS [Country],
         revenue                  AS [Annual Revenue]
  FROM   dbo.account
  WHERE  ( SCRIBE_DELETEDON IS NULL )
  UNION ALL
  SELECT opportunityid      AS [Account Id],
         NULL               AS [Account Name],
         ownerid            AS [Owner Id],
         NULL               AS [Territory Id],
         NULL               AS [Industry],
         owningbusinessunit AS [Business Unit Id],
         NULL               AS [City],
         NULL               AS [State],
         NULL               AS [Country],
         NULL               AS [Annual Revenue]
  FROM   dbo.opportunity
  WHERE  ( parentaccountid IS NULL AND SCRIBE_DELETEDON IS NULL );
go


-- BusinessUnitView
CREATE VIEW smgt.businessunitview
AS
    WITH tree AS
    (
        SELECT parentbusinessunitid,
                parentbusinessunitidname,
                businessunitid,
                name,
                0                                    AS [level],
                Cast(businessunitid AS VARCHAR(max)) AS pth
        FROM   dbo.businessunit
        WHERE  parentbusinessunitid IS NULL AND ( SCRIBE_DELETEDON IS NULL )
        UNION ALL
        SELECT a.parentbusinessunitid,
                a.parentbusinessunitidname,
                a.businessunitid,
                a.name,
                t.[level] + 1                                  AS [level],
                t.pth + Cast(a.businessunitid AS VARCHAR(max)) AS pth
        FROM   tree AS t INNER JOIN dbo.businessunit AS a ON a.parentbusinessunitid = t.businessunitid
        WHERE ( a.SCRIBE_DELETEDON IS NULL )
    )
    SELECT hierarchy.businessunitid AS [Business Unit Id],
            hierarchy.name           AS [Business Unit Name],
            [level],
            CONVERT(VARCHAR, b.name) AS Level1,
            CONVERT(VARCHAR, c.name) AS Level2,
            CONVERT(VARCHAR, d.name) AS Level3
    FROM   (SELECT businessunitid,
                    name,
                    [level],
                    CONVERT(UNIQUEIDENTIFIER, NULLIF(Substring(pth, 1, 36), ''))  AS level1,
                    CONVERT(UNIQUEIDENTIFIER, NULLIF(Substring(pth, 37, 36), '')) AS level2,
                    CONVERT(UNIQUEIDENTIFIER, NULLIF(Substring(pth, 73, 36), '')) AS level3
            FROM tree) AS hierarchy LEFT JOIN businessunit AS b ON b.businessunitid = level1
                                    LEFT JOIN businessunit AS c ON c.businessunitid = level2
                                    LEFT JOIN businessunit AS d ON d.businessunitid = level3;
go


-- ConfigurationView
CREATE VIEW smgt.configurationview
AS
    SELECT [id],
            configuration_group    AS [Configuration Group],
            configuration_subgroup AS [Configuration Subgroup],
            [name]                 AS [Name],
            [value]                AS [Value]
    FROM   smgt.[configuration]
    WHERE  visible = 1;
go

-- DateView
CREATE VIEW smgt.dateview
AS
    SELECT date_key,
           full_date        AS [Date],
           day_of_week      AS [Day of the Week],
           day_num_in_month AS [Day Number of the Month],
           day_name         AS [Day Name],
           day_abbrev       AS [Day Abbreviated],
           weekday_flag     AS [Weekday Flag],
           [month],
           month_name       AS [Month Name],
           month_abbrev,
           [quarter],
           [year],
           same_day_year_ago_date,
           week_begin_date  AS [Week Begin Date]
    FROM   smgt.[date];
go

-- LeadView
CREATE VIEW smgt.leadview
AS
    SELECT estimatedamount               AS [Estimated Amount],
           statuscode_displayname        AS [Status],
           leadqualitycode_displayname   AS [Lead Quality],
           [subject]                     AS [Subject],
           jobtitle                      AS [Job Title],
           leadid                        AS [Lead Id],
           estimatedamount_base          AS [Estimated Amount Base],
           ownerid                       AS [Owner Id],
           statecode_displayname         AS [State],
           campaignid                    AS [Campaign Id],
           estimatedclosedate            AS [Estimated Close Date],
           leadsourcecode_displayname    AS [Lead Source Name],
           industrycode_displayname      AS [Industry Name],
           purchasetimeframe_displayname AS [Purchase Time Frame],
           createdon                     AS [Created On],
           companyname                   AS [Company Name],
           lastname                      AS [Last Name],
           firstname                     AS [First Name],
           emailaddress1                 AS [Email],
           address1_city                 AS [City],
           address1_country              AS [Country]
  FROM     dbo.lead
  WHERE ( SCRIBE_DELETEDON IS NULL );
go

-- TeamView
CREATE VIEW smgt.teamview
AS
  SELECT id      AS [Team Id],
         NAME    AS [Team Name]
  FROM   dbo.team
  WHERE SCRIBE_DELETEDON IS NULL;
go


-- OwnerView
CREATE VIEW smgt.ownerview
AS
  SELECT id         AS [Owner Id],
         [Name]	    AS [Owner Name],
         'Team'	    AS [Owner Type]
  FROM dbo.team WHERE SCRIBE_DELETEDON IS NULL
  UNION
  SELECT systemuserid AS [Owner Id],
         fullname     AS [Owner Name],
         'User'	      AS [Owner Type]
  FROM dbo.systemuser WHERE SCRIBE_DELETEDON IS NULL;;
go

-- MeasuresView
CREATE VIEW smgt.measuresview
AS
    SELECT TOP 0 1 AS MeasureValues;
go


-- OpportunityProductView
CREATE VIEW smgt.opportunityproductview
AS
    SELECT CONVERT(UNIQUEIDENTIFIER, productid) AS [Product Id],
            opportunityid                        AS [Opportunity Id],
            baseamount_base                      AS [Revenue]
    FROM   dbo.opportunityproduct
    WHERE  ( SCRIBE_DELETEDON IS NULL );
go


-- OpportunityView
CREATE VIEW smgt.opportunityview
AS
    SELECT  o.opportunityid                     AS [Opportunity Id],
            o.name                              AS [Opportunity Name],
            o.ownerid                           AS [Owner Id],
            CONVERT(DATE, o.createdon)          AS [Created Date],
            CONVERT(DATE, o.actualclosedate)    AS [Actual Close Date],
            CONVERT(DATE, o.estimatedclosedate) AS [Estimated Close Date],
            o.closeprobability                  AS [Close Probability],
            CASE
                WHEN o.parentaccountid IS NULL THEN o.opportunityid
                ELSE o.parentaccountid
            END                                 AS [Account Id],
            o.actualvalue                       AS [Actual Value],
            o.estimatedvalue                    AS [Estimated Value],
            o.estimatedvalue * o.closeprobability/100.0
                                            AS [Expected Value],
            o.statuscode_displayname            AS [Status],
            CASE
                WHEN stepname IS NULL OR Charindex('-', o.stepname) = 0 THEN NULL
                ELSE LEFT(o.stepname, Charindex('-', o.stepname) - 1)
            END                                 AS [Sales Stage Code],
            o.stepname                          AS [Sales Stage],
            o.statecode_displayname             AS [State],
            o.originatingleadid                 AS [Lead Id],
            o.opportunityratingcode_displayname AS [Opportunity Rating Name],
            NULL                                AS [Source]
    FROM   dbo.opportunity o
    WHERE  ( o.SCRIBE_DELETEDON IS NULL );
go

-- ProductView
CREATE VIEW smgt.productview
AS
    WITH tree AS
    (
        SELECT parentproductid,
               parentproductidname,
               productid,
               [name],
               0                               AS [level],
               Cast(productid AS VARCHAR(max)) AS pth
        FROM   dbo.product
        WHERE  parentproductid IS NULL AND SCRIBE_DELETEDON IS NULL
        UNION ALL
        SELECT a.parentproductid,
                a.parentproductidname,
                a.productid,
                a.name,
                t.[level] + 1                             AS [Level],
                t.pth + Cast(a.productid AS VARCHAR(max)) AS pth
        FROM   tree AS t INNER JOIN dbo.product AS a ON a.parentproductid = t.productid AND a.SCRIBE_DELETEDON IS NULL AND a.parentproductid is NOT NULL
    )
    SELECT hierarchy.productid     AS [Product Id],
           hierarchy.name          AS [Product Name],
           [level],
           CONVERT(VARCHAR, b.name) AS [Product Level 1],
           CONVERT(VARCHAR, c.name) AS [Product Level 2],
           CONVERT(VARCHAR, d.name) AS [Product Level 3]
    FROM   (SELECT productid,
                   name,
                   [level],
                   CONVERT(UNIQUEIDENTIFIER, NULLIF(Substring(pth, 1, 36), ''))  AS level1,
                   CONVERT(UNIQUEIDENTIFIER, NULLIF(Substring(pth, 37, 36), '')) AS level2,
                   CONVERT(UNIQUEIDENTIFIER, NULLIF(Substring(pth, 73, 36), '')) AS level3
            FROM   tree) AS hierarchy LEFT OUTER JOIN dbo.product AS b ON b.productid = level1 AND b.SCRIBE_DELETEDON IS NULL
                                      LEFT OUTER JOIN dbo.product AS c ON c.productid = level2 AND c.SCRIBE_DELETEDON IS NULL
                                      LEFT OUTER JOIN dbo.product AS d ON d.productid = level3 AND d.SCRIBE_DELETEDON IS NULL;
go

-- TempUserView
CREATE VIEW smgt.tempuserview
AS
    SELECT a.fullname,
            CONVERT(UNIQUEIDENTIFIER, a.systemuserid)       AS systemuserid,
            CONVERT(UNIQUEIDENTIFIER, a.parentsystemuserid) AS parentsystemuserid,
            a.hierarchylevel,
            systemuser_1.fullname                           AS managername
    FROM   (
                SELECT dbo.systemuser.fullname,
                    dbo.systemuser.systemuserid,
                    dbo.systemusermanagermap.parentsystemuserid,
                    dbo.systemusermanagermap.hierarchylevel
                FROM   dbo.systemusermanagermap LEFT OUTER JOIN dbo.systemuser ON dbo.systemusermanagermap.systemuserid = dbo.systemuser.systemuserid
                WHERE  systemusermanagermap.SCRIBE_DELETEDON IS NULL
            ) AS a
            LEFT OUTER JOIN dbo.systemuser AS systemuser_1 ON a.parentsystemuserid = systemuser_1.systemuserid
    WHERE  a.hierarchylevel = 1 AND
            systemuser_1.isdisabled = 0 AND
            systemuser_1.SCRIBE_DELETEDON IS NULL;
go

-- TerritoryView
CREATE VIEW smgt.territoryview
AS
    SELECT [name]      AS [Territory Name],
           territoryid AS [Territory Id]
    FROM   dbo.territory
    WHERE  SCRIBE_DELETEDON IS NULL;
go

-- UserAscendantsView
CREATE VIEW smgt.userascendantsview
AS
    WITH mycte(systemuserid, emailaddress, ascendantsystemuserid, ascendantemailaddress, employeelevel) AS
    (
            -- Anchor
            SELECT  u.systemuserid,
                    u.internalemailaddress AS emailaddress,
                    u.systemuserid         AS ascendantsystemuserid,
                    u.internalemailaddress AS ascendantemailaddress,
                    0                      AS employeelevel
            FROM   dbo.systemuser u
            WHERE  u.internalemailaddress IS NOT NULL AND u.SCRIBE_DELETEDON IS NULL AND u.isdisabled = 0
            UNION ALL
            -- ...and the recursive part
            SELECT  c.systemuserid,
                    c.emailaddress,
                    u.parentsystemuserid                           AS ascendantsystemuserid,
                    (SELECT u1.internalemailaddress
                     FROM   systemuser u1
                     WHERE  u.parentsystemuserid = u1.systemuserid) AS ascendantemailaddress,
                    c.employeelevel + 1                             AS employeelevel
            FROM   mycte c INNER JOIN dbo.systemuser u ON c.ascendantsystemuserid = u.systemuserid
            WHERE  u.parentsystemuserid IS NOT NULL AND u.SCRIBE_DELETEDON IS NULL AND u.isdisabled=0
    )
    SELECT  mycte.systemuserid          [User Id],
            mycte.emailaddress          [Email],
            mycte.ascendantsystemuserid [Ascendant User Id],
            mycte.ascendantemailaddress [Ascendant Email],
            mycte.employeelevel         [Employee Level],
            um.domainuser               [Ascendant Domain User]
    FROM   mycte LEFT OUTER JOIN smgt.usermapping um ON mycte.ascendantsystemuserid = um.userid;
go


-- UserView
CREATE VIEW smgt.userview
AS
    SELECT  fullname           AS [Full Name],
            systemuserid       AS [User Id],
            parentsystemuserid AS [Parent User Id],
            hierarchylevel     AS [Hierarchy Level],
            managername        AS [Manager Name]
    FROM   smgt.tempuserview
    UNION ALL
    SELECT  b.fullname                             AS [Full Name],
            b.systemuserid                         AS [User Id],
            '00000000-0000-0000-0000-000000000000' AS [Parent User Id],
            1                                      AS [Hierarchy Level],
            'Root'                                 AS [Manager Name]
    FROM   (
               SELECT DISTINCT fullname, systemuserid
               FROM   dbo.systemuser
               WHERE  isdisabled = 0 AND
                      SCRIBE_DELETEDON IS NULL AND
                      systemuserid NOT IN (SELECT DISTINCT systemuserid FROM smgt.tempuserview)
           ) AS b
    UNION ALL
    SELECT  'Root'                                 AS [Full Name],
            '00000000-0000-0000-0000-000000000000' AS [User Id],
            '00000000-0000-0000-0000-000000000000' AS [Parent User Id],
            1                                      AS [Hierarchy Level],
            'Root'                                 AS [Manager Name];
go
