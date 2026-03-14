using System.ComponentModel.DataAnnotations;

namespace RoyalVilla_API.Models.DTOs
{
    public class SignupRequestDTO
    {
      
        public required string Email { get; set; }

      
        public required string Name { get; set; }

  
        public required string Password { get; set; }

  
        public required string Role { get; set; } = "customer";
    }
}
