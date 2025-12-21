select * from Salary
--drop table  salary 

CREATE TABLE Salary (
    UserId VARCHAR(50),
    Year INT,
    BasicSalary DECIMAL(10,2),
    HRA DECIMAL(10,2),
    Allowance DECIMAL(10,2),
    Bonus DECIMAL(10,2),
    AnnualIncome DECIMAL(12,2),
    MonthlySalary DECIMAL(10,2),
    PRIMARY KEY (UserId, Year)
);

CREATE TABLE SalaryMonthly (
    UserId VARCHAR(50),
    Year INT,
    Month INT,
    GrossSalary DECIMAL(10,2),
    LeaveDays INT,
    Deduction DECIMAL(10,2),
    NetSalary DECIMAL(10,2),
    IsReleased BIT DEFAULT 0,
    ReleasedOn DATETIME,
    PRIMARY KEY (UserId, Year, Month)
);


alter PROCEDURE PRC_SaveSalary
(
 @UserId VARCHAR(50),
 @Year INT,
 @BasicSalary DECIMAL(10,2),
 @HRA DECIMAL(10,2),
 @Allowance DECIMAL(10,2),
 @Bonus DECIMAL(10,2)
)
AS
BEGIN
 DECLARE @Annual DECIMAL(12,2)
 DECLARE @Monthly DECIMAL(10,2)

 SET @Annual = (@BasicSalary + @HRA + @Allowance + @Bonus) * 12
 SET @Monthly = @Annual / 12

 IF EXISTS (SELECT 1 FROM Salary WHERE UserId=@UserId AND Year=@Year)
 BEGIN
   UPDATE Salary SET
    BasicSalary=@BasicSalary,
    HRA=@HRA,
    Allowance=@Allowance,
    Bonus=@Bonus,
    AnnualIncome=@Annual,
    MonthlySalary=@Monthly
   WHERE UserId=@UserId AND Year=@Year
 END
 ELSE
 BEGIN
   INSERT INTO Salary VALUES
   (@UserId,@Year,@BasicSalary,@HRA,@Allowance,@Bonus,@Annual,@Monthly)
 END
END


CREATE PROCEDURE PRC_ReleaseMonthlySalary
(
 @UserId VARCHAR(50),
 @Year INT,
 @Month INT,
 @LeaveDays INT,
 @Deduction DECIMAL(10,2)
)
AS
BEGIN
 DECLARE @Gross DECIMAL(10,2)
 DECLARE @Net DECIMAL(10,2)

 SELECT @Gross = MonthlySalary
 FROM Salary WHERE UserId=@UserId AND Year=@Year

 SET @Net = @Gross - @Deduction

 IF NOT EXISTS (
   SELECT 1 FROM SalaryMonthly
   WHERE UserId=@UserId AND Year=@Year AND Month=@Month
 )
 BEGIN
   INSERT INTO SalaryMonthly
   VALUES
   (@UserId,@Year,@Month,@Gross,@LeaveDays,@Deduction,@Net,1,GETDATE())
 END
END



