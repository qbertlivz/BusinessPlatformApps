SET NOCOUNT ON;
	
SELECT CASE lnk.overalllinkstatus
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
       END      AS OverallLinkStatus,
       Count(*) AS 'Site Status Count'
FROM   vreplicationlinksummary lnk INNER JOIN dbo.vserverdata srv ON lnk.childsitecode = srv.sitecode
GROUP  BY lnk.overalllinkstatus  
