--PR_DOC_Doctor_SelectAll;
CREATE OR ALTER PROC PR_DOC_Doctor_SelectAll
AS
BEGIN
    SELECT 
        D.DoctorID
        ,D.[Name]
        ,D.Phone
        ,D.Email
        ,D.Qualification
        ,D.Specialization
        ,D.IsActive
        ,D.Created
        ,D.Modified
        ,D.UserID
		,U.UserName
		,U.Email
		,U.MobileNo
    FROM [dbo].Doctor AS D 
	JOIN [dbo].[User] AS U
    ON D.UserID = U.UserID;
END



--PR_DOC_Doctor_Insert 'Dr. Smith','9999999999','smith@example.com','MBBS','Cardiology',1,'2024-01-01','2024-06-01',1
CREATE OR ALTER PROC PR_DOC_Doctor_Insert
    @Name            NVARCHAR(100),
    @Phone           NVARCHAR(20),
    @Email           NVARCHAR(100),
    @Qualification   NVARCHAR(100),
    @Specialization  NVARCHAR(100),
    @IsActive        BIT,
    @Created         DATETIME,
    @Modified        DATETIME,
    @UserID          INT
AS
BEGIN
    INSERT INTO [dbo].Doctor (
        [dbo].Doctor.[Name]
        ,[dbo].Doctor.Phone
        ,[dbo].Doctor.Email
        ,[dbo].Doctor.Qualification
        ,[dbo].Doctor.Specialization
        ,[dbo].Doctor.IsActive
        ,[dbo].Doctor.Created
        ,[dbo].Doctor.Modified
        ,[dbo].Doctor.UserID
    )
    VALUES (
        @Name,
        @Phone,
        @Email,
        @Qualification,
        @Specialization,
        @IsActive,
        @Created,
        @Modified,
        @UserID
    )
END



-- PR_DOC_Doctor_DeleteByPK 2
CREATE OR ALTER PROC PR_DOC_Doctor_DeleteByPK
    @DoctorID INT
AS
BEGIN
    DELETE FROM [dbo].Doctor
    WHERE [dbo].Doctor.DoctorID = @DoctorID
END



-- Example Call:
-- PR_DOC_Doctor_SelectByPK 2
CREATE OR ALTER PROC PR_DOC_Doctor_SelectByPK
    @DoctorID INT
AS
BEGIN
    SELECT 
        D.DoctorID
        ,D.[Name]
        ,D.Phone
        ,D.Email
        ,D.Qualification
        ,D.Specialization
        ,D.IsActive
        ,D.Created
        ,D.Modified
        ,D.UserID
		,U.UserName
		,U.Email
		,U.MobileNo
    FROM Doctor AS D 
	JOIN [User] AS U
    ON D.UserID = U.UserID
    WHERE DoctorID = @DoctorID;
END




-- Example Call:
-- PR_DOC_Doctor_UpdateByPK 3, 'Dr. Shah', '8888888888', 'shah@med.com', 'MD', 'Neurology', 1, '2024-01-01', '2024-06-01', 2
CREATE OR ALTER PROC PR_DOC_Doctor_UpdateByPK
    @DoctorID        INT,
    @Name            NVARCHAR(100),
    @Phone           NVARCHAR(20),
    @Email           NVARCHAR(100),
    @Qualification   NVARCHAR(100),
    @Specialization  NVARCHAR(100),
    @IsActive        BIT,
    @Created         DATETIME,
    @Modified        DATETIME,
    @UserID          INT
AS
BEGIN
    UPDATE [dbo].Doctor
    SET
        [dbo].Doctor.Name = @Name,
        [dbo].Doctor.Phone = @Phone,
        [dbo].Doctor.Email = @Email,
        [dbo].Doctor.Qualification = @Qualification,
        [dbo].Doctor.Specialization = @Specialization,
        [dbo].Doctor.IsActive = @IsActive,
        [dbo].Doctor.Created = @Created,
        [dbo].Doctor.Modified = @Modified,
        [dbo].Doctor.UserID = @UserID
    WHERE
        [dbo].Doctor.DoctorID = @DoctorID
END




