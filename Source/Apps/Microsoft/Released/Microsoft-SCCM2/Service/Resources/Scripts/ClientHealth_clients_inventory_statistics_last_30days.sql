SET NOCOUNT ON

DECLARE @TotalRecords 				INT -- Total machine records   
DECLARE @Clients 					INT -- Total client records  
DECLARE @NoClients 					INT -- Total no client records   
DECLARE @Obsolete 					INT -- Total obsolete client records   
DECLARE @ValidClients 				INT -- Total valid client records   
DECLARE @ActiveClientsCurrent		INT -- Total currently active client records  
DECLARE @InactiveClientsCurrent		INT -- Total currently inactive clients
DECLARE @ActiveClientsLast30Days	INT -- Total clients that were active during the last 30 days
DECLARE @HwSuccessLast30Days 		INT -- Total clients with HW inventory Last 30 Days
DECLARE @HwNotSuccessLast30Days 	INT -- Total clients without HW inventory Last 30 Days   
DECLARE @SwSuccessLast30Days 		INT -- Total clients with SW inventory Last 30 Days  
DECLARE @SwNotSuccessLast30Days 	INT -- Total clients without SW inventory Last 30 Days
DECLARE @HWInvMissing 				INT -- Total clients missing HW inventory
DECLARE @SWInvMissing 				INT -- Total clients missing SW inventory

SELECT @TotalRecords = Count(*),
       @Clients = Sum(CASE client0
                        WHEN 1 THEN 1
                        ELSE 0
                      END),
       @NoClients = Sum(CASE
                          WHEN client0 IS NULL THEN 1
                          ELSE 0
                        END),
       @Obsolete = Sum(CASE obsolete0
                         WHEN 1 THEN 1
                         ELSE 0
                       END)
FROM   v_r_system;

SELECT @ValidClients = Count(*),
       @ActiveClientsCurrent = Sum(CASE clientactivestatus
                                     WHEN 1 THEN 1
                                     ELSE 0
                                   END),
       @InactiveClientsCurrent = Sum(CASE clientactivestatus
                                       WHEN 0 THEN 1
                                       ELSE 0
                                     END),
       @ActiveClientsLast30Days = Sum(CASE
                                        WHEN ( Datediff(dd, chsum.lastactivetime, Getdate()) < 30 ) THEN 1
                                        ELSE 0
                                      END),
       @ActiveClientsLast30Days = Sum(CASE
                                        WHEN ( Datediff(dd, chsum.lastactivetime, Getdate()) < 30 ) THEN 1
                                        ELSE 0
                                      END),
       @HwSuccessLast30Days = Sum(CASE
                                    WHEN ( Datediff(dd, chsum.lasthw, Getdate()) < 30 ) AND ( Datediff(dd, chsum.lastactivetime, Getdate()) < 30 ) THEN 1
                                    ELSE 0
                                  END),
       @HwNotSuccessLast30Days = Sum(CASE
                                       WHEN ( ( Datediff(dd, chsum.lasthw, Getdate()) > 30 ) OR chsum.lasthw IS NULL ) AND ( Datediff(dd, chsum.lastactivetime, Getdate()) < 30 ) THEN 1
                                       ELSE 0
                                     END),
       @SwSuccessLast30Days = Sum(CASE
                                    WHEN ( Datediff(dd, chsum.lastsw, Getdate()) < 30 )  AND ( Datediff(dd, chsum.lastactivetime, Getdate()) < 30 ) THEN 1
                                    ELSE 0
                                  END),
       @SwNotSuccessLast30Days = Sum(CASE
                                       WHEN ( ( Datediff(dd, chsum.lastsw, Getdate()) > 30 ) OR chsum.lastsw IS NULL ) AND ( Datediff(dd, chsum.lastactivetime, Getdate()) < 30 ) THEN 1
                                       ELSE 0
                                     END),
       @HWInvMissing = Sum(CASE
                             WHEN chsum.lasthw IS NULL THEN 1
                             ELSE 0
                           END),
       @SWInvMissing = Sum(CASE
                             WHEN chsum.lastsw IS NULL THEN 1
                             ELSE 0
                           END)
FROM   v_ch_clientsummary chsum INNER JOIN v_r_system sysval ON chsum.resourceid = sysval.resourceid;

SELECT @TotalRecords                                               AS TotalRecords,
       @Clients                                                    AS Clients,
       Round(100.00 * @Clients / @TotalRecords, 1)                 AS ClientsPct,
       @NoClients                                                  AS NoClients,
       Round(100.00 * @NoClients / @TotalRecords, 1)               AS NoClientsPct,
       @Obsolete                                                   AS Obsolete,
       Round(100.00 * @Obsolete / @TotalRecords, 1)                AS ObsoletePct,
       @ValidClients                                               AS ValidClients,
       Round(100.00 * @ValidClients / @TotalRecords, 1)            AS ValidClientsPct,
       @ActiveClientsCurrent                                       AS ActiveClientsCurrent,
       Round(100.00 * @ActiveClientsCurrent / @ValidClients, 1)    AS ActiveClientsCurrentPct,
       @InactiveClientsCurrent                                     AS InactiveClientsCurrent,
       Round(100.00 * @InactiveClientsCurrent / @ValidClients, 1)  AS InactiveClientsCurrentPct,
       @ActiveClientsLast30Days                                    AS ActiveClientsLast30Days,
       Round(100.00 * @ActiveClientsLast30Days / @ValidClients, 1) AS ActiveClientsLast30DaysPct,
       @HwSuccessLast30Days                                        AS HwSuccessLast30Days,
       Round(100.00 * @HwSuccessLast30Days / @ValidClients, 1)     AS HwSuccessLast30DaysPct,
       @HwNotSuccessLast30Days                                     AS HwNotSuccessLast30Days,
       Round(100.00 * @HwNotSuccessLast30Days / @ValidClients, 1)  AS HwNotSuccessLast30DaysPct,
       @SwSuccessLast30Days                                        AS SwSuccessLast30Days,
       Round(100.00 * @SwSuccessLast30Days / @ValidClients, 1)     AS SwSuccessLast30DaysPct,
       @SwNotSuccessLast30Days                                     AS SwNotSuccessLast30Days,
       Round(100.00 * @SwNotSuccessLast30Days / @ValidClients, 1)  AS SwNotSuccessLast30DaysPct,
       @HWInvMissing                                               AS HWInvMissing,
       Round(100.00 * @HWInvMissing / @ValidClients, 1)            AS HWInvMissingPct,
       @SWInvMissing                                               AS SWInvMissing,
       Round(100.00 * @SWInvMissing / @ValidClients, 1)            AS SWInvMissingPct;
