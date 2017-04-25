SET ANSI_NULLS              ON;
SET ANSI_PADDING            ON;
SET ANSI_WARNINGS           ON;
SET ANSI_NULL_DFLT_ON       ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER       ON;
go

/************************************
* Date values                       *
*************************************/
DECLARE @startDate as date
DECLARE @endDate as date
DECLARE @curDate as date
DECLARE @dayName as nvarchar(50)
DECLARE @dayAbbrev as nvarchar(10)
DECLARE @weekDayFlag as char(1)

DECLARE @dateKey as int
DECLARE @dayOfWeek as int
DECLARE @dayOfMonth as int
DECLARE @weekNoOfYear as int
DECLARE @weekBeginDate as date
DECLARE @monthNo as tinyint
DECLARE @monthName as nvarchar(50)
DECLARE @monthAbbrev as nvarchar(10)
DECLARE @quarter as tinyint
DECLARE @year as smallint
DECLARE @yearmo as int
DECLARE @sameDayYearAgo as date

-- Go back 3 years from the first day of the current year
SET @startDate = dateadd(yy, -3, dateadd(dd, 1-datepart(dy, getdate()), getdate()))

-- Go forward 3 years from the last day of the current year
SET @endDate = dateadd(yy, 4, dateadd(dd, -datepart(dy, getdate()), getdate()))

SET @curDate = @startDate

WHILE @curDate <= @endDate
BEGIN
    SET @dateKey = datepart(yyyy,@curDate)*10000 + datepart(mm,@curdate)*100 + datepart(dd, @curdate)
    SET @dayOfWeek = datepart(dw,@curDate)
    SET @dayOfMonth = day(@curDate)
    SET @dayName =  datename(dw,@curDate)
    SET @dayAbbrev =  left(@dayName,3)
    SET @weekDayFlag = CASE WHEN (@@DATEFIRST+@dayOfWeek) % 7 <2  THEN 'y' ELSE 'n' END
    SET @weekNoOfYear = datepart(wk,@curDate)
    SET @weekBeginDate = dateadd(dd,-@dayOfWeek+1, @curDate)
    SET @monthNo = datepart(m, @curDate)
    SET @monthName = datename(mm, @curDate)
    SET @monthAbbrev = Left(@monthName, 3)
    SET @quarter = datepart(q, @curdate)
    SET @year = datepart(yy, @curdate)
    SET @yearmo = @year*100+@monthNo
    SET @sameDayYearAgo = dateadd(yy,-1,@curDate)

    -- Do the actual insert
    INSERT smgt.[date] ([date_key], [full_date], [day_of_week], [day_num_in_month], [day_name], [day_abbrev]
    ,[weekday_flag], [week_num_in_year], [week_begin_date], [month], [month_name],
    [month_abbrev], [quarter], [year],  [yearmo], same_day_year_ago_date)  
    VALUES (@dateKey,@curDate, @dayOfWeek, @dayOfMonth, @dayName, @dayAbbrev
    ,@weekDayFlag, @weekNoOfYear, @weekBeginDate,  @monthNo, @monthName
    ,@monthAbbrev, @quarter, @year, @yearmo,@sameDayYearAgo )

    -- Go to the next day
    SET @CurDate = dateadd(dd,1,@curDate)
END


/************************************
* Configuration values              *
*************************************/
INSERT smgt.[configuration] (configuration_group, configuration_subgroup, [name], [value]) VALUES (N'data', N'actual_sales', N'enabled', N'0');
INSERT smgt.[configuration] (configuration_group, configuration_subgroup, [name], [value], [visible]) VALUES (N'SolutionTemplate', N'SalesManagement', N'FiscalMonthStart', N'1', 1);
INSERT smgt.[configuration] (configuration_group, configuration_subgroup, [name], [value], [visible]) VALUES (N'SolutionTemplate', N'SalesManagement', N'version', N'1.0', 0);
INSERT smgt.[configuration] (configuration_group, configuration_subgroup, [name], [value], [visible]) VALUES (N'SolutionTemplate', N'SalesManagement', N'versionImage', N'', 0);
go
