SET NOCOUNT ON;

 SELECT distributionpoint,
       Isnull([failed], 0)      AS Failed,
       Isnull([in progress], 0) AS [InProgress],
       Isnull([success], 0)     AS Success
FROM   (SELECT vSMS_SC_SysResUse.servername AS DistributionPoint,
               CASE v_packagestatusdistpointssumm.installstatus
                 WHEN 'Content monitoring' THEN 'Failed'
                 WHEN 'Package installation failed' THEN 'Failed'
                 WHEN 'Retrying package installation' THEN 'Failed'
                 WHEN 'Content updating' THEN 'In Progress'
                 WHEN 'Waiting to install package' THEN 'In Progress'
                 WHEN 'Package Installation complete' THEN 'Success'
               END                          AS Status,
               Count(CASE v_packagestatusdistpointssumm.installstatus
                       WHEN 'Content monitoring' THEN 'Failed'
                       WHEN 'Package installation failed' THEN 'Failed'
                       WHEN 'Retrying package installation' THEN 'Failed'
                       WHEN 'Content updating' THEN 'In Progress'
                       WHEN 'Waiting to install package' THEN 'In Progress'
                       WHEN 'Package Installation complete' THEN 'Success'
                     END)                   AS TotalByStatus
        FROM   [dbo].[v_systemresourcelist] vSMS_SC_SysResUse INNER JOIN [dbo].[v_packagestatusdistpointssumm] v_PackageStatusDistPointsSumm  ON v_packagestatusdistpointssumm.servernalpath = vSMS_SC_SysResUse.nalpath
                                                              LEFT OUTER JOIN [dbo].[v_packagestatusrootsummarizer] v_PackageStatusRootSummarizer  ON v_packagestatusrootsummarizer.packageid = v_packagestatusdistpointssumm.packageid
        WHERE  vSMS_SC_SysResUse.rolename = 'SMS Distribution Point'
        GROUP  BY vSMS_SC_SysResUse.servername,
                  CASE v_packagestatusdistpointssumm.installstatus
                    WHEN 'Content monitoring' THEN 'Failed'
                    WHEN 'Package installation failed' THEN 'Failed'
                    WHEN 'Retrying package installation' THEN 'Failed'
                    WHEN 'Content updating' THEN 'In Progress'
                    WHEN 'Waiting to install package' THEN 'In Progress'
                    WHEN 'Package Installation complete' THEN 'Success'
                  END) MyTable
       PIVOT ( Sum(totalbystatus) FOR [status] IN ([Failed], [In Progress],  [Success]) ) AS mypivot;
