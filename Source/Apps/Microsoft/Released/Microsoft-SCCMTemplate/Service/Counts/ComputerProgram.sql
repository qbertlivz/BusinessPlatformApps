SET NOCOUNT ON;

WITH ttable as
(select resourceid
FROM   v_gs_add_remove_programs p INNER JOIN vsms_r_system s ON p.resourceid = s.itemkey
WHERE  s.obsolete0 = 0 AND
       s.decommissioned0 = 0 AND
       p.displayname0 IS NOT NULL AND
       p.displayname0<>'' 
UNION
SELECT resourceid
FROM   v_gs_add_remove_programs_64 p INNER JOIN vsms_r_system s ON p.resourceid = s.itemkey
WHERE  s.obsolete0 = 0 AND
       s.decommissioned0 = 0 AND
       p.displayname0 IS NOT NULL AND
       p.displayname0<>''
)
select count(*) from ttable
