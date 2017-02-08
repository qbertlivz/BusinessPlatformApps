SET NOCOUNT ON;

SELECT DISTINCT ds.collectionid,
                ds.collectionname,
                ds.offerid,
                ds.softwarename,
                ds.softwarename AS 'Application',
                ds.numbersuccess,
                ds.numberinprogress,
                ds.numberunknown,
                ds.numbererrors,
                ds.numberother,
                ds.numbertotal,
                ds.packageid
FROM   v_deploymentsummary ds
WHERE  ds.featuretype = 1

