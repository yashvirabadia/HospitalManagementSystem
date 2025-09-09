using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class DoctorDepartmentModel
    {
        [Key]
        public int? DoctorDepartmentID { get; set; }

        [Required(ErrorMessage = "Doctor is required.")]
        [Display(Name = "Doctor")]
        public int? DoctorID { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public int? DepartmentID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created Date")]
        public DateTime? Created { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [Display(Name = "Modified Date")]
        public DateTime? Modified { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "User is required.")]
        [Display(Name = "User ID")]
        public int? UserID { get; set; }
    }
}
