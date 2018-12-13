SET NOCOUNT ON;

SELECT DISTINCT vsms_sc_sysresuse.servername,
                vsms_sc_sysresuse.sitecode,
                v_packagestatusdistpointssumm.packageid,
                v_packagestatusrootsummarizer.NAME,
                v_packagestatusdistpointssumm.installstatus,
                v_packagestatusdistpointssumm.summarydate
FROM   vsms_sc_sysresuse vSMS_SC_SysResUse INNER JOIN v_packagestatusdistpointssumm v_PackageStatusDistPointsSumm ON v_packagestatusdistpointssumm.servernalpath = vsms_sc_sysresuse.nalpath
                                           LEFT OUTER JOIN v_packagestatusrootsummarizer v_PackageStatusRootSummarizer ON v_packagestatusrootsummarizer.packageid = v_packagestatusdistpointssumm.packageid
WHERE  vsms_sc_sysresuse.rolename = N'SMS Distribution Point';