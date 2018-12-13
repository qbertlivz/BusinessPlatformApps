SET NOCOUNT ON;

SELECT CASE
           WHEN chsumm.clientactivestatus = 0 THEN 'Inactive'
           WHEN chsumm.clientactivestatus = 1 THEN 'Active'
       END AS 'Status',
       Count(*) AS [NClients],
       CAST(1.0000 * Count(*) / (SELECT COUNT(*) FROM v_ch_clientsummary) AS DECIMAL(5, 4)) AS PCT
FROM   v_ch_clientsummary chsumm
GROUP BY chsumm.clientactivestatus 
