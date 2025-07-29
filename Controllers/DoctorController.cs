using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace HospitalManagementSystem.Controllers
{
    public class DoctorController : Controller
    {
        private IConfiguration configuration;

        public DoctorController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        #region Doctor List
        public IActionResult DoctorList()
        {

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_DOC_Doctor_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion

       #region Doctor Add Edit
        public IActionResult DoctorAddEdit(DoctorModel doctorModel)
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
                            command.CommandText = "PR_Doc_Doctor_Insert";

                            if (doctorModel.DoctorID == null)
                             {
                                command.CommandText = "PR_Doc_Doctor_Insert";
                             }
                            else
                             {
                                command.CommandText = "PR_Doc_Doctor_UpdateByPK";
                                command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctorModel.DoctorID;
                             }

                            command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = doctorModel.Name;
                            command.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = doctorModel.Phone;
                            command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = doctorModel.Email;
                            command.Parameters.Add("@Qualification", SqlDbType.NVarChar).Value = doctorModel.Qualification;
                            command.Parameters.Add("@Specialization", SqlDbType.NVarChar).Value = doctorModel.Specialization;
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = doctorModel.IsActive;
                            command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = doctorModel.Modified;
                            command.Parameters.Add("@UserId", SqlDbType.Int).Value = doctorModel.UserID;

                            command.ExecuteNonQuery();
                        }
                    }

                    return RedirectToAction("DoctorList");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while saving doctor: " + ex.Message;
                    return RedirectToAction("DoctorList");

                }
            }

            return View("DoctorAddEdit");
        }
        #endregion

        #region Doctor Form Fill
        public IActionResult DoctorFormFill(int ID)
        {
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
                            command.CommandText = "PR_DOC_Doctor_SelectByPK";
                            command.Parameters.AddWithValue("DoctorID", ID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                DoctorModel model = new DoctorModel
                                {
                                    Name = "",
                                    Phone = "",
                                    Email = "",
                                    Qualification = "",
                                    Specialization = ""
                                };

                                while (reader.Read())
                                {
                                    model.DoctorID = Convert.ToInt32(reader["DoctorID"]);
                                    model.Name = reader["Name"].ToString();
                                    model.Phone = reader["Phone"].ToString();
                                    model.Email = reader["Email"].ToString();
                                    model.Qualification = reader["Qualification"].ToString();
                                    model.Specialization = reader["Specialization"].ToString();
                                    model.IsActive = Convert.ToBoolean(reader["IsActive"]);
                                    model.UserID = Convert.ToInt32(reader["UserId"]);
                                }

                                return View("DoctorAddEdit", model);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while fetching doctor details: " + ex.Message;
                    return RedirectToAction("DoctorList");
                }
            }

            return View("DoctorAddEdit", new DoctorModel
            {
                Name = "",
                Phone = "",
                Email = "",
                Qualification = "",
                Specialization = ""
            });
        }
        #endregion

        #region Doctor Delete
        public IActionResult DoctorDelete(int DoctorID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_DOC_Doctor_DeleteByPK";
                command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = DoctorID;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("DoctorList");
        }
        #endregion
    }
}
