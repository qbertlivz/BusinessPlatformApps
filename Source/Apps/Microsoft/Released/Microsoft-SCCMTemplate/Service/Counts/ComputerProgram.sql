SET NOCOUNT ON;

SELECT Count(*)
FROM
    (
        SELECT p.resourceid,    
               p.displayname0,
               p.publisher0,
               p.version0
        FROM   dbo.v_gs_add_remove_programs p INNER JOIN dbo.vsms_r_system s ON p.resourceid = s.itemkey
        WHERE  s.obsolete0 = 0 AND
               s.decommissioned0 = 0 AND
               p.displayname0 IS NOT NULL AND
               p.displayname0<>'' 
        UNION
        SELECT p.resourceid,
               p.displayname0,
               p.publisher0,
               p.version0
        FROM   v_gs_add_remove_programs_64 p INNER JOIN vsms_r_system s ON p.resourceid = s.itemkey
        WHERE  s.obsolete0 = 0 AND
               s.decommissioned0 = 0 AND
               p.displayname0 IS NOT NULL AND
               p.displayname0<>''
    ) As T;
