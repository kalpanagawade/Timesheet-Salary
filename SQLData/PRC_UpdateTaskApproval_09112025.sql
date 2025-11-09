CREATE PROCEDURE PRC_UpdateTaskApproval
    @TaskID INT,
    @IsApproved BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE TaskEntries
    SET IsApproval = CASE WHEN @IsApproved = 1 THEN 'Y' ELSE 'N' END
    WHERE TaskID = @TaskID;
END;
GO


CREATE PROCEDURE PRC_UpdateMultipleTaskApproval
    @TaskIDs NVARCHAR(MAX),
    @UserID NVARCHAR(20),
    @IsApproved CHAR(1)
AS
BEGIN
    SET NOCOUNT ON;

    -- Split comma-separated TaskIDs into a table variable
    DECLARE @IDs TABLE (TaskID INT);
    DECLARE @xml XML = '<i>' + REPLACE(@TaskIDs, ',', '</i><i>') + '</i>';

    INSERT INTO @IDs (TaskID)
    SELECT t.value('.', 'INT') FROM @xml.nodes('/i') AS x(t);

    -- Update approval status for all selected TaskIDs for this user
    UPDATE T
    SET T.IsApproval = @IsApproved
    FROM TaskEntries T
    INNER JOIN @IDs I ON T.TaskID = I.TaskID
    WHERE T.UserID = @UserID;
END
