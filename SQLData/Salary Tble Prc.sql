
USE [kalpana]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Salary](
    [SalaryID]       INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId]         VARCHAR(20) NOT NULL,
    
    -- Monthly components
    [BasicSalary]    DECIMAL(18,2) NOT NULL,    -- Monthly basic salary
    [HRA]            DECIMAL(18,2) NULL,       -- Monthly House Rent Allowance
    [Allowance]      DECIMAL(18,2) NULL,       -- Monthly allowance
    [Deductions]     DECIMAL(18,2) NULL,       -- Monthly deductions
    [Bonus]          DECIMAL(18,2) NULL,       -- Yearly bonus (optional)

    -- Computed columns (directly using base columns)
    [MonthlyNetSalary] AS (([BasicSalary] + [HRA] + [Allowance] - [Deductions])),
	[MonthlySalary]  AS (((([BasicSalary]+[HRA])+[Allowance])-[Deductions])/(12)),
    [AnnualIncome]     AS ((([BasicSalary] + [HRA] + [Allowance] - [Deductions]) * 12) + ISNULL([Bonus],0))
)
GO


USE [kalpana]
GO
/****** Object:  StoredProcedure [dbo].[PRC_SaveSalary]    Script Date: 27-10-2025 07:17:41 ******/
--PRC_SaveSalary '100003','','300000.00','20000.00','80000.00','50000.00'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[PRC_SaveSalary]
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
