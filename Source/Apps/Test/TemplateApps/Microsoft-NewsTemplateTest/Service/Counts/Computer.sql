SET NOCOUNT ON;

WITH hwplatform AS
(
    SELECT cs.resourceid           MachineID
    FROM   dbo.v_gs_computer_system cs INNER JOIN (SELECT resourceid, MAX([TimeStamp]) AS [TimeStamp] FROM v_gs_computer_system GROUP BY resourceid) lts
                 ON cs.[TimeStamp]=lts.[TimeStamp] AND cs.ResourceID=lts.ResourceID
)
SELECT count(*)
FROM   vsms_r_system c
           LEFT JOIN v_clientmachines AS client ON c.itemkey = client.resourceid
           LEFT JOIN hwplatform AS hwp ON c.itemkey = hwp.machineid
WHERE
    decommissioned0=0 AND obsolete0=0 AND c.name0 NOT LIKE N'%|%';
