SET NOCOUNT ON;

SELECT count (distinct ( arp.prodid0 + convert(varchar(50),arp.[timestamp])))
FROM   v_gs_add_remove_programs arp
           INNER JOIN vsms_r_system s ON arp.resourceid = s.itemkey
WHERE  arp.displayname0 NOT LIKE '%(KB%' AND
       arp.displayname0 NOT LIKE '%Update%' AND
       arp.[timestamp] < Getdate() AND
       arp.[timestamp] > '2016/1/1' AND
       installdate0 IS NOT NULL AND
       Isdate(installdate0) = 1 AND
       s.decommissioned0 = 0 AND
       s.obsolete0 = 0
