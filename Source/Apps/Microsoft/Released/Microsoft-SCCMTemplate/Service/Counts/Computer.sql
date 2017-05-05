SET NOCOUNT ON;

SELECT Count(*)
FROM
    dbo.vsms_r_system
WHERE
    decommissioned0=0 AND obsolete0=0 AND name0 NOT LIKE N'%|%';
