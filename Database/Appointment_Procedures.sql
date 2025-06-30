CREATE OR ALTER PROC PR_APP_Appointment_SelectAll
AS
BEGIN
    SELECT 
        A.AppointmentID,
        A.DoctorID,
        D.[Name] AS DoctorName,
        D.Specialization,
        A.PatientID,
        P.[Name] AS PatientName,
        P.Gender,
        A.AppointmentDate,
        A.AppointmentStatus,
        A.[Description],
        A.SpecialRemarks,
        A.Created,
        A.Modified,
        A.UserID,
        U.UserName,
        A.TotalConsultedAmount
    FROM [dbo].Appointment A
    INNER JOIN [dbo].Doctor D ON A.DoctorID = D.DoctorID
    INNER JOIN [dbo].Patient P ON A.PatientID = P.PatientID
    INNER JOIN [dbo].[User] U ON A.UserID = U.UserID
END





-- PR_APP_Appointment_Insert 2, 2, '2024-07-01', 'Scheduled', 'Routine check-up', 'None', '2024-06-01', '2024-06-20', 1, 500.00
CREATE OR ALTER PROC PR_APP_Appointment_Insert
    @DoctorID              INT,
    @PatientID             INT,
    @AppointmentDate       DATETIME,
    @AppointmentStatus     NVARCHAR(20),
    @Description           NVARCHAR(250),
    @SpecialRemarks        NVARCHAR(100),
    @Created               DATETIME,
    @Modified              DATETIME,
    @UserID                INT,
    @TotalConsultedAmount  DECIMAL(5,2)
AS
BEGIN
    INSERT INTO [dbo].Appointment (
        [dbo].Appointment.DoctorID,
        [dbo].Appointment.PatientID,
        [dbo].Appointment.AppointmentDate,
        [dbo].Appointment.AppointmentStatus,
        [dbo].Appointment.[Description],
        [dbo].Appointment.SpecialRemarks,
        [dbo].Appointment.Created,
        [dbo].Appointment.Modified,
        [dbo].Appointment.UserID,
        [dbo].Appointment.TotalConsultedAmount
    )
    VALUES (
        @DoctorID,
        @PatientID,
        @AppointmentDate,
        @AppointmentStatus,
        @Description,
        @SpecialRemarks,
        @Created,
        @Modified,
        @UserID,
        @TotalConsultedAmount
    )
END




-- Example call: PR_APP_Appointment_DeleteByPK 1
CREATE OR ALTER PROC PR_APP_Appointment_DeleteByPK
    @AppointmentID INT
AS
BEGIN
    DELETE FROM [dbo].Appointment 
    WHERE [dbo].Appointment.AppointmentID = @AppointmentID
END



-- Example call: PR_APP_Appointment_SelectByPK 3
CREATE OR ALTER PROC PR_APP_Appointment_SelectByPK
    @AppointmentID INT
AS
BEGIN
    SELECT 
        A.AppointmentID,
        A.DoctorID,
        D.[Name] AS DoctorName,
        D.Specialization,
        A.PatientID,
        P.[Name] AS PatientName,
        P.Gender,
        A.AppointmentDate,
        A.AppointmentStatus,
        A.[Description],
        A.SpecialRemarks,
        A.Created,
        A.Modified,
        A.UserID,
        U.UserName,
        A.TotalConsultedAmount
    FROM [dbo].Appointment A
    INNER JOIN [dbo].Doctor D ON A.DoctorID = D.DoctorID
    INNER JOIN [dbo].Patient P ON A.PatientID = P.PatientID
    INNER JOIN [dbo].[User] U ON A.UserID = U.UserID
    WHERE A.AppointmentID = @AppointmentID
END




-- PR_APP_Appointment_UpdateByPK 3, 1, 2, '2024-07-01', 'Completed', 'Follow-up visit', 'Take meds', '2024-06-01', '2024-06-22', 5, 750.00
CREATE OR ALTER PROC PR_APP_Appointment_UpdateByPK
    @AppointmentID         INT,
    @DoctorID              INT,
    @PatientID             INT,
    @AppointmentDate       DATETIME,
    @AppointmentStatus     NVARCHAR(20),
    @Description           NVARCHAR(250),
    @SpecialRemarks        NVARCHAR(100),
    @Created               DATETIME,
    @Modified              DATETIME,
    @UserID                INT,
    @TotalConsultedAmount  DECIMAL(5,2)
AS
BEGIN
    UPDATE [dbo].Appointment
    SET
        [dbo].Appointment.DoctorID = @DoctorID,
        [dbo].Appointment.PatientID = @PatientID,
        [dbo].Appointment.AppointmentDate = @AppointmentDate,
        [dbo].Appointment.AppointmentStatus = @AppointmentStatus,
        [dbo].Appointment.[Description] = @Description,
        [dbo].Appointment.SpecialRemarks = @SpecialRemarks,
        [dbo].Appointment.Created = @Created,
        [dbo].Appointment.Modified = @Modified,
        [dbo].Appointment.UserID = @UserID,
        [dbo].Appointment.TotalConsultedAmount = @TotalConsultedAmount
    WHERE [dbo].Appointment.AppointmentID = @AppointmentID
END




--PR_APP_Appointment_SelectByUserID 1
CREATE OR ALTER PROC PR_APP_Appointment_SelectByUserID
    @UserID INT
AS
BEGIN
    SELECT 
        A.AppointmentID,
        A.DoctorID,
        D.[Name] AS DoctorName,
        D.Specialization,
        A.PatientID,
        P.[Name] AS PatientName,
        P.Gender,
        A.AppointmentDate,
        A.AppointmentStatus,
        A.[Description],
        A.SpecialRemarks,
        A.Created,
        A.Modified,
       -- A.UserID,
       -- U.UserName AS EnteredByUser,
        A.TotalConsultedAmount
    FROM [dbo].Appointment A
    INNER JOIN [dbo].Doctor D ON A.DoctorID = D.DoctorID
    INNER JOIN [dbo].Patient P ON A.PatientID = P.PatientID
    INNER JOIN [dbo].[User] U ON A.UserID = U.UserID
    WHERE A.UserID = @UserID
END
