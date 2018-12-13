SET NOCOUNT ON;

SELECT DISTINCT ds.collectionid,
                ds.collectionname,
                ds.offerid,
                ds.softwarename,
                ds.programname,
                ds.numbersuccess,
                ds.numberinprogress,
                ds.numberunknown,
                ds.numbererrors,
                ds.numberother,
                ds.numbertotal,
                ds.packageid
FROM   v_deploymentsummary ds
WHERE  ds.featuretype = 2  


