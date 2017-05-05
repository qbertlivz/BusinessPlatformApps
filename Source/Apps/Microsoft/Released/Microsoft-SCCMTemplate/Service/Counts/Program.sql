SET NOCOUNT ON;

SELECT Count(*)
FROM
    (SELECT DISTINCT displayname0, publisher0, version0
     FROM
        dbo.v_Add_Remove_Programs
     WHERE
        displayname0 IS NOT NULL AND displayname0 <> ''
    ) T;