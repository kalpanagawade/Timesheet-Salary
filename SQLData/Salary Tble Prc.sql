
USE [kalpana]
GO

/****** Object:  Table [dbo].[Salary]    Script Date: 29-11-2025 23:21:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Salary](
	[SalaryID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](20) NOT NULL,
	[BasicSalary] [decimal](18, 2) NOT NULL,
	[HRA] [decimal](18, 2) NULL,
	[Allowance] [decimal](18, 2) NULL,
	[Deductions] [decimal](18, 2) NULL,
	[Bonus] [decimal](18, 2) NULL,
	[MonthlyNetSalary]  AS ((([BasicSalary]+[HRA])+[Allowance])-[Deductions]),
	[MonthlySalary]  AS (((([BasicSalary]+[HRA])+[Allowance])-[Deductions])/(12)),
	[AnnualIncome]  AS (((([BasicSalary]+[HRA])+[Allowance])-[Deductions])*(12)+isnull([Bonus],(0))),
	[YEAR] [varchar](10) NULL,--  Add Column
PRIMARY KEY CLUSTERED 
(
	[SalaryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


USE [kalpana]
GO
/****** Object:  StoredProcedure [dbo].[PRC_SaveSalary]    Script Date: 27-10-2025 07:17:41 ******/
--PRC_SaveSalary '100003','','300000.00','20000.00','80000.00','50000.00'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[PRC_SaveSalary]
(
    @UserId VARCHAR(20),
    @Bonus DECIMAL(18,2) = NULL,
    @BasicSalary DECIMAL(18,2),
    @HRA DECIMAL(18,2),
    @Allowance DECIMAL(18,2),
    @Deductions DECIMAL(18,2)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Salary WHERE UserId = @UserId)
    BEGIN
        UPDATE Salary
        SET Bonus = @Bonus,
            BasicSalary = @BasicSalary,
            HRA = @HRA,
            Allowance = @Allowance,
            Deductions = @Deductions
        WHERE UserId = @UserId;
    END
    ELSE
    BEGIN
        INSERT INTO Salary (UserId, Bonus, BasicSalary, HRA, Allowance, Deductions)
        VALUES (@UserId, @Bonus, @BasicSalary, @HRA, @Allowance, @Deductions);
    END
END;
