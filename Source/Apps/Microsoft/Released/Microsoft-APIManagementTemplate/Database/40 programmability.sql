SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

CREATE PROCEDURE pbist_apimgmt.sp_edgetablesswap
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    EXEC sp_rename @objname='pbist_apimgmt.callextendededgelist', @newname='pbist_apimgmt.callextendededgelist_temp';
    EXEC sp_rename @objname='pbist_apimgmt.callextendededgelist_staging', @newname='pbist_apimgmt.callextendededgelist';
    EXEC sp_rename @objname='pbist_apimgmt.callextendededgelist_temp', @newname='pbist_apimgmt.callextendededgelist_staging';
    DROP TABLE pbist_apimgmt.callextendededgelist_temp;
    TRUNCATE TABLE pbist_apimgmt.callextendededgelist_staging;

    EXEC sp_rename @objname='pbist_apimgmt.callprobabilityedgelist', @newname='pbist_apimgmt.callprobabilityedgelist_temp';
    EXEC sp_rename @objname='pbist_apimgmt.callprobabilityedgelist_staging', @newname='pbist_apimgmt.callprobabilityedgelist';
    EXEC sp_rename @objname='pbist_apimgmt.callprobabilityedgelist_temp', @newname='pbist_apimgmt.callprobabilityedgelist_staging';
    DROP TABLE pbist_apimgmt.callprobabilityedgelist_temp;
    TRUNCATE TABLE pbist_apimgmt.callprobabilityedgelist_staging;
END;
go

CREATE PROCEDURE pbist_apimgmt.sp_ffttableswap
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    EXEC sp_rename @objname='pbist_apimgmt.fft', @newname='pbist_apimgmt.fft_temp';
    EXEC sp_rename @objname='pbist_apimgmt.fft_staging', @newname='pbist_apimgmt.fft';
    EXEC sp_rename @objname='pbist_apimgmt.fft_temp', @newname='pbist_apimgmt.fft_staging';
    DROP TABLE pbist_apimgmt.fft_temp;
    TRUNCATE TABLE pbist_apimgmt.fft_staging;
END;
go

CREATE PROCEDURE pbist_apimgmt.sp_getdistinctipaddressesinwindow
(
    -- Add the parameters for the stored procedure here
    @Start varchar(255) = NULL,
    @End varchar(255) = NULL,
    @MaxRows int = 100
)
AS
BEGIN
    DECLARE @StartDate datetime
    DECLARE @EndDate datetime
    DECLARE @CURRENT_TIME datetime
    SET @CURRENT_TIME = getdate()

    SET @Startdate = coalesce(try_convert(datetime, @Start, 127), dateadd(day, -7, @CURRENT_TIME))
    SET @EndDate = coalesce(try_convert(datetime, @End, 127), @CURRENT_TIME)

    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT TOP (@MaxRows) IPAddress
    FROM 
        pbist_apimgmt.request
    WHERE
        IPAddress IS NOT NULL
        AND RequestId IS NOT NULL
        AND Product IS NOT NULL
        AND Operation IS NOT NULL
        AND Api IS NOT NULL
        AND CreatedDate >= @StartDate
        AND CreatedDate < @EndDate
        GROUP BY IPAddress
    ORDER BY COUNT(IPAddress) DESC;
END;
go

CREATE PROCEDURE pbist_apimgmt.sp_getrequestsbyipaddressinwindow
(
    -- Add the parameters for the stored procedure here
    @IpAddress varchar(20) = NULL,
    @Start varchar(255) = NULL,
    @End varchar(255) = NULL
)
AS
BEGIN
    DECLARE @StartDate datetime
    DECLARE @EndDate datetime
    DECLARE @CURRENT_TIME datetime
    SET @CURRENT_TIME = getdate()

    SET @Startdate = coalesce(try_convert(datetime, @Start, 127), dateadd(day, -7, @CURRENT_TIME))
    SET @EndDate = coalesce(try_convert(datetime, @End, 127), @CURRENT_TIME)

    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT TOP 100000
        RequestId,
        ProductId As Product,
		OperationId As Operation,
		ApiId As Api,
        CreatedDate,
        IPAddress 
    FROM 
        pbist_apimgmt.request
    WHERE
        IPAddress IS NOT NULL
        AND RequestId IS NOT NULL
        AND Product IS NOT NULL
        AND Operation IS NOT NULL
        AND Api IS NOT NULL
        AND IPAddress = @IpAddress
        AND CreatedDate >= @StartDate
        AND CreatedDate < @EndDate
    ORDER BY CreatedDate;
END;
go

CREATE PROCEDURE pbist_apimgmt.sp_fftdataextraction
AS
BEGIN
    SET NOCOUNT ON;
    
	SELECT CreatedDate, IPAddress
	FROM pbist_apimgmt.request
	WHERE IPAddress IS NOT NULL	AND CreatedDate > DATEADD(day, -3, SYSDATETIME())
END;
go
