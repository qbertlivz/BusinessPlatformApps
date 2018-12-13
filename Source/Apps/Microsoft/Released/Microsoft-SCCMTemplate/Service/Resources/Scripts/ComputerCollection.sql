SET NOCOUNT ON;

SELECT cm.SiteID AS 'CollectionID', cm.MachineID AS 'ResourceID'
FROM
    dbo.CollectionMembers cm LEFT OUTER JOIN dbo.System_DISC sd ON cm.MachineID=sd.ItemKey AND cm.ArchitectureKey=5
WHERE
	sd.Obsolete0 IS NULL OR sd.Obsolete0=0;
