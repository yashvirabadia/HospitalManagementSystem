using System;

namespace HospitalManagementSystem.Models
{
    public class DoctorDepartmentModel
    {
        public int? DoctorDepartmentID { get; set; } 
        public int? DoctorID { get; set; }           
        public int? DepartmentID { get; set; }       
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; } = DateTime.Now;
        public int? UserID { get; set; }             
    }
}
