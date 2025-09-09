using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            SqlConnection connection = new(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_DOCDEP_DoctorDepartment_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new();
            table.Load(reader);
            return View(table);
        }
        #endregion

        #region Doctor Department Add Edit
        [HttpGet]
        public IActionResult DoctorDepartmentAddEdit()
        {
            UserDropDown();
            DoctorDropDown();
            DepartmentDropDown();
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
                    using (SqlConnection connection = new(connectionString))
                    {
                        connection.Open();
                        using SqlCommand command = connection.CreateCommand();
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
                    TempData["Message"] = "Add/Edited Successfully";
                    return RedirectToAction("DoctorDepartmentList");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error saving Doctor-Department: " + ex.Message;
                    UserDropDown();
                    DoctorDropDown();
                    DepartmentDropDown();
                    return View("DoctorDepartmentAddEdit", doctorDepartmentModel);
                }
            }

            return View("DoctorDepartmentAddEdit");
        }
        #endregion

        #region Doctor Department Form Fill
        public IActionResult DoctorDepartmentFormFill(int ID)
        {
            DoctorDepartmentModel model = new();

            if (ID > 0)
            {
                try
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    using (SqlConnection connection = new(connectionString))
                    {
                        connection.Open();
                        using SqlCommand command = connection.CreateCommand();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "PR_DOCDEP_DoctorDepartment_SelectByPK";
                        command.Parameters.AddWithValue("DoctorDepartmentID", ID);

                        using SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            model.UserID = Convert.ToInt32(reader["UserID"]);
                            model.DoctorID = Convert.ToInt32(reader["DoctorID"]);
                            model.DepartmentID = Convert.ToInt32(reader["DepartmentID"]);
                            model.DoctorDepartmentID = ID;
                            model.Modified = DateTime.Now;
                        }
                    }

                    return View("DoctorDepartmentAddEdit", model);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error loading Doctor-Department for edit: " + ex.Message;
                    UserDropDown();
                    DoctorDropDown();
                    DepartmentDropDown();
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
                SqlConnection connection = new(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_DOCDEP_DoctorDepartment_DeleteByPK";
                command.Parameters.Add("@DoctorDepartmentID", SqlDbType.Int).Value = DoctorDepartmentID;
                TempData["Message"] = "Deleted Successfully";
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

        #region Department Drop Down
        public void DepartmentDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_DEPT_Department_SelectForDropDown";

                SqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                List<SelectListItem> departmentList = new List<SelectListItem>();
                foreach (DataRow data in dt.Rows)
                {
                    departmentList.Add(new SelectListItem
                    {
                        Value = data["DepartmentID"].ToString(),
                        Text = data["DepartmentName"].ToString()
                    });
                }

                ViewBag.DepartmentList = departmentList;
            }
        }
        #endregion

    }
}
