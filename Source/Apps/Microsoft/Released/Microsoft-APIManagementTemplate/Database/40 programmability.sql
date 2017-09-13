SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

CREATE PROCEDURE EdgeTablesSwap
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    BEGIN TRANSACTION
        EXEC sp_rename @objname='CallExtendedEdgeList', @newname='CallExtendedEdgeList_TEMP'
        EXEC sp_rename @objname='CallExtendedEdgeList_STAGE', @newname='CallExtendedEdgeList'
        EXEC sp_rename @objname='CallExtendedEdgeList_TEMP', @newname='CallExtendedEdgeList_STAGE'

        EXEC sp_rename @objname='CallProbabilityEdgeList', @newname='CallProbabilityEdgeList_TEMP'
        EXEC sp_rename @objname='CallProbabilityEdgeList_STAGE', @newname='CallProbabilityEdgeList'
        EXEC sp_rename @objname='CallProbabilityEdgeList_TEMP', @newname='CallProbabilityEdgeList_STAGE'
        TRUNCATE TABLE dbo.CallExtendedEdgeList_STAGE
        TRUNCATE TABLE dbo.CallProbabilityEdgeList_STAGE
    COMMIT
END
go

CREATE PROCEDURE FFTTableSwap
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    BEGIN TRANSACTION
        EXEC sp_rename @objname='FFT', @newname='FFT_TEMP'
        EXEC sp_rename @objname='FFT_STAGE', @newname='FFT'
        EXEC sp_rename @objname='FFT_TEMP', @newname='FFT_STAGE'
        TRUNCATE TABLE dbo.FFT_STAGE
    COMMIT
END
go

CREATE PROCEDURE [dbo].[GetDistinctIpAddressesInWindow]
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
    SET NOCOUNT ON

    -- Insert statements for procedure here
    SELECT TOP (@MaxRows) IPAddress
    FROM 
        Request
    WHERE
        IPAddress IS NOT NULL
        AND RequestId IS NOT NULL
        AND Product IS NOT NULL
        AND Operation IS NOT NULL
        AND Api IS NOT NULL
        AND CreatedDate >= @StartDate
        AND CreatedDate < @EndDate
        GROUP BY IPAddress
    ORDER BY COUNT(IPAddress) DESC
END
go

CREATE PROCEDURE GetRequestsByIpAddressInWindow
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
    SET NOCOUNT ON

    -- Insert statements for procedure here
    SELECT TOP 100000
        Id,
        RequestId,
        Product,
        Operation,
        Api,
        CreatedDate,
        IPAddress 
    FROM 
        Request
    WHERE
        IPAddress IS NOT NULL
        AND RequestId IS NOT NULL
        AND Product IS NOT NULL
        AND Operation IS NOT NULL
        AND Api IS NOT NULL
        AND IPAddress = @IpAddress
        AND CreatedDate >= @StartDate
        AND CreatedDate < @EndDate
    ORDER BY CreatedDate ASC
END
go
