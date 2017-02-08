SET NOCOUNT ON;

SELECT rast.sms_assigned_sites0 AS 'ConfigMgr Site',
       Count(rast.resourceid)   AS 'Total Clients Assigned'
FROM   v_ra_system_smsassignedsites rast INNER JOIN v_r_system rsys ON rast.resourceid = rsys.resourceid AND rsys.client0=1 AND rsys.obsolete0=0
GROUP  BY rast.sms_assigned_sites0;
