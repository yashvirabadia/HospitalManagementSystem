using ClosedXML.Excel;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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
        [CheckAccess]
        public IActionResult UserList(string name, string email, string mobileNo)
        {
            DataTable table = new DataTable();

            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_USR_Users_SelectAll";

                    command.Parameters.AddWithValue("@Name", string.IsNullOrEmpty(name) ? (object)DBNull.Value : name);
                    command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
                    command.Parameters.AddWithValue("@MobileNo", string.IsNullOrEmpty(mobileNo) ? (object)DBNull.Value : mobileNo);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }
            }

            return View(table);
        }
        #endregion


        #region User Add Edit
        [CheckAccess]
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
        [CheckAccess]
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
        [CheckAccess]
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

        #region Export to Excel
        [HttpGet("ExportToExcel")]
        [CheckAccess]
        public IActionResult ExportToExcel()
        {
            DataTable dt = new DataTable();

            try
            {
                // ✅ Fetch data from Users table
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ConnectionString")))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT [User].UserID,[User].UserName ,[User].[Password],[User].Email,[User].MobileNo,[User].IsActive ,[User].Created ,[User].Modified FROM [dbo].[User]", connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }

                // ✅ Create Excel workbook
                using (var workbook = new XLWorkbook())
                {
                    dt.TableName = "Users";
                    workbook.Worksheets.Add(dt);

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        
                        return File(content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "UsersList.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error exporting data: " + ex.Message;
                return RedirectToAction("UserList");
            }
        }
        #endregion

        #region Export to CSV
        [HttpGet("ExportToCSV")]
        [CheckAccess]
        public IActionResult ExportToCSV()
        {
            DataTable dt = new DataTable();

            try
            {
                // ✅ Fetch data from Users table
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ConnectionString")))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT UserID, UserName, Email, IsActive, Created, Modified FROM [User]", connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }

                //Convert DataTable to CSV
                StringBuilder sb = new StringBuilder();

                // Add column headers
                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                sb.AppendLine(string.Join(",", columnNames));

                // Add rows
                foreach (DataRow row in dt.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field => "\"" + field.ToString().Replace("\"", "\"\"") + "\"");
                    sb.AppendLine(string.Join(",", fields));
                }

                //Return CSV File
                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "UsersList.csv");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error exporting data: " + ex.Message;
                return RedirectToAction("UserList");
            }
        }
        #endregion

        #region search user
        [CheckAccess]
        public IActionResult UserSearch(string searchTerm, string filterBy)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ConnectionString")))
            {
                string query = @"SELECT [User].UserID,
                                [User].UserName,
                                [User].[Password],
                                [User].Email,
                                [User].MobileNo,
                                [User].IsActive,
                                [User].Created,
                                [User].Modified
                         FROM [dbo].[User]";

                //Apply filter only if both filterBy and searchTerm exist
                if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(filterBy))
                {
                    query += $" WHERE [User].[{filterBy}] LIKE @Search";
                }

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(filterBy))
                    {
                        cmd.Parameters.AddWithValue("@Search", "%" + searchTerm + "%");
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return View("UserList", dt);
        }
        #endregion

        #region User Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_USR_User_ValidateLogin";
                    sqlCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = userLoginModel.Username;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            HttpContext.Session.SetInt32("UserID", Convert.ToInt32(dr["UserID"]));
                            HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                            HttpContext.Session.SetString("EmailAddress", dr["Email"].ToString());
                        }

                        return RedirectToAction("UserList", "User");

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "User is not found";
                        return RedirectToAction("Login", "User");

                    }

                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login");
        }
        //public IActionResult Login()
        //{
        //    return View();
        //}
        #endregion

        #region User Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
        #endregion
    }
}
