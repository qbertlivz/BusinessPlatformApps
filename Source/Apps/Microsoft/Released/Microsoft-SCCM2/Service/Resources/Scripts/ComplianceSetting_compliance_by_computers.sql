SET NOCOUNT ON;
   
SET nocount ON;

DECLARE @allassignments TABLE
(
    assignmentid   INT,
    collectionid   NVARCHAR(512),
    collectionname NVARCHAR(512),
    ci_id          INT,
    displayname    NVARCHAR(512),
    cidescription  NVARCHAR (512),
    citype_id      INT,
    ci_uniqueid    NVARCHAR(512),
    civersion      INT
);

INSERT INTO @allassignments
            (assignmentid,
             collectionid,
             collectionname,
             ci_id,
             displayname,
             cidescription,
             citype_id,
             ci_uniqueid,
             civersion)
SELECT assign.assignmentid,
       coll.collectionid AS CollectionID,
       coll.NAME         AS CollectionName,
       bls.ci_id,
       [BaselineName] = dbo.Fn_getlocalizedciname(1033, bls.ci_id),
       [BaselineDesc] = dbo.Fn_getlocalizedcidescription(1033, bls.ci_id),
       bls.citype_id,
       bls.ci_uniqueid,
       bls.civersion
FROM   v_ciassignment assign
       INNER JOIN v_collection coll
               ON coll.collectionid = assign.collectionid
       INNER JOIN v_ciassignmenttoci targ
               ON targ.assignmentid = assign.assignmentid
       INNER JOIN v_cirelation_all rel
               ON rel.ci_id = targ.ci_id
       INNER JOIN v_smsconfigurationitems bls
               ON bls.ci_id = rel.referencedci_id
WHERE  citype_id IN ( 2, 50 )
       AND bls.istombstoned = 0
       AND assign.assignmenttype IN ( 0, 8 );

--Remove previous temporary table if exists
IF Object_id(N'TempDB.DBO.#assstatus') IS NOT NULL
BEGIN
    DROP TABLE #assstatus;
END;

CREATE TABLE #assstatus
(
    lastcompliancemessagetime DATETIME,
    resourceid                INT,
    userid                    INT,
    ci_id                     INT
);

INSERT INTO #assstatus
SELECT Max(assstatus.lastevaluationmessagetime) AS LastComplianceMessageTime,
       resourceid,
       userid,
       ci_id                                    AS BL_ID
FROM   v_ciassignmentstatus assstatus
       JOIN @allassignments cis
         ON cis.assignmentid = assstatus.assignmentid
GROUP  BY resourceid,
          userid,
          ci_id;

CREATE INDEX ix ON #assstatus (resourceid, userid, ci_id);

--Remove previous temporary table if exists
IF Object_id(N'TempDB.DBO.#machines') IS NOT NULL
BEGIN
    DROP TABLE #machines;
END;

CREATE TABLE #machines(itemkey INT);

CREATE INDEX ix ON #machines (itemkey);

INSERT INTO #machines
SELECT DISTINCT cm.resourceid
FROM   v_clientcollectionmembers cm
       JOIN v_collection coll
         ON coll.collectionid = cm.collectionid
       JOIN @allassignments allbl
         ON allbl.collectionid = cm.collectionid;

--Remove previous temporary table if exists
IF Object_id(N'TempDB.DBO.#status') IS NOT NULL
  BEGIN
      DROP TABLE #status;
  END;

CREATE TABLE #status
  (
     resourceid                  INT,
     userid                      INT,
     ci_id                       INT,
     compliancestate             NVARCHAR(512),
     errorcode                   INT,
     maxnoncompliancecriticality INT
  );

INSERT INTO #status
SELECT cs.resourceid,
       cs.userid,
       allbl.ci_id,
       CASE cs.compliancestate
         WHEN 0 THEN 'Compliance State Unknown'
         WHEN 1 THEN 'Compliant'
         WHEN 2 THEN 'Non-Compliant'
         WHEN 3 THEN 'Eror'
       END,
       errors.errorcode,
       cs.maxnoncompliancecriticality
FROM   (SELECT DISTINCT displayname,
                        ci_id,
                        civersion,
                        ci_uniqueid
        FROM   @allassignments) allbl
       INNER JOIN v_cicurrentcompliancestatus cs
               ON cs.ci_id = allbl.ci_id
                  AND cs.civersion = allbl.civersion
       INNER JOIN #machines cm
               ON cm.itemkey = cs.resourceid
       LEFT JOIN v_ci_currenterrordetails errors
              ON errors.currentcompliancestatusid = cs.ci_currentcompliancestatusid;

CREATE INDEX ix2
  ON #status (ci_id, resourceid, userid);

SELECT r.netbios_name0    AS MachineName,
       [Domain] = r.resource_domain_or_workgr0,
       [ADSite] =r.ad_site_name0,
       CASE
         WHEN TargetCompliance.errorcode = 0 THEN NULL
         ELSE master.dbo.Fn_varbintohexstr(CONVERT(VARBINARY(8), CONVERT(INT, targetCOmpliance.errorcode)))
       END                AS ErrorCode,
       assign.displayname AS BaselineName,
       assign.civersion   AS BaselineContentVersion,
       TargetCompliance.compliancestate,
       TargetCompliance.maxnoncompliancecriticality,
       assstatus.lastcompliancemessagetime,
       assign.ci_uniqueid AS Baseline_UniqueID,
       r.ResourceId       AS MachineId
FROM   (SELECT DISTINCT displayname,
                        ci_id,
                        civersion,
                        ci_uniqueid
        FROM   @allassignments) assign  INNER JOIN (SELECT DISTINCT *  FROM   #status) TargetCompliance  ON TargetCompliance.ci_id = assign.ci_id
                                        INNER JOIN v_r_system_valid r ON r.resourceid = TargetCompliance.resourceid
                                        INNER JOIN #assstatus assstatus ON assstatus.resourceid = TargetCompliance.resourceid AND assstatus.userid = TargetCompliance.userid AND assstatus.ci_id = assign.ci_id;

DROP TABLE #assstatus;
DROP TABLE #status;
DROP TABLE #machines;
