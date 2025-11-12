USE [kalpana]
GO
/****** Object:  StoredProcedure [dbo].[PRC_GetCalendarStatus]    Script Date: 11/12/2025 10:51:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[PRC_GetCalendarStatus]
    @Year INT,
    @Month INT,
    @UserId VARCHAR(20) = Null
AS
BEGIN
    SET NOCOUNT ON;
	--If @UserId IS NULL
	--begin 
	--select DAY(StatusDate) AS DayNumber,
 --       StatusType from CalendarStatus where userId='100000'
	--end
 --   else 
	--begin 
	

	CREATE TABLE #CalendarStatus(
	[StatusDate] [date] NOT NULL,
	[StatusType] [varchar](50) NULL,
	[UserId] [varchar](20) NULL)

	insert into #CalendarStatus 
	SELECT  TaskDate,
    CASE 
        WHEN IsApproval = 'Y' THEN 'Approved'
		WHEN IsApproval = 'N' THEN 'NotApproved'
        WHEN IsPending = 'Y' THEN 'Pending'		
		WHEN IsHoliday = 'Y' THEN 'Holiday'
		WHEN IsLeave = 'Y' THEN 'Leave'
		WHEN IsWeeklyOff = 'Y' THEN 'Weekly Off'
		WHEN IsCompOff = 'Y' THEN 'CompOff'
		WHEN IsInYourBucket = 'Y' THEN 'In your bucket'
    END AS StatusType,
    UserId
FROM TaskEntries;

	SELECT 
        DAY(StatusDate) AS DayNumber,
        StatusType
    FROM 
        #CalendarStatus
    WHERE 
        YEAR(StatusDate) = @Year 
        AND MONTH(StatusDate) = @Month
        AND UserId = @UserId;

		drop table #CalendarStatus
	--end 
END
