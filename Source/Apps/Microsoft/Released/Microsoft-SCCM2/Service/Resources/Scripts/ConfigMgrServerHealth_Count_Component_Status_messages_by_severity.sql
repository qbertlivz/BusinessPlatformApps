SET NOCOUNT ON;

SELECT Count(*) AS TotalMessages,
       sta.component,
       CASE sta.severity
         WHEN -1073741824 THEN 'Error'
         WHEN 1073741824 THEN 'Informational'
         WHEN -2147483648 THEN 'Warning'
         ELSE 'Unknown'
       END      AS 'Severity'
FROM   v_statusmessage sta
WHERE  modulename = 'SMS Server'
GROUP  BY sta.component,  sta.severity;
