CREATE OR ALTER PROCEDURE PR_DOCDEP_DoctorDepartment_SelectAll
AS
BEGIN
    SELECT 
        DD.DoctorDepartmentID,
        DD.DoctorID,
        DD.DepartmentID,
        DD.Created,
        DD.Modified,
        DD.UserID,
        U.UserName,
        U.Email,
        U.MobileNo
    FROM DoctorDepartment AS DD
    JOIN [User] AS U ON DD.UserID = U.UserID;
END




-- EXEC PR_DOCDEP_DoctorDepartment_Insert 2, 2, '2024-06-01', '2024-06-22', 1
CREATE OR ALTER PROCEDURE PR_DOCDEP_DoctorDepartment_Insert
    @DoctorID       INT,
    @DepartmentID   INT,
    @Created        DATETIME,
    @Modified       DATETIME,
    @UserID         INT
AS
BEGIN
    INSERT INTO [dbo].DoctorDepartment
    (
        DoctorID,
        DepartmentID,
        Created,
        Modified,
        UserID
    )
    VALUES
    (
        @DoctorID,
        @DepartmentID,
        @Created,
        @Modified,
        @UserID
    )
END




-- Example: EXEC PR_DOCDEP_DoctorDepartment_DeleteByPK 1
CREATE OR ALTER PROCEDURE PR_DOCDEP_DoctorDepartment_DeleteByPK
    @DoctorDepartmentID INT
AS
BEGIN
    DELETE FROM [dbo].DoctorDepartment
    WHERE DoctorDepartmentID = @DoctorDepartmentID
END



-- Example: EXEC PR_DOCDEP_DoctorDepartment_SelectByPK 2
CREATE OR ALTER PROCEDURE PR_DOCDEP_DoctorDepartment_SelectByPK
    @DoctorDepartmentID INT
AS
BEGIN
    SELECT 
        DD.DoctorDepartmentID,
        DD.DoctorID,
        DD.DepartmentID,
        DD.Created,
        DD.Modified,
        DD.UserID,
        U.UserName,
        U.Email,
        U.MobileNo
    FROM DoctorDepartment AS DD
    JOIN [User] AS U ON DD.UserID = U.UserID
    WHERE DD.DoctorDepartmentID = @DoctorDepartmentID
END





-- EXEC PR_DOCDEP_DoctorDepartment_UpdateByPK 3, 1, 2, '2024-06-01', '2024-06-25', 4
CREATE OR ALTER PROCEDURE PR_DOCDEP_DoctorDepartment_UpdateByPK
    @DoctorDepartmentID INT,
    @DoctorID           INT,
    @DepartmentID       INT,
    @Created            DATETIME,
    @Modified           DATETIME,
    @UserID             INT
AS
BEGIN
    UPDATE [dbo].DoctorDepartment
    SET
        DoctorID     = @DoctorID,
        DepartmentID = @DepartmentID,
        Created      = @Created,
        Modified     = @Modified,
        UserID       = @UserID
    WHERE DoctorDepartmentID = @DoctorDepartmentID
END
