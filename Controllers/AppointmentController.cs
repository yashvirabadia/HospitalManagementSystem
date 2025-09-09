using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HospitalManagementSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private IConfiguration configuration;

        public AppointmentController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        #region Appointment List
        public IActionResult AppointmentList()
        {

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_APP_Appointment_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion

        #region Appointment Add Edit

        public IActionResult AppointmentAddEdit(AppointmentModel appointmentModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                UserDropDown();
                DoctorDropDown();
                PatientDropDown();
                return View("AppointmentAddEdit", appointmentModel);
            }

            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (appointmentModel.AppointmentID > 0)
                        {
                            command.CommandText = "PR_APP_Appointment_UpdateByPK";
                            command.Parameters.AddWithValue("AppointmentID", appointmentModel.AppointmentID);
                        }
                        else
                        {
                            command.CommandText = "PR_APP_Appointment_Insert";
                        }

                        appointmentModel.Modified = DateTime.Now;

                        command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = appointmentModel.DoctorID;
                        command.Parameters.Add("@PatientID", SqlDbType.Int).Value = appointmentModel.PatientID;
                        command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = appointmentModel.AppointmentDate;
                        command.Parameters.Add("@AppointmentStatus", SqlDbType.NVarChar).Value = appointmentModel.AppointmentStatus;
                        command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = appointmentModel.Description;
                        command.Parameters.Add("@SpecialRemarks", SqlDbType.NVarChar).Value = appointmentModel.SpecialRemarks;
                        command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = appointmentModel.Modified;
                        command.Parameters.Add("@TotalConsultedAmount", SqlDbType.Decimal).Value = appointmentModel.TotalConsultedAmount;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = appointmentModel.UserID;
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("AppointmentList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error saving appointment: " + ex.Message;
                UserDropDown();
                DoctorDropDown();
                PatientDropDown();
                return View("AppointmentAddEdit", appointmentModel);
            }
        }
        #endregion

        #region Appointment Form Fill
        public IActionResult AppointmentFormFill(int ID)
        {
            AppointmentModel model = new AppointmentModel();

            if (ID > 0)
            {
                try
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "PR_APP_Appointment_SelectByPK";
                            command.Parameters.AddWithValue("AppointmentID", ID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    model.UserID = Convert.ToInt32(reader["UserID"]);
                                    model.AppointmentID = Convert.ToInt32(reader["AppointmentID"]);
                                    model.DoctorID = Convert.ToInt32(reader["DoctorID"]);
                                    model.PatientID = Convert.ToInt32(reader["PatientID"]);
                                    model.AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                                    model.AppointmentStatus = reader["AppointmentStatus"].ToString();
                                    model.Description = reader["Description"].ToString();
                                    model.SpecialRemarks = reader["SpecialRemarks"].ToString();
                                    model.Created = Convert.ToDateTime(reader["Created"]);
                                    model.TotalConsultedAmount = Convert.ToInt64(reader["TotalConsultedAmount"]);
                                }
                            }
                        }
                    }

                    return View("AppointmentAddEdit", model);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error loading appointment: " + ex.Message;
                    return RedirectToAction("AppointmentList");
                }
            }
            UserDropDown();
            DoctorDropDown();
            PatientDropDown();
            return View("AppointmentAddEdit", model);
        }
        #endregion


        #region Appointment Delete
        public IActionResult AppointmentDelete(int AppointmentID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_APP_Appointment_DeleteByPK";
                command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("AppointmentList");
        }
        #endregion

        #region User Drop Down
        public void UserDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_USR_User_SelectForDropDown";

                SqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                List<SelectListItem> userList = new List<SelectListItem>();
                foreach (DataRow data in dt.Rows)
                {
                    userList.Add(new SelectListItem
                    {
                        Value = data["UserID"].ToString(),
                        Text = data["UserName"].ToString()
                    });
                }

                ViewBag.UserList = userList;
            }
        }


        #endregion

        #region Doctor Drop Down
        public void DoctorDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_DOC_Doctor_SelectForDropDown";

                SqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                List<SelectListItem> doctorList = new List<SelectListItem>();
                foreach (DataRow data in dt.Rows)
                {
                    doctorList.Add(new SelectListItem
                    {
                        Value = data["DoctorID"].ToString(),
                        Text = data["DoctorName"].ToString()
                    });
                }

                ViewBag.DoctorList = doctorList;
            }
        }
        #endregion

        #region Patient Drop Down
        public void PatientDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_PAT_Patient_SelectForDropDown";

                SqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                List<SelectListItem> patientList = new List<SelectListItem>();
                foreach (DataRow data in dt.Rows)
                {
                    patientList.Add(new SelectListItem
                    {
                        Value = data["PatientID"].ToString(),
                        Text = data["PatientName"].ToString()
                    });
                }

                ViewBag.PatientList = patientList;
            }
        }
        #endregion

    }
}
