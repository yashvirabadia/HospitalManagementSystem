using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace HospitalManagementSystem.Controllers
{
    public class DoctorDepartmentController : Controller
    {
        private IConfiguration configuration;

        public DoctorDepartmentController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        #region Doctor Department List
        public IActionResult DoctorDepartmentList()
        {

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_DOCDEP_DoctorDepartment_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion

        #region Doctor Department Add Edit
        [HttpGet]
        public IActionResult DoctorDepartmentAddEdit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DoctorDepartmentAddEdit(DoctorDepartmentModel doctorDepartmentModel)
        {
            if (ModelState.IsValid)
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

                            if (doctorDepartmentModel.DoctorDepartmentID != null && doctorDepartmentModel.DoctorDepartmentID > 0)
                            {
                                command.CommandText = "PR_DOCDEP_DoctorDepartment_UpdateByPK";
                                command.Parameters.AddWithValue("@DoctorDepartmentID", doctorDepartmentModel.DoctorDepartmentID);
                            }
                            else
                            {
                                command.CommandText = "PR_DOCDEP_DoctorDepartment_Insert";
                            }

                            command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctorDepartmentModel.DoctorID;
                            command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = doctorDepartmentModel.DepartmentID;
                            command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = doctorDepartmentModel.Modified;
                            command.Parameters.Add("@UserID", SqlDbType.Int).Value = doctorDepartmentModel.UserID;

                            command.ExecuteNonQuery();
                        }
                    }

                    return RedirectToAction("DoctorDepartmentList");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error saving Doctor-Department: " + ex.Message;
                }
            }

            return View("DoctorDepartmentAddEdit");
        }
        #endregion

        #region Doctor Department Form Fill
        public IActionResult DoctorDepartmentFormFill(int ID)
        {
            DoctorDepartmentModel model = new DoctorDepartmentModel();

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
                            command.CommandText = "PR_DOCDEP_DoctorDepartment_SelectByPK";
                            command.Parameters.AddWithValue("DoctorDepartmentID", ID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    model.UserID = Convert.ToInt32(reader["UserID"]);
                                    model.DoctorID = Convert.ToInt32(reader["DoctorID"]);
                                    model.DepartmentID = Convert.ToInt32(reader["DepartmentID"]);
                                    model.DoctorDepartmentID = ID;
                                    model.Modified = DateTime.Now;
                                }
                            }
                        }
                    }

                    return View("DoctorDepartmentAddEdit", model);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error loading Doctor-Department for edit: " + ex.Message;
                    return RedirectToAction("DoctorDepartmentList");
                }
            }
            else
            {
                return View("DoctorDepartmentList", model);
            }
        }
        #endregion

        #region Doctor Department Delete
        public IActionResult DoctorDepartmentDelete(int DoctorDepartmentID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_DOCDEP_DoctorDepartment_DeleteByPK";
                command.Parameters.Add("@DoctorDepartmentID", SqlDbType.Int).Value = DoctorDepartmentID;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("DoctorDepartmentList");
        }
        #endregion
    }
}
