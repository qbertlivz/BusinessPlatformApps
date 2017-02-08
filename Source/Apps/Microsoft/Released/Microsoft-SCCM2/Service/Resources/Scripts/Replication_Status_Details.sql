SET NOCOUNT ON;

 SELECT lnk.parentsitecode,
       lnk.childsitecode,
       CASE lnk.overalllinkstatus
         WHEN 0 THEN 'Deleted'
         WHEN 1 THEN 'Tombstoned'
         WHEN 2 THEN 'Active'
         WHEN 3 THEN 'Initializing'
         WHEN 4 THEN 'NotStarted'
         WHEN 5 THEN 'Error'
         WHEN 6 THEN 'Unknown'
         WHEN 7 THEN 'Degraded'
         WHEN 8 THEN 'Failed'
         WHEN 9 THEN 'Failed'
       END                                                          AS OverallLinkStatus,
       CASE lnk.globalparenttochildlinkstatus
         WHEN 0 THEN 'Deleted'
         WHEN 1 THEN 'Tombstoned'
         WHEN 2 THEN 'Active'
         WHEN 3 THEN 'Active_INTEROP'
         WHEN 4 THEN 'Initializing'
         WHEN 5 THEN 'NotStarted'
         WHEN 6 THEN 'Error'
         WHEN 7 THEN 'Unknown'
         WHEN 8 THEN 'Degraded'
         WHEN 9 THEN 'Failed'
       END                                                          AS GlobalParentToChildLinkStatus,
       CASE lnk.globalchildtoparentlinkstatus
         WHEN 0 THEN 'Deleted'
         WHEN 1 THEN 'Tombstoned'
         WHEN 2 THEN 'Active'
         WHEN 3 THEN 'Initializing'
         WHEN 4 THEN 'NotStarted'
         WHEN 5 THEN 'Error'
         WHEN 6 THEN 'Unknown'
         WHEN 7 THEN 'Degraded'
         WHEN 8 THEN 'Failed'
         WHEN 9 THEN 'Failed'
       END                                                          AS GlobalChildToParentLinkStatus,
       CASE lnk.sitechildtoparentlinkstatus
         WHEN 0 THEN 'Deleted'
         WHEN 1 THEN 'Tombstoned'
         WHEN 2 THEN 'Active'
         WHEN 3 THEN 'Initializing'
         WHEN 4 THEN 'NotStarted'
         WHEN 5 THEN 'Error'
         WHEN 6 THEN 'Unknown'
         WHEN 7 THEN 'Degraded'
         WHEN 8 THEN 'Failed'
         WHEN 9 THEN 'Failed'
         WHEN 99 THEN 'Secondary Site'
       END                                                          AS SiteChildToParentLinkStatus,
       ( lnk.lastsendtimeparenttochild - Getutcdate() + Getdate() ) AS LastSendTimeParentToChild,
       ( lnk.lastsendtimechildtoparent - Getutcdate() + Getdate() ) AS LastSendTimeChildToParent,
       ( lnk.lastsitesynctime - Getutcdate() + Getdate() )          AS LastSiteSyncTime,
       CASE srv.serverrole
         WHEN 'Peer' THEN 'Primaries'
         WHEN 'Proxy' THEN 'Secondaries'
       END                                                          AS ServerRole
FROM   dbo.rcm_replicationlinksummary_child lnk INNER JOIN dbo.vserverdata srv ON lnk.childsitecode = srv.sitecode;