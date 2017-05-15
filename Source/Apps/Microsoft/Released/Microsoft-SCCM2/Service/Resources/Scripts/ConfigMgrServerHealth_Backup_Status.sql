SET NOCOUNT ON;

SELECT tsk.sitecode,
       CASE
         WHEN tsk.isenabled = 0 THEN 'Disabled'
         WHEN tsk.isenabled = 1 AND dr.[state] IS NULL THEN 'Enabled'
         ELSE dr.state
       END AS 'State',
       dr.[time]
FROM   vsms_sc_sql_task tsk LEFT OUTER JOIN (SELECT sta.sitecode,
                                               CASE
                                                 WHEN sta.messageid = 5035 OR sta.messageid = 5040 THEN 'Success'
                                                 ELSE 'Failed'
                                               END           AS 'State',
                                               sta.[time] AS 'Time'
                                            FROM   v_statusmessage sta  INNER JOIN (SELECT sitecode,
                                                                                           Max([time]) AS 'Time'
                                                                                    FROM   v_statusmessage
                                                                                    WHERE  component = 'SMS_SITE_Backup' AND messageid IN ( 5035, 5040, 5060 )
                                                                                    GROUP  BY sitecode
                                                                                   ) stb
                                                                            ON stb.sitecode=sta.sitecode AND stb.[time]=sta.[time]
                                            WHERE  sta.component = 'SMS_SITE_Backup' AND sta.messageid IN ( 5035, 5040, 5060 )
                                           ) dr
                                ON dr.sitecode = tsk.sitecode
WHERE  tsk.taskname = 'Backup SMS Site Server';