using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HospitalManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IConfiguration configuration;

        public DashboardController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult Index()
        {
            DashboardModel model = new DashboardModel();

            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                // Entity counts
                model.UserCount = ExecuteCount(connection, "SELECT COUNT(*) FROM [User]");
                model.DoctorCount = ExecuteCount(connection, "SELECT COUNT(*) FROM Doctor");
                model.PatientCount = ExecuteCount(connection, "SELECT COUNT(*) FROM Patient");
                model.DepartmentCount = ExecuteCount(connection, "SELECT COUNT(*) FROM Department");
                model.AppointmentCount = ExecuteCount(connection, "SELECT COUNT(*) FROM Appointment");
                model.DocDepCount = ExecuteCount(connection, "SELECT COUNT(*) FROM DoctorDepartment");

                // Revenue (per month)
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT DATENAME(MONTH, Created) AS [Month], " +
                    "SUM(TotalConsultedAmount) AS Total " +
                    "FROM Appointment " +
                    "GROUP BY DATENAME(MONTH, Created)", connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        model.RevenueLabels.Add(reader["Month"].ToString());
                        model.RevenueData.Add(Convert.ToDecimal(reader["Total"]));
                    }
                }

                // Appointments per month
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT DATENAME(MONTH, Created) AS [Month], " +
                    "COUNT(*) AS Total " +
                    "FROM Appointment " +
                    "GROUP BY DATENAME(MONTH, Created)", connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        model.AppointmentLabels.Add(reader["Month"].ToString());
                        model.AppointmentData.Add(Convert.ToInt32(reader["Total"]));
                    }
                }

                // Top 5 Doctors by appointment count
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT TOP 5 d.Name, COUNT(a.AppointmentID) AS TotalAppointments " +
                    "FROM Appointment a " +
                    "INNER JOIN Doctor d ON a.DoctorID = d.DoctorID " +
                    "GROUP BY d.Name " +
                    "ORDER BY TotalAppointments DESC", connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        model.TopDoctorLabels.Add(reader["Name"].ToString());
                        model.TopDoctorData.Add(Convert.ToInt32(reader["TotalAppointments"]));
                    }
                }

                // Appointment status distribution
                //using (SqlCommand cmd = new SqlCommand(
                //    "SELECT AppointmentStatus, COUNT(*) AS Total " +
                //    "FROM Appointment " +
                //    "GROUP BY AppointmentStatus", connection))
                //using (SqlDataReader reader = cmd.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        model.AppointmentStatusLabels.Add(reader["AppointmentStatus"].ToString());
                //        model.AppointmentStatusData.Add(Convert.ToInt32(reader["Total"]));
                //    }
                //}
            }

            return View(model);
        }

        private int ExecuteCount(SqlConnection conn, string query)
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}
