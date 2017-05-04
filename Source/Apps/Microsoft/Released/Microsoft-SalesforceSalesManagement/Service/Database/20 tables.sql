SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

/* SMGT specific schemas */

CREATE TABLE smgt.configuration
(
  id                     INT IDENTITY(1, 1) NOT NULL,
  configuration_group    VARCHAR(150) NOT NULL,
  configuration_subgroup VARCHAR(150) NOT NULL,
  name                   VARCHAR(150) NOT NULL,
  value                  VARCHAR(max) NULL,
  visible                BIT NOT NULL DEFAULT 0
);

CREATE TABLE smgt.[date]
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
   [month]                TINYINT NOT NULL,
   month_name             NVARCHAR(50) NOT NULL,
   month_abbrev           NVARCHAR(10) NOT NULL,
   [quarter]              TINYINT NOT NULL,
   [year]                 SMALLINT NOT NULL,
   yearmo                 INT NOT NULL,
   same_day_year_ago_date DATE NOT NULL
   CONSTRAINT pk_dim_date PRIMARY KEY CLUSTERED (date_key)
);



CREATE TABLE smgt.usermapping
(
    userid     VARCHAR(50) NULL,
    domainuser VARCHAR(50) NULL
);


CREATE TABLE smgt.entityinitialcount
(
    entityname    NVARCHAR(40) NOT NULL,
    initialcount  INT NULL,
    lastcount     INT NULL,
    lasttimestamp DATETIME2 NULL
);

CREATE TABLE dbo.account
(
    id                  NVARCHAR(18) NOT NULL,
    name                NVARCHAR(255) NULL,
    ownerid             NVARCHAR(18) NULL,
    industry            NVARCHAR(40) NULL,
    billingcity         NVARCHAR(40) NULL,
    billingstate        NVARCHAR(80) NULL,
    billingcountry      NVARCHAR(80) NULL,
    isdeleted           INT NULL
);

CREATE TABLE dbo.opportunity
(
    id                  NVARCHAR(18) NOT NULL,
    name                NVARCHAR(120) NULL,
    ownerid             NVARCHAR(18) NULL,
    createddate         DATETIME NULL,
    isclosed            INT NULL,
    iswon               INT NULL,
    probability         FLOAT NULL,
    accountid           NVARCHAR(18) NULL,
    amount              FLOAT NULL,
    expectedrevenue     FLOAT NULL,
    forecastcategoryname    NVARCHAR(40) NULL,
    stagename           NVARCHAR(40) NULL,
    leadsource          NVARCHAR(40) NULL,
    isdeleted           INT NULL,
    closedate           DATETIME NULL
);

CREATE TABLE dbo.userrole
(
    id                  NVARCHAR(18) NOT NULL,
    name                NVARCHAR(80) NULL,
    parentroleid        NVARCHAR(18) NULL
);

CREATE TABLE dbo.lead
(
    id                  NVARCHAR(18) NOT NULL,
    title               NVARCHAR(128) NULL,
    [status]            NVARCHAR(40) NULL,
    ownerid             NVARCHAR(18) NULL,
    leadsource          NVARCHAR(40) NULL,
    industry            NVARCHAR(40) NULL,
    createddate         DATETIME NULL,
    company             NVARCHAR(255) NULL,
    lastname            NVARCHAR(80) NULL,
    firstname           NVARCHAR(40) NULL,
    email               NVARCHAR(80) NULL,
    city                NVARCHAR(40) NULL,
    country             NVARCHAR(80) NULL
);

CREATE TABLE dbo.opportunitylineitem
(
    product2id          NVARCHAR(18) NULL,
    opportunityid       NVARCHAR(18) NULL,
    totalprice          FLOAT NULL
);

CREATE TABLE dbo.product2
(
    id                  NVARCHAR(18)  NOT NULL,
    [name]              NVARCHAR(255) NULL,
    family              NVARCHAR(40)  NULL,
    isactive            INT NULL
);

CREATE TABLE dbo.[user]
(
    id                  NVARCHAR(18) NOT NULL,
    [name]              NVARCHAR(121) NULL,
    managerid           NVARCHAR(18) NULL,
    isactive            INT NULL,
    userroleid          NVARCHAR(18) NULL,
    email               NVARCHAR(128) NULL
);

CREATE TABLE dbo.opportunitystage
(
    sortorder           INT NULL,
    masterlabel         NVARCHAR(255) NULL
);