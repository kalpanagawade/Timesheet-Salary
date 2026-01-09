USE [kalpana]
GO

/****** Object:  Table [dbo].[UserLogTimes]    Script Date: 1/9/2026 11:47:23 PM ******/
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
	[Work_Place] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [kalpana]
GO
/****** Object:  StoredProcedure [dbo].[PRC_InsertLoginLogout]    Script Date: 1/9/2026 11:12:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[PRC_InsertLoginLogout]
    @UserId VARCHAR(20),
    @Action VARCHAR(10),   -- 'LOGIN' or 'LOGOUT'
    @LogDate DATE = NULL,
	@In_Time datetime = NULL,
	@Out_Time datetime = NULL,
	@Work_Place varchar(20)
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
            INSERT INTO UserLogTimes (UserId, LogDate, In_Time,Work_Place)
            VALUES (@UserId, @LogDate, GETDATE(),@Work_Place);
        END
    END
    ELSE IF @Action = 'LOGOUT'
    BEGIN
        -- Update only if user has open login
        UPDATE UserLogTimes
        SET Out_Time = GETDATE(),Work_Place= @Work_Place
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
            INSERT INTO UserLogTimes (UserId, LogDate, In_Time, Out_Time,Work_Place)
            VALUES (@UserId, @LogDate, @In_Time, @Out_Time,@Work_Place);
        END
	END

END;



