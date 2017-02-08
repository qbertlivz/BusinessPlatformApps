SET NOCOUNT ON;

SELECT Count(*) AS 'Total',
       CASE
         WHEN ch.lastevaluationhealthy = 1 THEN 'Healthy Client'
         WHEN ch.lastevaluationhealthy = 2 THEN 'Unhealthy Client'
         WHEN ch.lastevaluationhealthy = 3 THEN 'Unknown'
       END      AS 'Health',
       ch.lastevaluationhealthy
FROM   v_r_system sys INNER JOIN v_ch_clientsummary ch ON sys.resourceid=ch.resourceid AND sys.client0=1 AND sys.obsolete0=0 AND sys.decommissioned0=0
GROUP  BY ch.lastevaluationhealthy;