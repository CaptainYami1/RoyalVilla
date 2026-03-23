using RoyalVilla_API.Models.DTOs;

namespace RoyalVilla_API.Services
{
    public interface IAuthService
    {
        public Task<bool> IsEmailExistAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> SignupAsync(SignupRequestDTO signUpRequestDTO)
        {
            throw new NotImplementedException();
        }

       public  Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
