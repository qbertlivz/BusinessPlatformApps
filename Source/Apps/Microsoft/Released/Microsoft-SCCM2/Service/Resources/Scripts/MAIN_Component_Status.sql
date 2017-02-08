SET NOCOUNT ON;

SELECT count(*) NbCompStatus,
                CASE status
                  WHEN 0 THEN 'Healthy'
                  WHEN 1 THEN 'Warning'
                  WHEN 2 THEN 'Critical'
                END AS 'Status'
FROM   v_componentsummarizer comps
WHERE  tallyinterval = N'0001128000100008'  
GROUP BY comps.[status];
