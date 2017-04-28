SET NOCOUNT ON;

SELECT 
T1.Client_Version0 as VersionNumber,
CASE
WHEN T1.Client_Version0 = '5.00.7711.0000' THEN 'ConfigMgr 2012 RTM'
WHEN T1.Client_Version0 = '5.00.7711.0200' THEN 'ConfigMgr 2012 RTM CU1'
WHEN T1.Client_Version0 = '5.00.7711.0301' THEN 'ConfigMgr 2012 RTM CU2'
WHEN T1.Client_Version0 = '5.00.7804.1000' THEN 'ConfigMgr 2012 SP1'
WHEN T1.Client_Version0 = '5.00.7804.1202' THEN 'ConfigMgr 2012 SP1 CU1'
WHEN T1.Client_Version0 = '5.00.7804.1300' THEN 'ConfigMgr 2012 SP1 CU2'
WHEN T1.Client_Version0 = '5.00.7804.1400' THEN 'ConfigMgr 2012 SP1 CU3'
WHEN T1.Client_Version0 = '5.00.7804.1500' THEN 'ConfigMgr 2012 SP1 CU4'
WHEN T1.Client_Version0 = '5.00.7804.1600' THEN 'ConfigMgr 2012 SP1 CU5'
WHEN T1.Client_Version0 = '5.00.7958.1000' THEN 'ConfigMgr 2012 R2'
WHEN T1.Client_Version0 = '5.00.7958.1101' THEN 'ConfigMgr 2012 R2 with KB290500'
WHEN T1.Client_Version0 = '5.00.7958.1203' THEN 'ConfigMgr 2012 R2 CU1'
WHEN T1.Client_Version0 = '5.00.7958.1303' THEN 'ConfigMgr 2012 R2 CU2'
WHEN T1.Client_Version0 = '5.00.7958.1401' THEN 'ConfigMgr 2012 R2 CU3'
WHEN T1.Client_Version0 = '5.00.7958.1501' THEN 'ConfigMgr 2012 R2 CU4'
WHEN T1.Client_Version0 = '5.00.7958.1601' THEN 'ConfigMgr 2012 R2 CU5'
WHEN T1.Client_Version0 = '5.00.8239.1000' THEN 'ConfigMgr 2012 R2 SP1'
WHEN T1.Client_Version0 = '5.00.8239.1203' THEN 'ConfigMgr 2012 R2 SP1 CU1'
WHEN T1.Client_Version0 = '5.00.8239.1301' THEN 'ConfigMgr 2012 R2 SP1 CU2'
WHEN T1.Client_Version0 = '5.00.8239.1403' THEN 'ConfigMgr 2012 R2 SP1 CU3'
WHEN T1.Client_Version0 = '5.00.8325.1000' THEN 'ConfigMgr Update 1511'
WHEN T1.Client_Version0 = '5.00.8355.1000' THEN 'ConfigMgr Update 1602'
WHEN T1.Client_Version0 = '5.00.8355.1306' THEN 'ConfigMgr Update 1602 UR1'
WHEN T1.Client_Version0 = '5.00.8412.1007' THEN 'ConfigMgr Update 1606'
WHEN T1.Client_Version0 = '5.00.8412.1307' THEN 'ConfigMgr Update 1606 UR1'
ELSE T1.Client_Version0 END as 'Description',
COUNT(T1.ResourceID) as 'Total',
T1.ResourceID MachineId
FROM v_R_System_Valid  T1
GROUP BY T1.Client_Version0, T1.ResourceID
ORDER BY Total DESC