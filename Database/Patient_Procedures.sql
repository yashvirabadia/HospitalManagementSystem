--PR_PAT_Patient_SelectAll
CREATE OR ALTER PROC PR_PAT_Patient_SelectAll
AS
BEGIN
    SELECT 
        P.PatientID
        ,P.[Name]
        ,P.DateOfBirth
        ,P.Gender
        ,P.Email
        ,P.Phone
        ,P.[Address]
        ,P.City
        ,P.[State]
        ,P.IsActive
        ,P.Created
        ,P.Modified
        ,P.UserID
     FROM [dbo].Patient AS P 
	 JOIN [User] AS U
     ON P.UserID = U.UserID;
END




--PR_PAT_Patient_Insert 'Asha','2000-01-01','Female','asha@gmail.com','9876543210','Street 123','Surat','Gujarat',1,'2024-01-01','2024-06-01',1
--PR_PAT_Patient_Insert 'Reva Joshi', '2000-05-01', 'Female', 'reva@example.com', '9990008888', 'Green St, 12', 'Ahmedabad', 'Gujarat', 1, '2024-01-01','2024-06-01' , 1
--PR_PAT_Patient_Insert 'Karan Mehta', '1995-10-20', 'Male', 'karan@example.com', '7778889990', 'Blue Hill Road', 'Rajkot', 'Gujarat', 1, '2024-01-01','2024-06-01', 2
CREATE OR ALTER PROC PR_PAT_Patient_Insert
    @Name           NVARCHAR(100),
    @DateOfBirth    DATETIME,
    @Gender         NVARCHAR(10),
    @Email          NVARCHAR(100),
    @Phone          NVARCHAR(100),
    @Address        NVARCHAR(250),
    @City           NVARCHAR(100),
    @State          NVARCHAR(100),
    @IsActive       BIT,
    @Created        DATETIME,
    @Modified       DATETIME,
    @UserID         INT
AS
BEGIN
    INSERT INTO [dbo].Patient (
        [dbo].Patient.[Name],
        [dbo].Patient.DateOfBirth,
        [dbo].Patient.Gender,
        [dbo].Patient.Email,
        [dbo].Patient.Phone,
        [dbo].Patient.[Address],
        [dbo].Patient.City,
        [dbo].Patient.[State],
        [dbo].Patient.IsActive,
        [dbo].Patient.Created,
        [dbo].Patient.Modified,
        [dbo].Patient.UserID
    )
    VALUES (
        @Name,
        @DateOfBirth,
        @Gender,
        @Email,
        @Phone,
        @Address,
        @City,
        @State,
        @IsActive,
        @Created,
        @Modified,
        @UserID
    )
END



--PR_PAT_Patient_DeleteByPK 4
CREATE OR ALTER PROC PR_PAT_Patient_DeleteByPK
    @PatientID INT
AS
BEGIN
    DELETE FROM [dbo].Patient 
    WHERE [dbo].Patient.PatientID = @PatientID
END




-- Example: PR_PAT_Patient_SelectByPK 2
CREATE OR ALTER PROC PR_PAT_Patient_SelectByPK
    @PatientID INT
AS
BEGIN
    SELECT 
        P.PatientID
        ,P.[Name]
        ,P.DateOfBirth
        ,P.Gender
        ,P.Email
        ,P.Phone
        ,P.[Address]
        ,P.City
        ,P.[State]
        ,P.IsActive
        ,P.Created
        ,P.Modified
        ,P.UserID
    FROM [dbo].Patient AS P 
	JOIN [dbo].[User] AS U
    ON P.UserID = U.UserID
    WHERE P.PatientID = @PatientID
END



-- PR_PAT_Patient_UpdateByPK 3,'Asha Patel','2000-01-01','Female','asha@update.com','1112223333','New Address','Ahmedabad','Gujarat',1,'2024-01-01','2024-06-01',2
CREATE OR ALTER PROC PR_PAT_Patient_UpdateByPK
    @PatientID      INT,
    @Name           NVARCHAR(100),
    @DateOfBirth    DATETIME,
    @Gender         NVARCHAR(10),
    @Email          NVARCHAR(100),
    @Phone          NVARCHAR(100),
    @Address        NVARCHAR(250),
    @City           NVARCHAR(100),
    @State          NVARCHAR(100),
    @IsActive       BIT,
    @Created        DATETIME,
    @Modified       DATETIME,
    @UserID         INT
AS
BEGIN
    UPDATE [dbo].Patient
    SET
        [dbo].Patient.[Name]		= @Name,
        [dbo].Patient.DateOfBirth	= @DateOfBirth,
        [dbo].Patient.Gender		= @Gender,
        [dbo].Patient.Email			= @Email,
        [dbo].Patient.Phone			= @Phone,
        [dbo].Patient.[Address]		= @Address,
        [dbo].Patient.City			= @City,
        [dbo].Patient.[State]		= @State,
        [dbo].Patient.IsActive		= @IsActive,
        [dbo].Patient.Created		= @Created,
        [dbo].Patient.Modified		= @Modified,
        [dbo].Patient.UserID		= @UserID
    WHERE [dbo].Patient.PatientID	= @PatientID
END



