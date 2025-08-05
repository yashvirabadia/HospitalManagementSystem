using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models
{
    public class UserModel
    {
       
            public int? UserID { get; set; }

            public required string UserName { get; set; }


            public required string Password { get; set; }

            public required string Email { get; set; }

            public required string MobileNo { get; set; }

            public bool IsActive { get; set; }

            public DateTime? Created { get; set; }

            public DateTime? Modified { get; set; } = DateTime.Now;
        
    }
}
