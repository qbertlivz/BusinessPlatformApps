SET NOCOUNT ON;	

 SELECT ts.NAME  AS TaskSequenceName,
       Count(*) AS Total,
       Sum(CASE
             WHEN stat.laststate NOT IN ( 9, 11, 13 ) THEN 1
             ELSE 0
           END) AS Unknown,
       Sum(CASE
             WHEN stat.laststate = 9 THEN 1
             ELSE 0
           END) AS Running,
       Sum(CASE
             WHEN stat.laststate = 13 THEN 1
             ELSE 0
           END) AS Successful,
       Sum(CASE
             WHEN stat.laststate = 11 THEN 1
             ELSE 0
           END) AS Failed
INTO   #temp_ts
FROM   dbo.[v_tasksequencepackage] ts INNER JOIN dbo.[v_advertisement] adv ON ts.packageid = adv.packageid
                                      INNER JOIN dbo.[v_clientadvertisementstatus] stat ON adv.advertisementid = stat.advertisementid
                                      LEFT OUTER JOIN [dbo].[v_currentadvertisementassignments] assg ON stat.advertisementid = assg.advertisementid AND stat.resourceid = assg.resourceid
WHERE  ( stat.laststate IN ( 9, 11, 13 ) OR assg.resourceid IS NOT NULL )  AND stat.laststatustime IS NOT NULL
GROUP  BY ts.NAME;

SELECT tts.tasksequencename,
       Cast(tts.successful AS FLOAT) / Cast(tts.total AS FLOAT) * 100 AS 'Success',
       Cast(tts.running AS FLOAT) / Cast(tts.total AS FLOAT) * 100    AS 'Running',
       Cast(tts.failed AS FLOAT) / Cast(tts.total AS FLOAT) * 100     AS 'Failed',
       Cast(tts.unknown AS FLOAT) / Cast(tts.total AS FLOAT) * 100    AS 'Unknown'
FROM   #temp_ts tts

DROP TABLE #temp_ts  