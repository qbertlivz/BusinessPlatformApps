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
  SELECT id             AS [Account Id],
         NAME           AS [Account Name],
         ownerid        AS [Owner Id],
         NULL           AS [Territory Id],
         industry       AS [Industry],
         NULL           AS [Business Unit Id],
         billingcity    AS [City],
         billingstate   AS [State],
         billingcountry AS [Country]
  FROM   dbo.account
  UNION ALL
  -- Creates a dummy account for opportunities that don't have an account.
  SELECT id      AS [Account Id],
         NULL    AS [Account Name],
         ownerid AS [Owner Id],
         NULL    AS [Territory Id],
         NULL    AS [Industry],
         NULL    AS [Business Unit Id],
         NULL    AS [City],
         NULL    AS [State],
         NULL    AS [Country]
  FROM   dbo.opportunity
  WHERE  accountid IS NULL;
go


-- BusinessUnitView
CREATE VIEW smgt.businessunitview
AS
  WITH tree
       AS (SELECT [id],
                  NAME,
                  parentroleid,
                  0                        AS [Level],
                  Cast(id AS VARCHAR(max)) AS pth
           FROM   dbo.userrole
           WHERE  parentroleid = ''
           UNION ALL
           SELECT a.[id],
                  a.NAME,
                  a.parentroleid,
                  t.[level] + 1                      As [Level],
                  t.pth + Cast(a.id AS VARCHAR(max)) As pth
           FROM   tree AS t INNER JOIN dbo.userrole AS a ON a.parentroleid = t.id
          )
  SELECT t.id   AS [Business Unit Id],
         t.NAME AS [Business Unit Name],
         [level],
         b.NAME AS [Level1],
         c.NAME AS [Level2],
         d.NAME AS [Level3]
  FROM   tree t LEFT OUTER JOIN dbo.userrole b ON Substring(pth, 1, 18) = b.id
                LEFT OUTER JOIN dbo.userrole c ON Substring(pth, 19, 18) = c.id
                LEFT OUTER JOIN dbo.userrole d ON Substring(pth, 37, 18) = d.id;
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
    SELECT NULL          AS [Estimated Amount],
           [status]      AS [Status],
           NULL          AS [Lead Quality],
           NULL          AS [Subject],
           title         AS [Job Title],
           id            AS [Lead Id],
           NULL          AS [Estimated Amount Base],
           ownerid       AS [Owner Id],
           case when [status] like '%Not Converted%' then 'Disqualified' when [status] like '%Converted%' then 'Qualified' else 'Open' end
		             AS [State],
           NULL          AS [Campaign Id],
           NULL          AS [Estimated Close Date],
           leadsource    AS [Lead Source Name],
           industry      AS [Industry Name],
           NULL          AS [Purchase Time Frame],
           Convert(date, createddate)   AS [Created On],
           company       AS [Company Name],
           lastname      AS [Last Name],
           firstname     AS [First Name],
           email         AS [Email],
		   city          AS [City],
		   country       as [Country]
    FROM   dbo.lead;
go


-- MeasuresView
CREATE VIEW smgt.measuresview
AS
    SELECT TOP 0 1 AS MeasureValues;
go


-- OpportunityProductView
CREATE VIEW smgt.opportunityproductview
AS
    SELECT product2id    AS [Product Id],
           opportunityid AS [Opportunity Id],
           totalprice    AS [Revenue]
    FROM   dbo.opportunitylineitem;
go


-- OpportunityView
CREATE VIEW smgt.opportunityview
AS
    SELECT o.id                    AS [Opportunity Id],
            o.NAME                 AS [Opportunity Name],
            o.ownerid              AS [Owner Id],
	    CONVERT(DATE, o.createddate)
	                           AS [Created Date],
            CASE
                WHEN o.isclosed = 0 THEN NULL
                ELSE CONVERT(DATE, o.closedate)
            END                    AS [Actual Close Date],
            CASE
                WHEN o.isclosed = 1 THEN NULL
                ELSE CONVERT(DATE, o.closedate)
            END                    AS [Estimated Close Date],
            o.probability          AS [Close Probability],
            CASE
                WHEN o.accountid IS NULL THEN o.id
                ELSE o.accountid
            END                    AS [Account Id],
            CASE
                WHEN o.isclosed = 0 THEN NULL
                ELSE o.amount
            END                    AS [Actual Value],
            o.amount               AS [Estimated Value],
            o.expectedrevenue      AS [Expected Value],
            o.forecastcategoryname AS [Status],
            o.stagename            AS [Sales Stage],
            s.sortorder            AS [Sales Stage Code],
            CASE
                WHEN o.isclosed = 1 AND o.iswon = 1 THEN 'Won'
                ELSE
                    CASE
                        WHEN o.isclosed = 1 AND o.iswon = 0 THEN 'Lost'
                        ELSE 'Open'
                    END
            END                    AS [State],
            NULL                   AS [Lead Id],
            o.probability          AS [Opportunity Rating Name],
            o.leadsource           AS [Source]
    FROM   dbo.opportunity o LEFT OUTER JOIN dbo.opportunitystage s ON o.stagename=s.masterlabel;
go

-- ProductView
CREATE VIEW smgt.productview
AS
    SELECT id      AS [Product Id],
            NAME   AS [Product Name],
            1      AS [level],
            family AS [Product Level 1],
            NAME   AS [Product Level 2],
            NULL   AS [Product Level 3]
    FROM   dbo.product2
    WHERE  isactive=1;
go


-- TerritoryView
CREATE VIEW smgt.territoryview
AS
  SELECT  NULL AS [Territory Name], NULL AS [territory Id];
go

-- UserAscendantsView
CREATE VIEW smgt.userascendantsview
AS
    WITH mycte(roleid, ascendantroleid, rolelevel) AS
    (
        -- Anchor
        SELECT r.id            AS roleid,
                r.parentroleid AS ascendantroleid,
                0              AS RoleLevel
        FROM   userrole r
        UNION ALL
        -- ...and the recursive part
        SELECT c.roleid          AS roleid,
               ur.parentroleid  AS ascendantroleid,
               c.rolelevel + 1  AS RoleLevel
        FROM   mycte c INNER JOIN userrole ur ON c.ascendantroleid = ur.id
        WHERE  ur.[parentroleid] IS NOT NULL
    )
    SELECT  c.roleid,
            c.ascendantroleid,
            ur.NAME       AS rolename,
            ur2.NAME      AS ascendantrolename,
            u.[id]        AS [User Id],
            u.email       AS [Email],
            u2.[id]       AS [Ascendant User Id],
            u2.email      AS [Ascendant Email],
            0             AS [Employee Level],
            um.domainuser AS [Ascendant Domain User]
  FROM   mycte c INNER JOIN [user] u ON c.roleid = u.userroleid
                 INNER JOIN [user] u2 ON c.ascendantroleid = u2.userroleid
                 LEFT OUTER JOIN userrole ur ON c.roleid = ur.id
                 LEFT OUTER JOIN userrole ur2 ON c.ascendantroleid = ur2.id
                 LEFT OUTER JOIN smgt.usermapping um ON u2.[id] = um.userid
  WHERE  u.email IS NOT NULL AND
         u2.email IS NOT NULL AND
         u.isactive = 1 AND
         u2.isactive = 1
  UNION ALL
  -- ...Add users to give the mpermissions to themselves
  SELECT u.userroleid  AS roleid,
         u.userroleid  AS ascendantroleid,
         u.id          AS [User Id],
         ur.NAME       AS rolename,
         ur.NAME       AS ascendantrolename,
         u.email       AS [Email],
         u.id          AS [Ascendant User Id],
         u.email       AS [Ascendant Email],
         0             AS [Employee Level],
         um.domainuser AS [Ascendant Domain User]
  FROM   dbo.[user] u LEFT OUTER JOIN userrole ur ON ur.id = u.userroleid
                      LEFT OUTER JOIN smgt.usermapping um ON u.id = um.userid
  WHERE  u.email IS NOT NULL AND u.isactive=1;
go


-- UserView
CREATE VIEW smgt.userview
AS
    SELECT a.NAME      AS [Full Name],
           a.id        AS [User Id],
           a.managerid AS [Parent User Id],
           0           AS [hierarchy level],
           b.NAME      AS [Manager Name]
    FROM   dbo.[user] a INNER JOIN dbo.[user] b ON a.managerid = b.id
    WHERE  a.managerid IN (SELECT id
                           FROM   dbo.[user]
                           WHERE  isactive = 1)
    UNION ALL
    SELECT b.NAME               AS [Full Name],
           b.id                 AS [User Id],
           '000000000000000000' AS [Parent User Id],
           1                    AS [hierarchy level],
           'Root'               AS [Manager Name]
    FROM   (SELECT NAME,
                   id
            FROM   dbo.[user]
            WHERE  isactive = 1 AND 
                   ( managerid = '' OR
                     managerid IS NULL OR
                     managerid NOT IN (SELECT id FROM dbo.[user] WHERE isactive=1)
                   )
           ) AS b
    UNION ALL
    SELECT 'Root'               AS [Full Name],
           '000000000000000000' AS [User Id],
           '000000000000000000' AS [Parent User Id],
           1                    AS [hierarchy level],
           'Root'               AS [Manager Name];
go
