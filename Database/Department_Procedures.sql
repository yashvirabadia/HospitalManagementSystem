--PR_DEPT_Department_Selectall
--CREATE OR ALTER PROC PR_DEPT_Department_SelectAll
--AS
--BEGIN
--    SELECT 
--        [dbo].Department.DepartmentID
--        ,[dbo].Department.DepartmentName
--        ,[dbo].Department.[Description]
--        ,[dbo].Department.IsActive
--        ,[dbo].Department.Created
--        ,[dbo].Department.Modified
--        ,[dbo].Department.UserID
--    FROM [dbo].Department
--END
CREATE OR ALTER PROCEDURE PR_DEPT_Department_SelectAll
AS
BEGIN
    SELECT 
		D.DepartmentID,
		D.DepartmentName,
		D.[Description],
		D.IsActive,
		D.Modified,
		D.UserID,
		D.Created,
		U.UserName,
		U.Email,
		U.MobileNo
	FROM 
	Department AS D 
	JOIN [User] AS U
    ON D.UserID = U.UserID;
END

--PR_DEPT_Department_Insert 'fghj','fffffffffff',1,'1999-05-06','2024-05-06',1
--PR_DEPT_Department_Insert 'Cardiology', 'Heart and blood vessel treatment', 1, '1999-05-06','2024-05-06', 1
--PR_DEPT_Department_Insert 'Dentistry', 'Dental and oral care', 1,  '1999-05-06','2024-05-06', 2
CREATE OR ALTER PROC PR_DEPT_Department_Insert
    @DepartmentName		NVARCHAR(100),
    @Description		NVARCHAR(250),
    @IsActive			BIT,
    @Created			DATETIME,
    @Modified			DATETIME,
    @UserID				INT
AS
BEGIN
    INSERT INTO [dbo].Department 
	(
        [dbo].Department.DepartmentName
        ,[dbo].Department.[Description]
        ,[dbo].Department.IsActive
        ,[dbo].Department.Created
        ,[dbo].Department.Modified
        ,[dbo].Department.UserID
    )
    VALUES 
	(
        @DepartmentName,
        @Description,
        @IsActive,
        @Created,
        @Modified,
        @UserID
    )
END


--PR_DEPT_Department_DeleteByPK 4
CREATE OR ALTER PROC PR_DEPT_Department_DeleteByPK
	@departmentid INT
AS
BEGIN
	DELETE FROM [dbo].Department 
	WHERE [dbo].Department.DepartmentID = @departmentid
END


--PR_DEPT_Department_SelectByPK 2
CREATE OR ALTER PROC PR_DEPT_Department_SelectByPK
	@departmentid INT
AS
BEGIN
	SELECT 
		D.DepartmentID
		,D.DepartmentName
		,D.[Description]
		,D.IsActive
		,D.Created
		,D.Modified
		,D.UserID
		,U.UserName
		,U.Email
		,U.MobileNo
	FROM [dbo].Department AS D JOIN [User] AS U
        ON D.UserID = U.UserID
    WHERE D.DepartmentID = @DepartmentID
END


--PR_DEPT_Department_UpdateByPK 
CREATE OR ALTER PROC PR_DEPT_Department_UpdateByPK
	@DepartmentID		INT,
	@DepartmentName		NVARCHAR(100),
    @Description		NVARCHAR(250),
    @IsActive			BIT,
    @Created			DATETIME,
    @Modified			DATETIME,
    @UserID				INT
AS
BEGIN
	UPDATE [dbo].Department
	SET
		[dbo].Department.DepartmentName = @DepartmentName
		,[dbo].Department.[Description] = @Description		
		,[dbo].Department.IsActive		= @IsActive
		,[dbo].Department.Created		= @Created
		,[dbo].Department.Modified		= @Modified
	WHERE
	[dbo].Department.DepartmentID = @departmentid
END