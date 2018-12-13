SET NOCOUNT ON;

SELECT DISTINCT cs.compliancestate,
                ComplianceStateName=(SELECT statename
                                     FROM   sr_statenames sn
                                     WHERE  sn.stateid = cs.compliancestate  AND sn.topictype = 401),
                TotPerState = Count (cs.itemkey) OVER (partition BY cs.compliancestate),
                PTot = CONVERT(NUMERIC(10, 2), ( Count (cs.itemkey) OVER (partition BY cs.compliancestate) ), 2) / CONVERT(NUMERIC(10, 2), Count(cs.itemkey) OVER(), 2),
                Tot = Count(cs.itemkey)  OVER()
FROM   v_ci_currentcompliancestatus cs INNER JOIN vci_configurationitems ci ON cs.ci_id = ci.ci_id
WHERE  ci.citype_id = 2;