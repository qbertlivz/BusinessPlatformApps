IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='zzz' AND TABLE_SCHEMA='aal' AND TABLE_TYPE='BASE TABLE')
  DROP TABLE aal.zzz;

WITH Last(correlationId, timestamp) AS
(
    SELECT correlationId, MAX(timestamp) AS TS FROM aal.ActivityLogData WHERE eventCategory='ServiceHealth' GROUP BY correlationId
)
SELECT ald.correlationId, ald.[status] -- INTO aal.zzz
FROM
   aal.ActivityLogData ald INNER JOIN Last ON ald.correlationId=last.correlationId AND ald.timestamp=last.timestamp
WHERE
    ald.eventCategory = 'ServiceHealth';
