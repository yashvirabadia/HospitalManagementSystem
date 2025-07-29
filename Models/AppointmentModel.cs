using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class AppointmentModel
    {

        public int? AppointmentID { get; set; }

        [Required]
        public int DoctorID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(20)]
        public string? AppointmentStatus { get; set; }

        [Required]
        [StringLength(250)]
        public string? Description { get; set; }

        [Required]
        [StringLength(100)]
        public string? SpecialRemarks { get; set; }

        [Required]
        public DateTime? Created { get; set; } = DateTime.Now;

        [Required]
        public DateTime? Modified { get; set; } = DateTime.Now;

        [Required]
        public int UserID { get; set; }

        [Required]
        [Range(0.00, 999999.99)]
        public decimal TotalConsultedAmount { get; set; }
    }
}



