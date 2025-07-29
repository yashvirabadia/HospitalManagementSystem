using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace HospitalManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private IConfiguration configuration;
       

        public UserController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        #region User List
        public IActionResult UserList()
        {
            

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_USR_Users_Selectall";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion

        #region User Add Edit
        public IActionResult UserAddEdit(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                // Log validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return View("UserAddEdit", userModel);
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

                        if (userModel.UserID > 0)
                        {
                            command.CommandText = "PR_USR_Users_UpdateByPK";
                            command.Parameters.AddWithValue("UserID", userModel.UserID);
                        }
                        else
                        {
                            command.CommandText = "PR_USR_Users_Insert";
                        }

                        userModel.Modified = DateTime.Now;

                        command.Parameters.AddWithValue("@UserName", userModel.UserName);
                        command.Parameters.AddWithValue("@Password", userModel.Password);
                        command.Parameters.AddWithValue("@Email", userModel.Email);
                        command.Parameters.AddWithValue("@MobileNo", userModel.MobileNo);
                        command.Parameters.AddWithValue("@IsActive", userModel.IsActive);
                        command.Parameters.AddWithValue("@Modified", userModel.Modified);

                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error saving user: " + ex.Message;
                return View("UserAddEdit", userModel);
            }
        }
        #endregion

        #region User Form Fill
        public IActionResult UserFormFill(int ID)
        {
            UserModel model = new UserModel
            {
                UserName = "",
                Password = "",
                Email = "",
                MobileNo = "",
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
                            command.CommandText = "PR_USR_Users_SelectByPK";
                            command.Parameters.AddWithValue("UserID", ID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    model.UserID = Convert.ToInt32(reader["UserID"]);
                                    model.UserName = reader["UserName"].ToString();
                                    model.Password = reader["Password"].ToString();
                                    model.Email = reader["Email"].ToString();
                                    model.MobileNo = reader["MobileNo"].ToString();
                                    model.IsActive = Convert.ToBoolean(reader["IsActive"]);
                                }
                            }
                        }
                    }

                    return View("UserAddEdit", model);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error loading user data: " + ex.Message;
                    return RedirectToAction("UserList");
                }
            }

            return View("UserAddEdit", model);
        }
        #endregion


        #region User Delete
        public IActionResult UserDelete(int UserID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_USR_Users_DeleteByPK";
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("UserList");
        }
        #endregion

    }
}
