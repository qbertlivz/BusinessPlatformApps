SET NOCOUNT ON;

WITH hwplatform AS
(
    SELECT cs.ResourceID           MachineID,
           cs.Manufacturer0        Manufacturer,
           cs.Model0               [Model],
           cs.SystemType0          [Platform],
           cs.TotalPhysicalMemory0 [Physical Memory],
           ROW_NUMBER() OVER (PARTITION BY cs.ResourceID ORDER BY cs.[TimeStamp] DESC) AS RN
    FROM   dbo.v_GS_COMPUTER_SYSTEM cs
)
SELECT c.ItemKey                       machineid,
       client.SiteCode                 sitecode,
       c.Name0                         [name],
       c.Operating_System_Name_and0 AS [operating system],
       c.Client_Type0                  [client type],
       hwplatform.Manufacturer,
       hwplatform.Model,
       hwplatform.[Platform],
       hwplatform.[Physical Memory]
FROM   dbo.vSMS_R_System c
           LEFT JOIN dbo.v_ClientMachines AS client ON c.ItemKey = client.resourceid
           LEFT JOIN hwplatform ON c.ItemKey = hwplatform.MachineID AND hwplatform.RN=1
WHERE
     c.Decommissioned0=0 AND c.Obsolete0=0 AND c.Name0 NOT LIKE N'%|%';
