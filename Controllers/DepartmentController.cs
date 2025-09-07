using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace HospitalManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {
        private IConfiguration configuration;

        public DepartmentController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        #region Get All Department List
        public IActionResult DepartmentList()
        {

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_DEPT_Department_Selectall";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion


        #region Department (Add/Edit)
        
        [HttpGet]
        public IActionResult DepartmentAddEdit()
        {
            UserDropDown();  
            return View();
        }

        [HttpPost]
        public IActionResult DepartmentAddEdit(DepartmentModel departmentModel)
        {
            // remove validation for properties not posted from the form
            ModelState.Remove("Created");
            ModelState.Remove("Modified");

            if (!ModelState.IsValid)
            {
                UserDropDown();
                return View("DepartmentAddEdit", departmentModel);
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

                        if (departmentModel.DepartmentID == null)
                        {
                            command.CommandText = "PR_DEPT_Department_Insert";
                            departmentModel.Created = DateTime.Now;
                            command.Parameters.AddWithValue("@Created", departmentModel.Created);
                        }
                        else
                        {
                            command.CommandText = "PR_DEPT_Department_UpdateByPK";
                            command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = departmentModel.DepartmentID;
                        }

                        // always set Modified
                        departmentModel.Modified = DateTime.Now;

                        command.Parameters.AddWithValue("@DepartmentName", departmentModel.DepartmentName);
                        command.Parameters.AddWithValue("@Description", departmentModel.Description);
                        command.Parameters.AddWithValue("@IsActive", departmentModel.IsActive);
                        command.Parameters.AddWithValue("@Modified", departmentModel.Modified);
                        command.Parameters.AddWithValue("@UserID", departmentModel.UserID);

                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("DepartmentList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error saving department: " + ex.Message;
                UserDropDown();
                return View("DepartmentAddEdit", departmentModel);
            }
        }

        #endregion

        #region Department Fill Form
        public IActionResult DepartmentFormFill(int ID)
        {
            DepartmentModel model = new DepartmentModel
            {
                DepartmentName = "",
                Description = ""
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
                            command.CommandText = "PR_DEPT_Department_SelectByPK";
                            command.Parameters.AddWithValue("DepartmentID", ID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    model.DepartmentID = ID;
                                    model.UserID = Convert.ToInt32(reader["UserID"]);
                                    model.DepartmentName = reader["DepartmentName"].ToString();
                                    model.Description = reader["Description"].ToString();
                                    model.IsActive = Convert.ToBoolean(reader["IsActive"]);
                                }
                            }
                        }
                    }

                    UserDropDown();
                    return View("DepartmentAddEdit", model);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error loading department: " + ex.Message;
                    return RedirectToAction("DepartmentList");
                }
            }

            return View("DepartmentAddEdit", model);
        }
        #endregion


        #region Department Delete
        public IActionResult DepartmentDelete(int DepartmentID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_DEPT_Department_DeleteByPK";
                command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = DepartmentID;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("DepartmentList");
        }

        #endregion


        #region User Drop Down
        public void UserDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_USR_User_SelectForDropDown";

            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            List<UserDropDownModel> userList = new List<UserDropDownModel>();
            foreach (DataRow data in dataTable.Rows)
            {
                UserDropDownModel model = new UserDropDownModel();
                model.UserID = Convert.ToInt32(data["UserID"]);
                model.UserName = data["UserName"].ToString();
                userList.Add(model);
            }

            ViewBag.UserList = userList;
        }

        #endregion
    }
}
