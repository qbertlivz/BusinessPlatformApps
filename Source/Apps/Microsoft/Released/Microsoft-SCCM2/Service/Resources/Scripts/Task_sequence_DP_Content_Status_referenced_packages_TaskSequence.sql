SET NOCOUNT ON;

SELECT smssitecode AS SiteCode,
       servername,
       packageid,
       NAME        AS PackageName,
       [version],
       CASE installstatus
         WHEN 'Content monitoring' THEN 'Failed'
         WHEN 'Package installation failed' THEN 'Failed'
         WHEN 'Retrying package installation' THEN 'Failed'
         WHEN 'Content updating' THEN 'In Progress'
         WHEN 'Waiting to install package' THEN 'In Progress'
         WHEN 'Package Installation complete' THEN 'Success'
         WHEN 'Not assigned' THEN 'Not Assigned'
       END         AS [Status],
       summarydate,
       taskseqid
FROM   (SELECT servername,
               T01.smssitecode,
               isshareddp,
               T00.*,
               Isnull([installstatus], 'Not assigned') AS InstallStatus,
               summarydate
        FROM   (SELECT Z00.taskseqid,
                       Z01.packageid,
                       Z01.NAME,
                       Z01.[description],
                       Z01.[version],
                       Z01.sourceversion
                FROM   (SELECT v_tasksequencereferencesinfo.referencepackageid AS PackageID,
                               packageid                                       TaskSeqID
                        FROM   v_tasksequencereferencesinfo v_TaskSequenceReferencesInfo
                        UNION
                        SELECT v_tasksequenceappreferencesinfo.refapppackageid AS PackageID,
                               packageid                                       TaskSeqID
                        FROM   v_tasksequenceappreferencesinfo v_TaskSequenceAppReferencesInfo) AS Z00
                       LEFT JOIN v_package AS Z01
                              ON Z00.packageid = Z01.packageid) AS T00
               CROSS JOIN (SELECT servername,
                                  smssitecode,
                                  nalpath,
                                  IsSharedDP = CASE
                                                 WHEN identityguid = '' THEN 1
                                                 ELSE 0
                                               END
                           FROM   v_distributionpoints
                           WHERE  nalpath IN (SELECT dp.servernalpath
                                              FROM   v_tasksequencereferencesinfo AS tsref LEFT JOIN v_packagestatusdistpointssumm AS dp ON dp.packageid = tsref.referencepackageid AND dp.state = 0
                                              WHERE  servernalpath IS NOT NULL)) AS T01
               LEFT JOIN v_packagestatusdistpointssumm AS T02
                      ON ( T00.packageid = T02.packageid
                           AND T01.nalpath = T02.servernalpath )) AS K01; 