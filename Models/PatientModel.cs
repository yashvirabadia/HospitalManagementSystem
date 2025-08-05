
using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class PatientModel
    {
        public int? PatientID { get; set; }

        public required string Name { get; set; }


        public DateTime DateOfBirth { get; set; }
        [StringLength(10)]
        public required string Gender { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(100)]
        public required string Phone { get; set; }

        [Required]
        [StringLength(250)]
        public required string Address { get; set; }

        [Required]
        [StringLength(100)]
        public required string City { get; set; }

        [Required]
        [StringLength(100)]
        public string State { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public DateTime Modified { get; set; } = DateTime.Now;

        [Required]
        public int UserID { get; set; }
    }
}
