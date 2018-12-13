SET NOCOUNT ON;

 SELECT CASE [status]
         WHEN 0 THEN 'Healthy'
         WHEN 1 THEN 'Warning'
         WHEN 2 THEN 'Critical'
       END           AS 'Status',
       componentname AS 'Component Name',
       machinename   AS 'Site System',
       componenttype AS 'Component Type',
       sitecode      AS 'Site Code',
       CASE availabilitystate
         WHEN 0 THEN 'Online'
         WHEN 3 THEN 'Offline'
       END           AS 'Availability State',
       CASE [state]
         WHEN 0 THEN 'Stopped'
         WHEN 1 THEN 'Started'
         WHEN 2 THEN 'Paused'
         WHEN 3 THEN 'Installing'
         WHEN 4 THEN 'Re-Installing'
         WHEN 6 THEN 'De-Installing'
       END           AS 'State',
       CASE [type]
         WHEN 0 THEN 'Auto-Starting'
         WHEN 1 THEN 'Scheduled'
         WHEN 2 THEN 'Manual'
       END           AS 'Type',
       infos         AS 'Infos Counter',
       warnings      AS 'Warnings Counter',
       errors        AS 'Errors Counter',
       lastcontacted AS 'Last Contacted'
FROM   v_componentsummarizer
WHERE  tallyinterval = N'0001128000100008';
