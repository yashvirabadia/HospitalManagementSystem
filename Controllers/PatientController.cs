using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace HospitalManagementSystem.Controllers
{
    public class PatientController : Controller
    {
        private IConfiguration configuration;

        public PatientController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        #region Patient List
        public IActionResult PatientList()
        {

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_PAT_Patient_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion

        #region Patient Add/Edit
        public IActionResult PatientAddEdit(PatientModel patientModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                UserDropDown();
                return View("PatientAddEdit", patientModel);
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

                        if (patientModel.PatientID > 0)
                        {
                            command.CommandText = "PR_Pat_patient_UpdateByPK";
                            command.Parameters.AddWithValue("PatientID", patientModel.PatientID);
                        }
                        else
                        {
                            command.CommandText = "PR_Pat_Patient_Insert";
                        }

                        patientModel.Modified = DateTime.Now;

                        command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = patientModel.Name;
                        command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = patientModel.DateOfBirth;
                        command.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = patientModel.Gender;
                        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = patientModel.Email;
                        command.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = patientModel.Phone;
                        command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = patientModel.Address;
                        command.Parameters.Add("@City", SqlDbType.NVarChar).Value = patientModel.City;
                        command.Parameters.Add("@State", SqlDbType.NVarChar).Value = patientModel.State;
                        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = patientModel.IsActive;
                        command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = patientModel.Modified;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = patientModel.UserID;

                        command.ExecuteNonQuery();
                    }
                }
                TempData["Message"] = "Add/Edited Successfully";
                return RedirectToAction("PatientList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error saving patient: " + ex.Message;
                UserDropDown();
                return View("PatientAddEdit", patientModel);
            }
        }
        #endregion

        #region Patient Form Fill
        public IActionResult PatientFormFill(int ID)
        {
            PatientModel model = new PatientModel
            {
                Name = "",
                Gender = "",
                Email = "",
                Phone = "",
                Address = "",
                City = "",
            };

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
                            command.CommandText = "PR_PAT_Patient_SelectByPK";

                            command.Parameters.AddWithValue("PatientID", ID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    model.PatientID = Convert.ToInt32(reader["PatientID"]);
                                    model.UserID = Convert.ToInt32(reader["UserID"]);
                                    model.City = reader["City"].ToString();
                                    model.State = reader["State"].ToString();
                                    model.Name = reader["Name"].ToString();
                                    model.Gender = reader["Gender"].ToString();
                                    model.Phone = reader["Phone"].ToString();
                                    model.Email = reader["Email"].ToString();
                                    model.Address = reader["Address"].ToString();
                                    model.IsActive = Convert.ToBoolean(reader["IsActive"]);
                                    model.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                                    model.Modified = DateTime.Now;
                                }
                            }
                        }
                    }

                    return View("PatientAddEdit", model);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error loading patient: " + ex.Message;
                    return RedirectToAction("PatientList");
                }
            }
            UserDropDown();
            return View("PatientAddEdit", model);
        }
        #endregion


        #region Patient Delete
        public IActionResult PatientDelete(int PatientID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_PAT_Patient_DeleteByPK";
                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = PatientID;
                TempData["Message"] = "Deleted Successfully";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("PatientList");
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
    }
}
