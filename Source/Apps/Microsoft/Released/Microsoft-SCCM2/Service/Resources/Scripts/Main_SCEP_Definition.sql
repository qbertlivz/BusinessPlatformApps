SET NOCOUNT ON;

select distinct b.ResourceID
, b.Name
, (a.SignatureUpTo1DayOld) AS SignatureUpTo1DayOld
, (a.SignatureUpTo3DaysOld) AS SignatureUpTo3DaysOld
, (a.SignatureUpTo7DaysOld) AS SignatureUpTo7DaysOld
, (a.SignatureOlderThan7Days) AS SignatureOlderThan7Days
, (a.NoSignature) AS NoSignature
, a.EpInstalled
from v_EndpointProtectionStatus AS a
inner join v_FullCollectionMembership_valid AS b on b.ResourceID = a.ResourceID
and a.EpInstalled is not null 
and b.ResourceType <> 2

