SET NOCOUNT ON;

SELECT COUNT(*)
FROM   dbo.v_r_system
WHERE  user_domain0 IS NOT NULL AND
       user_name0 IS NOT NULL AND
       decommissioned0=0 AND 
       obsolete0=0;
