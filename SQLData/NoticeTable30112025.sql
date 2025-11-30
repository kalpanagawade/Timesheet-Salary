CREATE TABLE EmployeeHolidays (
    HolidayID INT IDENTITY(1,1) PRIMARY KEY,
    HolidayDate DATE NOT NULL,
    HolidayName VARCHAR(100)
);
INSERT INTO EmployeeHolidays (HolidayDate, HolidayName) VALUES
('2024-01-01', 'New Year'),
('2024-01-14', 'Makar Sankranti'),
('2024-01-26', 'Republic Day'),
('2024-03-08', 'Mahashivratri'),
('2024-03-25', 'Holi'),
('2024-03-29', 'Good Friday'),
('2024-04-11', 'Eid-ul-Fitr'),
('2024-05-01', 'Maharashtra Day / Labour Day'),
('2024-06-17', 'Bakri Eid (Eid-al-Adha)'),
('2024-08-15', 'Independence Day'),
('2024-09-07', 'Ganesh Chaturthi'),
('2024-10-02', 'Gandhi Jayanti'),
('2024-10-12', 'Dussehra'),
('2024-11-01', 'Diwali'),
('2024-11-15', 'Guru Nanak Jayanti'),
('2024-12-25', 'Christmas');
INSERT INTO EmployeeHolidays (HolidayDate, HolidayName) VALUES
('2025-01-01', 'New Year'),
('2025-01-14', 'Makar Sankranti'),
('2025-01-26', 'Republic Day'),
('2025-02-26', 'Mahashivratri'),
('2025-03-14', 'Holi'),
('2025-04-18', 'Good Friday'),
('2025-03-31', 'Eid-ul-Fitr'),
('2025-05-01', 'Maharashtra Day / Labour Day'),
('2025-06-07', 'Bakri Eid (Eid-al-Adha)'),
('2025-08-15', 'Independence Day'),
('2025-08-27', 'Ganesh Chaturthi'),
('2025-10-02', 'Gandhi Jayanti'),
('2025-10-01', 'Dussehra'),
('2025-10-20', 'Diwali'),
('2025-11-05', 'Guru Nanak Jayanti'),
('2025-12-25', 'Christmas');

CREATE TABLE NoticeBoard (
    NoticeID INT IDENTITY(1,1) PRIMARY KEY,
    NoticeText VARCHAR(500),
    CreatedDate DATETIME DEFAULT GETDATE()
);

--⚫ Office will remain closed on 25th Dec (25-Dec-2025)
--⚫ Salary will be processed on 5th of every month (20-Nov-2025)
--⚫ New HR portal launched, login required (15-Nov-2025)