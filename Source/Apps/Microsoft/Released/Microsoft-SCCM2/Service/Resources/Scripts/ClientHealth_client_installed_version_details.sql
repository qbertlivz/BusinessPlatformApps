SET NOCOUNT ON;

SELECT rsys.netbios_name0     AS 'Netbios',
       rsys.full_domain_name0 AS 'Domain',
       rsys.user_name0        AS 'User',
       rsys.client_version0   AS 'Client Version',
       rsys.creation_date0    AS 'Creation Date',
       chsum.lastddr          AS 'Last DDR',
       rsys.ResourceId        AS MachineId
FROM   v_r_system rsys INNER JOIN v_ch_clientsummary chsum ON rsys.resourceid = chsum.resourceid AND rsys.client0=1 AND rsys.obsolete0=0;
