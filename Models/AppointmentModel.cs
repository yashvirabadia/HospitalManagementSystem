using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class AppointmentModel
    {
        [Key]
        public int? AppointmentID { get; set; }

        [Required(ErrorMessage = "Doctor is required.")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Patient is required.")]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Appointment date is required.")]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        public string? AppointmentStatus { get; set; }

        [Required(ErrorMessage = "Please enter description.")]
        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Special remarks are required.")]
        [StringLength(100, ErrorMessage = "Remarks cannot exceed 100 characters.")]
        public string? SpecialRemarks { get; set; }

        [Required]
        public DateTime? Created { get; set; } = DateTime.Now;

        [Required]
        public DateTime? Modified { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "User is required.")]
        public int UserID { get; set; }

        [Range(0.00, 99999.99, ErrorMessage = "Amount must be between 0.00 and 99999.99")]
        [DataType(DataType.Currency)]
        public decimal? TotalConsultedAmount { get; set; }
    }
}
