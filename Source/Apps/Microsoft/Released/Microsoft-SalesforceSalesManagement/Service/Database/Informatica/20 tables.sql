SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go


-- Account
CREATE TABLE dbo.account
(
    id                           NVARCHAR(18) NULL,
    isdeleted                    TINYINT NULL,
    masterrecordid               NVARCHAR(18) NULL,
    name                         NVARCHAR(255) NULL,
    [type]                       NVARCHAR(40) NULL,
    parentid                     NVARCHAR(18) NULL,
    billingstreet                NVARCHAR(255) NULL,
    billingcity                  NVARCHAR(40) NULL,
    billingstate                 NVARCHAR(80) NULL,
    billingpostalcode            NVARCHAR(20) NULL,
    billingcountry               NVARCHAR(80) NULL,
    billinglatitude              FLOAT NULL,
    billinglongitude             FLOAT NULL,
    shippingstreet               NVARCHAR(255) NULL,
    shippingcity                 NVARCHAR(40) NULL,
    shippingstate                NVARCHAR(80) NULL,
    shippingpostalcode           NVARCHAR(20) NULL,
    shippingcountry              NVARCHAR(80) NULL,
    shippinglatitude             FLOAT NULL,
    shippinglongitude            FLOAT NULL,
    phone                        NVARCHAR(40) NULL,
    fax                          NVARCHAR(40) NULL,
    accountnumber                NVARCHAR(40) NULL,
    website                      NVARCHAR(255) NULL,
    photourl                     NVARCHAR(255) NULL,
    sic                          NVARCHAR(20) NULL,
    industry                     NVARCHAR(40) NULL,
    annualrevenue                FLOAT NULL,
    numberofemployees            INT NULL,
    [ownership]                  NVARCHAR(40) NULL,
    tickersymbol                 NVARCHAR(20) NULL,
    [description]                NVARCHAR(max) NULL,
    rating                       NVARCHAR(40) NULL,
    [site]                       NVARCHAR(80) NULL,
    ownerid                      NVARCHAR(18) NULL,
    createddate                  DATETIME NULL,
    createdbyid                  NVARCHAR(18) NULL,
    lastmodifieddate             DATETIME NULL,
    lastmodifiedbyid             NVARCHAR(18) NULL,
    systemmodstamp               DATETIME NULL,
    lastactivitydate             DATETIME NULL,
    lastvieweddate               DATETIME NULL,
    lastreferenceddate           DATETIME NULL,
    jigsaw                       NVARCHAR(20) NULL,
    jigsawcompanyid              NVARCHAR(20) NULL,
    cleanstatus                  NVARCHAR(40) NULL,
    accountsource                NVARCHAR(40) NULL,
    dunsnumber                   NVARCHAR(9) NULL,
    tradestyle                   NVARCHAR(255) NULL,
    naicscode                    NVARCHAR(8) NULL,
    naicsdesc                    NVARCHAR(120) NULL,
    yearstarted                  NVARCHAR(4) NULL,
    sicdesc                      NVARCHAR(80) NULL,
    dandbcompanyid               NVARCHAR(18) NULL,
    customerpriority__c          NVARCHAR(255) NULL,
    sla__c                       NVARCHAR(255) NULL,
    active__c                    NVARCHAR(255) NULL,
    numberoflocations__c         FLOAT NULL,
    upsellopportunity__c         NVARCHAR(255) NULL,
    slaserialnumber__c           NVARCHAR(10) NULL,
    slaexpirationdate__c         DATETIME NULL,
    import2_id__c                NVARCHAR(255) NULL,
    account_parent_import2_id__c NVARCHAR(255) NULL
);


-- Lead
 CREATE TABLE dbo.lead
(
    id                     NVARCHAR(18) NULL,
    isdeleted              TINYINT NULL,
    masterrecordid         NVARCHAR(18) NULL,
    lastname               NVARCHAR(80) NULL,
    firstname              NVARCHAR(40) NULL,
    salutation             NVARCHAR(40) NULL,
    name                   NVARCHAR(121) NULL,
    title                  NVARCHAR(128) NULL,
    company                NVARCHAR(255) NULL,
    street                 NVARCHAR(255) NULL,
    city                   NVARCHAR(40) NULL,
    [state]                NVARCHAR(80) NULL,
    postalcode             NVARCHAR(20) NULL,
    country                NVARCHAR(80) NULL,
    latitude               FLOAT NULL,
    longitude              FLOAT NULL,
    phone                  NVARCHAR(40) NULL,
    mobilephone            NVARCHAR(40) NULL,
    fax                    NVARCHAR(40) NULL,
    email                  NVARCHAR(80) NULL,
    website                NVARCHAR(255) NULL,
    photourl               NVARCHAR(255) NULL,
    [description]          NVARCHAR(max) NULL,
    leadsource             NVARCHAR(40) NULL,
    [status]               NVARCHAR(40) NULL,
    industry               NVARCHAR(40) NULL,
    rating                 NVARCHAR(40) NULL,
    annualrevenue          FLOAT NULL,
    numberofemployees      INT NULL,
    ownerid                NVARCHAR(18) NULL,
    isconverted            TINYINT NULL,
    converteddate          DATETIME NULL,
    convertedaccountid     NVARCHAR(18) NULL,
    convertedcontactid     NVARCHAR(18) NULL,
    convertedopportunityid NVARCHAR(18) NULL,
    isunreadbyowner        TINYINT NULL,
    createddate            DATETIME NULL,
    createdbyid            NVARCHAR(18) NULL,
    lastmodifieddate       DATETIME NULL,
    lastmodifiedbyid       NVARCHAR(18) NULL,
    systemmodstamp         DATETIME NULL,
    lastactivitydate       DATETIME NULL,
    lastvieweddate         DATETIME NULL,
    lastreferenceddate     DATETIME NULL,
    jigsaw                 NVARCHAR(20) NULL,
    jigsawcontactid        NVARCHAR(20) NULL,
    cleanstatus            NVARCHAR(40) NULL,
    companydunsnumber      NVARCHAR(9) NULL,
    dandbcompanyid         NVARCHAR(18) NULL,
    emailbouncedreason     NVARCHAR(255) NULL,
    emailbounceddate       DATETIME NULL,
    siccode__c             NVARCHAR(15) NULL,
    productinterest__c     NVARCHAR(255) NULL,
    primary__c             NVARCHAR(255) NULL,
    currentgenerators__c   NVARCHAR(100) NULL,
    numberoflocations__c   FLOAT NULL
);

-- Opportunity
CREATE TABLE dbo.opportunity
(
    id                            NVARCHAR(18) NULL,
    isdeleted                     TINYINT NULL,
    accountid                     NVARCHAR(18) NULL,
    isprivate                     TINYINT NULL,
    name                          NVARCHAR(120) NULL,
    [description]                 NVARCHAR(max) NULL,
    stagename                     NVARCHAR(40) NULL,
    amount                        FLOAT NULL,
    probability                   FLOAT NULL,
    expectedrevenue               FLOAT NULL,
    totalopportunityquantity      FLOAT NULL,
    closedate                     DATETIME NULL,
    [type]                        NVARCHAR(40) NULL,
    nextstep                      NVARCHAR(255) NULL,
    leadsource                    NVARCHAR(40) NULL,
    isclosed                      TINYINT NULL,
    iswon                         TINYINT NULL,
    forecastcategory              NVARCHAR(40) NULL,
    forecastcategoryname          NVARCHAR(40) NULL,
    campaignid                    NVARCHAR(18) NULL,
    hasopportunitylineitem        TINYINT NULL,
    pricebook2id                  NVARCHAR(18) NULL,
    ownerid                       NVARCHAR(18) NULL,
    createddate                   DATETIME NULL,
    createdbyid                   NVARCHAR(18) NULL,
    lastmodifieddate              DATETIME NULL,
    lastmodifiedbyid              NVARCHAR(18) NULL,
    systemmodstamp                DATETIME NULL,
    lastactivitydate              DATETIME NULL,
    fiscalquarter                 INT NULL,
    fiscalyear                    INT NULL,
    fiscal                        NVARCHAR(6) NULL,
    lastvieweddate                DATETIME NULL,
    lastreferenceddate            DATETIME NULL,
    deliveryinstallationstatus__c NVARCHAR(255) NULL,
    trackingnumber__c             NVARCHAR(12) NULL,
    ordernumber__c                NVARCHAR(8) NULL,
    currentgenerators__c          NVARCHAR(100) NULL,
    maincompetitors__c            NVARCHAR(100) NULL,
    import2_id__c                 NVARCHAR(255) NULL,
    account_account_import2_id__c NVARCHAR(255) NULL
);

-- OpportunityLineItem
CREATE TABLE [dbo].[opportunitylineitem]
(
    id                                    NVARCHAR(18) NULL,
    opportunityid                         NVARCHAR(18) NULL,
    sortorder                             INT NULL,
    pricebookentryid                      NVARCHAR(18) NULL,
    product2id                            NVARCHAR(18) NULL,
    productcode                           NVARCHAR(255) NULL,
    name                                  NVARCHAR(376) NULL,
    quantity                              FLOAT NULL,
    totalprice                            FLOAT NULL,
    unitprice                             FLOAT NULL,
    listprice                             FLOAT NULL,
    servicedate                           DATETIME NULL,
    [description]                         NVARCHAR(255) NULL,
    createddate                           DATETIME NULL,
    createdbyid                           NVARCHAR(18) NULL,
    lastmodifieddate                      DATETIME NULL,
    lastmodifiedbyid                      NVARCHAR(18) NULL,
    systemmodstamp                        DATETIME NULL,
    isdeleted                             TINYINT NULL,
    opportunity_opportunity_import2_id__c NVARCHAR(255) NULL
)  

-- OpportunityStage
CREATE TABLE dbo.opportunitystage
(
    id                 NVARCHAR(18) NOT NULL,
    masterlabel        NVARCHAR(255) NULL,
    sortorder          INT NULL,
    defaultprobability FLOAT NULL
);


-- Product2
CREATE TABLE dbo.product2
(
    id                 NVARCHAR(18) NULL,
    name               NVARCHAR(255) NULL,
    productcode        NVARCHAR(255) NULL,
    [description]      NVARCHAR(4000) NULL,
    isactive           TINYINT NULL,
    createddate        DATETIME NULL,
    createdbyid        NVARCHAR(18) NULL,
    lastmodifieddate   DATETIME NULL,
    lastmodifiedbyid   NVARCHAR(18) NULL,
    systemmodstamp     DATETIME NULL,
    family             NVARCHAR(40) NULL,
    isdeleted          TINYINT NULL,
    lastvieweddate     DATETIME NULL,
    lastreferenceddate DATETIME NULL
);


-- User
CREATE TABLE dbo.[user]
(
    id                                                 NVARCHAR(18) NULL,
    username                                           NVARCHAR(80) NULL,
    lastname                                           NVARCHAR(80) NULL,
    firstname                                          NVARCHAR(40) NULL,
    name                                               NVARCHAR(121) NULL,
    companyname                                        NVARCHAR(80) NULL,
    division                                           NVARCHAR(80) NULL,
    department                                         NVARCHAR(80) NULL,
    title                                              NVARCHAR(80) NULL,
    street                                             NVARCHAR(255) NULL,
    city                                               NVARCHAR(40) NULL,
    [state]                                            NVARCHAR(80) NULL,
    postalcode                                         NVARCHAR(20) NULL,
    country                                            NVARCHAR(80) NULL,
    latitude                                           FLOAT NULL,
    longitude                                          FLOAT NULL,
    email                                              NVARCHAR(128) NULL,
    emailpreferencesautobcc                            TINYINT NULL,
    emailpreferencesautobccstayintouch                 TINYINT NULL,
    emailpreferencesstayintouchreminder                TINYINT NULL,
    senderemail                                        NVARCHAR(80) NULL,
    sendername                                         NVARCHAR(80) NULL,
    [signature]                                        NVARCHAR(1333) NULL,
    stayintouchsubject                                 NVARCHAR(80) NULL,
    stayintouchsignature                               NVARCHAR(512) NULL,
    stayintouchnote                                    NVARCHAR(512) NULL,
    phone                                              NVARCHAR(40) NULL,
    fax                                                NVARCHAR(40) NULL,
    mobilephone                                        NVARCHAR(40) NULL,
    alias                                              NVARCHAR(8) NULL,
    communitynickname                                  NVARCHAR(40) NULL,
    badgetext                                          NVARCHAR(80) NULL,
    isactive                                           TINYINT NULL,
    timezonesidkey                                     NVARCHAR(40) NULL,
    userroleid                                         NVARCHAR(18) NULL,
    localesidkey                                       NVARCHAR(40) NULL,
    receivesinfoemails                                 TINYINT NULL,
    receivesadmininfoemails                            TINYINT NULL,
    emailencodingkey                                   NVARCHAR(40) NULL,
    profileid                                          NVARCHAR(18) NULL,
    usertype                                           NVARCHAR(40) NULL,
    languagelocalekey                                  NVARCHAR(40) NULL,
    employeenumber                                     NVARCHAR(20) NULL,
    delegatedapproverid                                NVARCHAR(18) NULL,
    managerid                                          NVARCHAR(18) NULL,
    lastlogindate                                      DATETIME NULL,
    lastpasswordchangedate                             DATETIME NULL,
    createddate                                        DATETIME NULL,
    createdbyid                                        NVARCHAR(18) NULL,
    lastmodifieddate                                   DATETIME NULL,
    lastmodifiedbyid                                   NVARCHAR(18) NULL,
    systemmodstamp                                     DATETIME NULL,
    offlinetrialexpirationdate                         DATETIME NULL,
    offlinepdatrialexpirationdate                      DATETIME NULL,
    userpermissionsmarketinguser                       TINYINT NULL,
    userpermissionsofflineuser                         TINYINT NULL,
    userpermissionscallcenterautologin                 TINYINT NULL,
    userpermissionsmobileuser                          TINYINT NULL,
    userpermissionssfcontentuser                       TINYINT NULL,
    userpermissionsknowledgeuser                       TINYINT NULL,
    userpermissionsinteractionuser                     TINYINT NULL,
    userpermissionssupportuser                         TINYINT NULL,
    userpermissionsjigsawprospectinguser               TINYINT NULL,
    userpermissionssiteforcecontributoruser            TINYINT NULL,
    userpermissionssiteforcepublisheruser              TINYINT NULL,
    userpermissionschatteranswersuser                  TINYINT NULL,
    userpermissionsworkdotcomuserfeature               TINYINT NULL,
    forecastenabled                                    TINYINT NULL,
    userpreferencesactivityreminderspopup              TINYINT NULL,
    userpreferenceseventreminderscheckboxdefault       TINYINT NULL,
    userpreferencestaskreminderscheckboxdefault        TINYINT NULL,
    userpreferencesremindersoundoff                    TINYINT NULL,
    userpreferencesdisableallfeedsemail                TINYINT NULL,
    userpreferencesdisablefollowersemail               TINYINT NULL,
    userpreferencesdisableprofilepostemail             TINYINT NULL,
    userpreferencesdisablechangecommentemail           TINYINT NULL,
    userpreferencesdisablelatercommentemail            TINYINT NULL,
    userpreferencesdisprofpostcommentemail             TINYINT NULL,
    userpreferencescontentnoemail                      TINYINT NULL,
    userpreferencescontentemailasandwhen               TINYINT NULL,
    userpreferencesapexpagesdevelopermode              TINYINT NULL,
    userpreferenceshidecsngetchattermobiletask         TINYINT NULL,
    userpreferencesdisablementionspostemail            TINYINT NULL,
    userpreferencesdismentionscommentemail             TINYINT NULL,
    userpreferenceshidecsndesktoptask                  TINYINT NULL,
    userpreferenceshidechatteronboardingsplash         TINYINT NULL,
    userpreferenceshidesecondchatteronboardingsplash   TINYINT NULL,
    userpreferencesdiscommentafterlikeemail            TINYINT NULL,
    userpreferencesdisablelikeemail                    TINYINT NULL,
    userpreferencesdisablemessageemail                 TINYINT NULL,
    userpreferencesjigsawlistuser                      TINYINT NULL,
    userpreferencesdisablebookmarkemail                TINYINT NULL,
    userpreferencesdisablesharepostemail               TINYINT NULL,
    userpreferencesenableautosubforfeeds               TINYINT NULL,
    userpreferencesdisablefilesharenotificationsforapi TINYINT NULL,
    userpreferencesshowtitletoexternalusers            TINYINT NULL,
    userpreferencesshowmanagertoexternalusers          TINYINT NULL,
    userpreferencesshowemailtoexternalusers            TINYINT NULL,
    userpreferencesshowworkphonetoexternalusers        TINYINT NULL,
    userpreferencesshowmobilephonetoexternalusers      TINYINT NULL,
    userpreferencesshowfaxtoexternalusers              TINYINT NULL,
    userpreferencesshowstreetaddresstoexternalusers    TINYINT NULL,
    userpreferencesshowcitytoexternalusers             TINYINT NULL,
    userpreferencesshowstatetoexternalusers            TINYINT NULL,
    userpreferencesshowpostalcodetoexternalusers       TINYINT NULL,
    userpreferencesshowcountrytoexternalusers          TINYINT NULL,
    userpreferencesshowprofilepictoguestusers          TINYINT NULL,
    userpreferencesshowtitletoguestusers               TINYINT NULL,
    userpreferencesshowcitytoguestusers                TINYINT NULL,
    userpreferencesshowstatetoguestusers               TINYINT NULL,
    userpreferencesshowpostalcodetoguestusers          TINYINT NULL,
    userpreferencesshowcountrytoguestusers             TINYINT NULL,
    userpreferencesdisablefeedbackemail                TINYINT NULL,
    userpreferencesdisableworkemail                    TINYINT NULL,
    userpreferenceshides1browserui                     TINYINT NULL,
    userpreferencesdisableendorsementemail             TINYINT NULL,
    userpreferencespathassistantcollapsed              TINYINT NULL,
    userpreferenceslightningexperiencepreferred        TINYINT NULL,
    contactid                                          NVARCHAR(18) NULL,
    accountid                                          NVARCHAR(18) NULL,
    callcenterid                                       NVARCHAR(18) NULL,
    extension                                          NVARCHAR(40) NULL,
    federationidentifier                               NVARCHAR(512) NULL,
    aboutme                                            NVARCHAR(1000) NULL,
    fullphotourl                                       NVARCHAR(1024) NULL,
    smallphotourl                                      NVARCHAR(1024) NULL,
    digestfrequency                                    NVARCHAR(40) NULL,
    defaultgroupnotificationfrequency                  NVARCHAR(40) NULL,
    jigsawimportlimitoverride                          INT NULL,
    lastvieweddate                                     DATETIME NULL,
    lastreferenceddate                                 DATETIME NULL
);


-- UserRole
CREATE TABLE dbo.userrole
(
    id                                    NVARCHAR(18) NULL,
    name                                  NVARCHAR(80) NULL,
    parentroleid                          NVARCHAR(18) NULL,
    rollupdescription                     NVARCHAR(80) NULL,
    opportunityaccessforaccountowner      NVARCHAR(40) NULL,
    caseaccessforaccountowner             NVARCHAR(40) NULL,
    contactaccessforaccountowner          NVARCHAR(40) NULL,
    forecastuserid                        NVARCHAR(18) NULL,
    mayforecastmanagershare               TINYINT NULL,
    lastmodifieddate                      DATETIME NULL,
    lastmodifiedbyid                      NVARCHAR(18) NULL,
    systemmodstamp                        DATETIME NULL,
    developername                         NVARCHAR(80) NULL,
    portalaccountid                       NVARCHAR(18) NULL,
    portaltype                            NVARCHAR(40) NULL,
    portalaccountownerid                  NVARCHAR(18) NULL,
    account_portalaccountid_import2_id__c NVARCHAR(255) NULL
);





/* SMGT specific schemas */
CREATE TABLE smgt.actualsales
(
  invoiceid   VARCHAR(50) NULL,
  actualsales DECIMAL NULL,
  invoicedate DATE NULL,
  accountid   VARCHAR(50) NULL,
  productid   VARCHAR(50) NULL
);

CREATE TABLE smgt.entityinitialcount
(
    entityname    NVARCHAR(40) NOT NULL,
    initialcount  INT NULL,
    lastcount     INT NULL,
    lasttimestamp DATETIME2 NULL
);

CREATE TABLE smgt.configuration
(
  id                     INT IDENTITY(1, 1) NOT NULL,
  configuration_group    VARCHAR(150) NOT NULL,
  configuration_subgroup VARCHAR(150) NOT NULL,
  [name]                 VARCHAR(150) NOT NULL,
  [value]                VARCHAR(max) NULL,
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
go
