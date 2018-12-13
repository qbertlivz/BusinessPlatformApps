SET NOCOUNT ON;

SELECT tspkg.packageid AS TSID,
       Sum(CASE
             WHEN ps.targeted > 0
                  AND ps.targeted = ps.installed THEN 1
             ELSE 0
           END)        AS FullyDistributed,
       Sum(CASE
             WHEN ps.failed > 0
                   OR ps.retrying > 0 THEN 1
             ELSE 0
           END)        AS PartiallyDistributed,
       Sum(CASE
             WHEN ps.installed = 0 THEN 1
             ELSE 0
           END)        AS NotAssignedPackage
INTO   #temp_tscomp
FROM   (SELECT v_TSRefInfo.referencepackageid   AS PackageID,
               v_TSRefInfo.packageid            AS TSPkgID,
               v_TSRefInfo.referencepackagetype AS PkgType
        FROM   v_tasksequencereferencesinfo v_TSRefInfo
        UNION
        SELECT v_TSAppRefInfo.refapppackageid AS PackageID,
               v_TSAppRefInfo.packageid       AS TSPkgID,
               8                              AS PkgType
        FROM   v_tasksequenceappreferencesinfo v_TSAppRefInfo) RefPkgs INNER JOIN v_packagestatusrootsummarizer ps ON ps.packageid = RefPkgs.packageid
                                                                       INNER JOIN v_tasksequencepackage tspkg ON tspkg.packageid = RefPkgs.tspkgid
GROUP  BY tspkg.packageid;

SELECT adv.packageid                              AS TaskSequenceID,
       ts.NAME                                    AS TaskSequenceName,
       CONVERT(VARCHAR, stat.laststatustime, 104) AS 'Date',
       Count(*)                                   AS Total,
       Sum(CASE
             WHEN stat.laststate = 9 THEN 1
             ELSE 0
           END)                                   AS Running,
       Sum(CASE
             WHEN stat.laststate = 13 THEN 1
             ELSE 0
           END)                                   AS Successful,
       Sum(CASE
             WHEN stat.laststate = 11 THEN 1
             ELSE 0
           END)                                   AS Failed
INTO   #temp_tsstate
FROM   v_clientadvertisementstatus stat INNER JOIN v_advertisement adv ON adv.advertisementid = stat.advertisementid AND adv.programname = '*'
                                        INNER JOIN v_tasksequencepackage ts ON ts.packageid = adv.packageid
       LEFT JOIN (SELECT advertisementid,
                         resourceid,
                         Max(executiontime) AS [Time]
                  FROM   v_taskexecutionstatus
                  GROUP  BY advertisementid,  resourceid) AS tse1
              ON tse1.advertisementid = stat.advertisementid AND tse1.resourceid = stat.resourceid
       LEFT JOIN v_taskexecutionstatus tse2 ON tse1.advertisementid = tse2.advertisementid AND tse1.resourceid = tse2.resourceid AND tse1.time = tse2.executiontime
GROUP  BY CONVERT(VARCHAR, stat.laststatustime, 104), adv.packageid, ts.NAME;

SELECT *,
       #temp_tscomp.fullydistributed + #temp_tscomp.notassignedpackage + #temp_tscomp.partiallydistributed AS TotalPkg
FROM   #temp_tsstate INNER JOIN #temp_tscomp ON #temp_tscomp.tsid = #temp_tsstate.tasksequenceid;

DROP TABLE #temp_tscomp;
DROP TABLE #temp_tsstate; 