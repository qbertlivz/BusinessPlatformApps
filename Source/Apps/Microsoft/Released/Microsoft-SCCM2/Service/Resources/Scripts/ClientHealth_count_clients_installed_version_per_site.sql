SET NOCOUNT ON;

 SELECT CASE
         WHEN sysval.client_version0 = '5.00.7711.0000' THEN 'ConfigMgr 2012 RTM'
         WHEN sysval.client_version0 = '5.00.7711.0200' THEN 'ConfigMgr 2012 RTM CU1'
         WHEN sysval.client_version0 = '5.00.7711.0301' THEN 'ConfigMgr 2012 RTM CU2'
         WHEN sysval.client_version0 = '5.00.7804.1000' THEN 'ConfigMgr 2012 SP1'
         WHEN sysval.client_version0 = '5.00.7804.1202' THEN 'ConfigMgr 2012 SP1 CU1'
         WHEN sysval.client_version0 = '5.00.7804.1300' THEN 'ConfigMgr 2012 SP1 CU2'
         WHEN sysval.client_version0 = '5.00.7804.1400' THEN 'ConfigMgr 2012 SP1 CU3'
         WHEN sysval.client_version0 = '5.00.7804.1500' THEN 'ConfigMgr 2012 SP1 CU4'
         WHEN sysval.client_version0 = '5.00.7804.1600' THEN 'ConfigMgr 2012 SP1 CU5'
         WHEN sysval.client_version0 = '5.00.7958.1000' THEN 'ConfigMgr 2012 R2'
         WHEN sysval.client_version0 = '5.00.7958.1101' THEN 'ConfigMgr 2012 R2 with KB290500'
         WHEN sysval.client_version0 = '5.00.7958.1203' THEN 'ConfigMgr 2012 R2 CU1'
         WHEN sysval.client_version0 = '5.00.7958.1303' THEN 'ConfigMgr 2012 R2 CU2'
         WHEN sysval.client_version0 = '5.00.7958.1401' THEN 'ConfigMgr 2012 R2 CU3'
         WHEN sysval.client_version0 = '5.00.7958.1501' THEN 'ConfigMgr 2012 R2 CU4'
         WHEN sysval.client_version0 = '5.00.7958.1601' THEN 'ConfigMgr 2012 R2 CU5'
         WHEN sysval.client_version0 = '5.00.8239.1000' THEN 'ConfigMgr 2012 R2 SP1'
         WHEN sysval.client_version0 = '5.00.8239.1203' THEN 'ConfigMgr 2012 R2 SP1 CU1'
         WHEN sysval.client_version0 = '5.00.8239.1301' THEN 'ConfigMgr 2012 R2 SP1 CU2'
         WHEN sysval.client_version0 = '5.00.8239.1403' THEN 'ConfigMgr 2012 R2 SP1 CU3'
         WHEN sysval.client_version0 = '5.00.8325.1000' THEN 'ConfigMgr Update 1511'
         WHEN sysval.client_version0 = '5.00.8355.1000' THEN 'ConfigMgr Update 1602'
         WHEN sysval.client_version0 = '5.00.8355.1306' THEN 'ConfigMgr Update 1602 UR1'
         WHEN sysval.client_version0 = '5.00.8412.1007' THEN 'ConfigMgr Update 1606'
         WHEN sysval.client_version0 = '5.00.8412.1307' THEN 'ConfigMgr Update 1606 UR1'
         ELSE sysval.client_version0
       END                        AS 'Description',
       sysval.client_version0     AS 'VersionNumber',
       cmsite.sms_assigned_sites0 AS 'Site Code',
       Count(sysval.resourceid)   AS 'Total'
FROM   v_r_system_valid sysval  INNER JOIN dbo.v_ra_system_smsassignedsites cmsite
               ON sysval.resourceid = cmsite.resourceid
GROUP  BY cmsite.sms_assigned_sites0, sysval.client_version0;
