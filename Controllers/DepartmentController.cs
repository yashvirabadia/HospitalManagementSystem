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


        #region Department Insert (Add/Edit)
        public IActionResult DepartmentAddEdit(DepartmentModel departmentModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

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
                        }
                        else
                        {
                            command.CommandText = "PR_DEPT_Department_UpdateByPK";
                            command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = departmentModel.DepartmentID;
                        }

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

        public IActionResult DepartmentSave(DepartmentModel departmentModel)
        {
            if (departmentModel.DepartmentID <= 0)
            {
                ModelState.AddModelError("DepartmentID", "A valid Department is required.");
            }

            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                if (departmentModel.DepartmentID > 0)
                {
                    command.CommandText = "PR_DEPT_Department_UpdateByPK";
                    command.Parameters.AddWithValue("DepartmentID", departmentModel.DepartmentID);
                }
                else
                {
                    command.CommandText = "PR_DEPT_Department_Insert";
                }

                command.Parameters.Add("@DepartmentName", SqlDbType.VarChar).Value = departmentModel.DepartmentName;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = departmentModel.Description;
                command.Parameters.Add("@IsActive", SqlDbType.Decimal).Value = departmentModel.IsActive;
                command.Parameters.Add("@Modified", SqlDbType.VarChar).Value = departmentModel.Modified;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = departmentModel.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("ProductList");
            }

            return View("ProductAddEdit", departmentModel);
        }
    }
}
