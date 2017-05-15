SET NOCOUNT ON;

 SELECT DISTINCT DP.servername,
                dp.[description] 'DP Description',
                DP.sitecode,
                CASE
                  WHEN dp.ispeerdp = 1 THEN '*'
                  ELSE ''
                END            AS 'Branch DP',
                CASE
                  WHEN dp.bitsenabled = 1 THEN '*'
                  ELSE ''
                END            AS 'BITS enabled',
                CASE
                  WHEN dp.isprotected = 1 THEN '*'
                  ELSE ''
                END            AS 'AllowFallback',
                CASE
                  WHEN DPSI.ispulldp = 1 THEN '1'
                  ELSE ''
                END            AS 'PullDP',
                CASE
                  WHEN DPSI.ispxe = 1 THEN '*'
                  ELSE ''
                END            AS 'PXE',
                CASE
                  WHEN DPSI.ismulticast = 1 THEN '*'
                  ELSE ''
                END            AS 'MultiCast',
                CASE
                  WHEN dp.transferrate <> 0 THEN '*'
                  ELSE ''
                END            AS 'TransferRate',
                CASE
                  WHEN dp.prestagingallowed = 1 THEN '*'
                  ELSE ''
                END            AS 'PrestagingAllowed',
                CASE
                  WHEN dp.healthcheckenabled = 1 THEN '*'
                  ELSE ''
                END            AS 'ContentValidation',
                dp.groupcount  'DPGroupCount',
                CASE DPSI.messagestate
                  WHEN '1' THEN 'Healthy'
                  WHEN '2' THEN 'In Progress'
                  WHEN '3' THEN 'Warning'
                  WHEN '4' THEN 'Failed'
                  ELSE 'Unknown'
                END            AS 'State',
                DPSI.messagecount,
                DPSI.laststatustime
FROM   v_distributionpointinfo DP INNER JOIN vsms_dpstatusinfo AS DPSI ON DP.nalpath = DPSI.nalpath
