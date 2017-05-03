SET NOCOUNT ON;

SELECT count( DISTINCT user_name0 +
                unique_user_name0)     
FROM   v_r_user
WHERE  user_name0 IS NOT NULL AND 
       full_user_name0 NOT LIKE '%$%';
