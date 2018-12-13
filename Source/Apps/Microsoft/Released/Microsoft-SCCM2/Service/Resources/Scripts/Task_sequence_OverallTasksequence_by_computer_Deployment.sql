SET NOCOUNT ON;

SELECT sysr.netbios_name0,
       tsname.NAME,
       stat.resourceid,
       adv.packageid       AS TaskSequenceID,
       stat.advertisementid,
       stat.laststatustime AS 'Date',
       ( CASE
           WHEN stat.laststate NOT IN ( 9, 11, 13 ) THEN 'Pending'
           WHEN stat.laststate = 9 THEN 'Running'
           WHEN stat.laststate = 13 THEN 'Successful'
           WHEN stat.laststate = 11 THEN 'Failed'
         END )             AS 'State'
FROM   v_clientadvertisementstatus stat INNER JOIN v_r_system sysr ON sysr.resourceid = stat.resourceid
                                        INNER JOIN v_advertisement adv ON adv.advertisementid = stat.advertisementid
                                        INNER JOIN v_tasksequencepackage tsname ON adv.packageid = tsname.packageid
                                        LEFT OUTER JOIN v_currentadvertisementassignments assg ON stat.advertisementid = assg.advertisementid AND stat.resourceid = assg.resourceid
WHERE  ( stat.laststate IN ( 8, 9, 10, 11, 12, 13 )  OR assg.resourceid IS NOT NULL );
