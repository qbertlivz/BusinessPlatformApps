SET NOCOUNT ON;

SELECT sy.netbios_name0        AS 'ComputerName',
       gswua.version0          AS 'WUA Agent Version',
       gsos.caption0           AS 'Operating System',
       gsos.csdversion0        AS 'Service Pack',
       Count(gswua.resourceid) AS 'Total', 
       sy.ResourceId           AS MachineId
FROM   v_gs_windowsupdateagentversion gswua INNER JOIN v_gs_operating_system gsos ON gswua.resourceid=gsos.resourceid
                                            INNER JOIN v_r_system sy ON gswua.resourceid = sy.resourceid
GROUP  BY gswua.version0,
          gsos.caption0,
          gsos.csdversion0,
          sy.netbios_name0,
		  sy.ResourceId;