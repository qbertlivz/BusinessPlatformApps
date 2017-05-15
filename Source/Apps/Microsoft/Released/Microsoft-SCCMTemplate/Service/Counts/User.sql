SET NOCOUNT ON;

SELECT Count( DISTINCT unique_user_name0)
FROM   dbo.v_r_user
WHERE  user_name0 IS NOT NULL AND 
       full_user_name0 NOT LIKE '%$%';
