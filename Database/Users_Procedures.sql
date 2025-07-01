--PR_USR_Users_Selectall
CREATE OR ALTER PROC PR_USR_Users_Selectall
AS
BEGIN
    SELECT 
        [dbo].[User].UserID
        ,[dbo].[User].UserName
        ,[dbo].[User].[Password]
		,[dbo].[User].Email
		,[dbo].[User].MobileNo
        ,[dbo].[User].IsActive
        ,[dbo].[User].Created
        ,[dbo].[User].Modified
    FROM [dbo].[User];
END

--PR_USR_Users_Insert 'drashti','cvbnm','drashti@com','456321',1,'2024-05-01','2025-02-01'
CREATE OR ALTER PROC PR_USR_Users_Insert
     @UserName		nvarchar(100)
    ,@Password		nvarchar(100)
	,@Email			nvarchar(100)
	,@MobileNo		nvarchar(100)
    ,@IsActive		bit
    ,@Created		datetime
    ,@Modified		datetime			
AS
BEGIN
    INSERT INTO  [dbo].[User]
	(
		 [dbo].[User].UserName
		,[dbo].[User].[Password]
		,[dbo].[User].Email		
		,[dbo].[User].MobileNo
		,[dbo].[User].IsActive
		,[dbo].[User].Created
		,[dbo].[User].Modified
    )
    VALUES 
	(
		@UserName
		,@Password
		,@Email		
		,@MobileNo
		,@IsActive
		,@Created
		,@Modified
    )
END


--
CREATE OR ALTER PROC PR_USR_Users_DeleteByPK
    @UserID INT
AS
BEGIN
    DELETE FROM [dbo].[User]
    WHERE [dbo].[User].UserID = @UserID
END



--PR_USR_Users_SelectByID 1
CREATE OR ALTER PROC PR_USR_Users_SelectByPK
	@UserID INT
AS
BEGIN
	SELECT 
		[dbo].[User].UserID		
		,[dbo].[User].UserName
		,[dbo].[User].[Password]
		,[dbo].[User].Email		
		,[dbo].[User].MobileNo
		,[dbo].[User].IsActive
		,[dbo].[User].Created
		,[dbo].[User].Modified
	FROM [dbo].[User]
	WHERE [dbo].[User].UserID = @UserID
END


--
CREATE OR ALTER PROC PR_USR_Users_UpdateByPK
    @UserID     INT,
    @UserName   NVARCHAR(100),
    @Password   NVARCHAR(100),
    @Email      NVARCHAR(100),
    @MobileNo   NVARCHAR(100),
    @IsActive   BIT,
    @Created    DATETIME,
    @Modified   DATETIME
AS
BEGIN

    UPDATE [dbo].[User]
    SET 
        [dbo].[User].UserName   = @UserName
        ,[dbo].[User].[Password] = @Password 
        ,[dbo].[User].Email      = @Email
        ,[dbo].[User].MobileNo   = @MobileNo
        ,[dbo].[User].IsActive   = @IsActive
        ,[dbo].[User].Created    = @Created
        ,[dbo].[User].Modified   = @Modified
    FROM [dbo].[User] 
    WHERE [dbo].[User].UserID = @UserID;
END


