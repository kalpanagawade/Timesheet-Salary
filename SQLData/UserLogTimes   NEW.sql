USE [kalpana]
GO
/****** Object:  Table [dbo].[UserLogTimes]    Script Date: 28-09-2025 13:16:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLogTimes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LogDate] [date] NOT NULL,
	[In_Time] [datetime] NULL,
	[Out_Time] [datetime] NULL,
	[UserId] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[UserLogTimes] ON 
GO
INSERT [dbo].[UserLogTimes] ([Id], [LogDate], [In_Time], [Out_Time], [UserId]) VALUES (1, CAST(N'2025-09-21' AS Date), CAST(N'2025-09-21T16:58:17.187' AS DateTime), CAST(N'2025-09-21T17:19:05.887' AS DateTime), N'100000')
GO
INSERT [dbo].[UserLogTimes] ([Id], [LogDate], [In_Time], [Out_Time], [UserId]) VALUES (3, CAST(N'2025-09-20' AS Date), CAST(N'2025-09-20T12:58:17.187' AS DateTime), CAST(N'2025-09-20T13:19:05.887' AS DateTime), N'100000')
GO
INSERT [dbo].[UserLogTimes] ([Id], [LogDate], [In_Time], [Out_Time], [UserId]) VALUES (5, CAST(N'2025-09-19' AS Date), CAST(N'2025-09-19T12:00:00.000' AS DateTime), CAST(N'2025-09-19T13:00:00.000' AS DateTime), N'100000')
GO
INSERT [dbo].[UserLogTimes] ([Id], [LogDate], [In_Time], [Out_Time], [UserId]) VALUES (6, CAST(N'2025-09-18' AS Date), CAST(N'2025-09-18T01:00:00.000' AS DateTime), CAST(N'2025-09-18T02:00:00.000' AS DateTime), N'100000')
GO
INSERT [dbo].[UserLogTimes] ([Id], [LogDate], [In_Time], [Out_Time], [UserId]) VALUES (8, CAST(N'2025-09-28' AS Date), CAST(N'2025-09-28T12:56:36.360' AS DateTime), CAST(N'2025-09-28T12:57:07.730' AS DateTime), N'100000')
GO
SET IDENTITY_INSERT [dbo].[UserLogTimes] OFF
GO
/****** Object:  StoredProcedure [dbo].[Ins_Map_designation_UserID_Data]    Script Date: 28-09-2025 13:16:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Ins_Map_designation_UserID_Data]
@UserId varchar(20),
@Des_Name varchar(100)

AS
BEGIN
Declare @getDate datetime,@Des_ID int
set @getDate=getdate()
select @Des_ID=Des_ID from MST_designation where Des_Name=@Des_Name------Wrong Statement 
IF EXISTS (SELECT * FROM MAP_designation_UserId WHERE UserId = @UserId)
BEGIN
    UPDATE MAP_designation_UserId
    SET Des_ID = @Des_Name
    WHERE UserId = @UserId
END
ELSE
BEGIN
    INSERT INTO MAP_designation_UserId (UserId, Des_ID)
    VALUES (@UserId, @Des_Name)
END


END
GO
/****** Object:  StoredProcedure [dbo].[PRC_InsertLoginLogout]    Script Date: 28-09-2025 13:16:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PRC_InsertLoginLogout]
    @UserId VARCHAR(20),
    @Action VARCHAR(10),   -- 'LOGIN' or 'LOGOUT'
    @LogDate DATE = NULL,
	@In_Time datetime = NULL,
	@Out_Time datetime = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @LogDate IS NULL
        SET @LogDate = CAST(GETDATE() AS DATE);

    IF @Action = 'LOGIN'
    BEGIN
        -- If no open login for today, insert one
        IF NOT EXISTS (
            SELECT 1 FROM UserLogTimes
            WHERE UserId = @UserId AND LogDate = @LogDate AND Out_Time IS NULL
        )
        BEGIN
            INSERT INTO UserLogTimes (UserId, LogDate, In_Time)
            VALUES (@UserId, @LogDate, GETDATE());
        END
    END
    ELSE IF @Action = 'LOGOUT'
    BEGIN
        -- Update only if user has open login
        UPDATE UserLogTimes
        SET Out_Time = GETDATE()
        WHERE UserId = @UserId
          AND LogDate = @LogDate
          AND Out_Time IS NULL;
    END
	ELSE IF @Action = 'Both'
	BEGIN
		IF NOT EXISTS (
            SELECT 1 FROM UserLogTimes
            WHERE UserId = @UserId AND LogDate = @LogDate 
        )
        BEGIN
            INSERT INTO UserLogTimes (UserId, LogDate, In_Time, Out_Time)
            VALUES (@UserId, @LogDate, @In_Time, @Out_Time);
        END
	END

END;
GO
