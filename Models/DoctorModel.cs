using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class DoctorModel
    {
        public int? DoctorID { get; set; }
        public required string Name { get; set; }


        public required string Phone { get; set; }


        public required string Email { get; set; }


        public required string Qualification { get; set; }


        public required string Specialization { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; } = DateTime.Now;

        public int UserID { get; set; }
    }
}