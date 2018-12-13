SET NOCOUNT ON;

WITH machines AS
(
    SELECT computer.ItemKey  AS MachineID
    FROM   dbo.vsms_r_system computer
    WHERE  computer.Decommissioned0=0 AND computer.Obsolete0=0
),
healthsummary AS
(
    SELECT cs.resourceid                 AS MachineID
    FROM  dbo.[v_ch_clientsummary] cs
)
SELECT Count(*)
FROM 
    machines c INNER JOIN healthsummary hs ON hs.machineid = c.machineid;
