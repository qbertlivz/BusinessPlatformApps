SET NOCOUNT ON;

SELECT DISTINCT Substring(sta.servernalpath, Charindex('\\', sta.servernalpath) + 2, Charindex('"]', sta.servernalpath) -
                                                                                     Charindex('\\', sta.servernalpath) - 3) AS
                DPServer,
                sta.packageid,
                pck.NAME,
                sta.sitecode,
                sta.installstatus
FROM   dbo.v_packagestatusdistpointssumm sta INNER JOIN v_package pck ON sta.packageid = pck.packageid  