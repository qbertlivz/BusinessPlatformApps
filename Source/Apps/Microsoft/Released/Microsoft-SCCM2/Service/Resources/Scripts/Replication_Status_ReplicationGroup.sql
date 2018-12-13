SET NOCOUNT ON;

SELECT  trk.SiteRequesting
       ,trk.SiteFulfilling
       ,trk.ReplicationGroup
       ,dta.ReplicationPattern
       ,trk.InitializationStatus AS [InitStatusCode]
  INTO #InitInfo
  FROM dbo.RCM_DrsInitializationTracking trk
       INNER JOIN dbo.ReplicationData dta
          ON trk.ReplicationGroup = dta.ReplicationGroup
       INNER JOIN (
                   SELECT  trk.SiteRequesting
                          ,trk.SiteFulfilling
                          ,trk.ReplicationGroup
                          ,MAX(trk.ModifiedTime) AS [MaxTime]
                     FROM dbo.RCM_DrsInitializationTracking  trk 
                    GROUP BY  trk.SiteRequesting
                             ,trk.SiteFulfilling
                             ,trk.ReplicationGroup
                   ) mxt
          ON trk.SiteRequesting = mxt.SiteRequesting
         AND trk.SiteFulfilling = mxt.SiteFulfilling
         AND trk.ReplicationGroup = mxt.ReplicationGroup
         AND trk.ModifiedTime = mxt.MaxTime

-- Site Info
SELECT  SiteRequesting
       ,SiteFulfilling
       ,ReplicationPattern
       ,COUNT(CASE InitStatusCode WHEN 6 THEN ReplicationGroup END) [ReplGroupsCompleted]
       ,COUNT(CASE WHEN InitStatusCode NOT IN (6,99) THEN ReplicationGroup END) [ReplGroupsPending]
       ,COUNT(CASE InitStatusCode WHEN 99 THEN ReplicationGroup END) [ReplGroupsFailed]
       ,COUNT(CASE InitStatusCode WHEN 6 THEN ReplicationGroup END)/CONVERT(float,COUNT(ReplicationGroup))*100.00 [PercentComplete]
  FROM #InitInfo
 WHERE SiteRequesting = (sELECT ThisSiteCode SiteCode FROM vSMSData)
   AND ReplicationPattern = 'Site'
 GROUP BY  SiteRequesting
          ,SiteFulfilling
          ,ReplicationPattern
UNION ALL
-- Global Info
SELECT  SiteRequesting
       ,SiteFulfilling
       ,ReplicationPattern
       ,COUNT(CASE InitStatusCode WHEN 6 THEN ReplicationGroup END) [ReplGroupsCompleted]
       ,COUNT(CASE WHEN InitStatusCode NOT IN (6,99) THEN ReplicationGroup END) [ReplGroupsPending]
       ,COUNT(CASE InitStatusCode WHEN 99 THEN ReplicationGroup END) [ReplGroupsFailed]
       ,COUNT(CASE InitStatusCode WHEN 6 THEN ReplicationGroup END)/CONVERT(float,COUNT(ReplicationGroup))*100.00 [PercentComplete]
  FROM #InitInfo
 WHERE SiteRequesting !=(sELECT ThisSiteCode SiteCode FROM vSMSData)
   AND ReplicationPattern = 'Global'
 GROUP BY  SiteRequesting
          ,SiteFulfilling
          ,ReplicationPattern

DROP TABLE #InitInfo

