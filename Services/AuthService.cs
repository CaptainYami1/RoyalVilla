using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Context;
using RoyalVilla_API.Models.DTOs;
using RoyalVilla_API.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace RoyalVilla_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly RoyalVilleDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(RoyalVilleDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<UserDTO> SignupAsync(SignupRequestDTO signUpRequestDTO)
        {
            try
            {
                if (await IsEmailExistAsync(signUpRequestDTO.Email))
                {
                    throw new InvalidOperationException($"User with email {signUpRequestDTO.Email} already exist");
                }

                User user = new()
                {
                    Email = signUpRequestDTO.Email,
                    Name = signUpRequestDTO.Name,
                    Password = signUpRequestDTO.Password,
                    Role = string.IsNullOrEmpty(signUpRequestDTO.Role) ? "Customer" : signUpRequestDTO.Role,
                    CreatedAt = DateTime.Now
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return _mapper.Map<UserDTO>(user);
            }

            catch (Exception ex) {
              
                throw new InvalidOperationException("An unexpected error occurred during user registration", ex); 
            }
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
         try {  var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower());
            if (user == null || user.Password != loginRequestDTO.Password){
                    return null;
            }

                var token = GenerateJwtToken(user);
                return new LoginResponseDTO
                {
                    User = _mapper.Map<UserDTO>(user),
                    Token = token
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new InvalidOperationException("An unexpected error occurred during user signin", ex);
            }
        }

        private string GenerateJwtToken(User user)
        {
            var key =Encoding.ASCII.GetBytes(_configuration.GetSection("JwtSettings")["Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ),
                    new Claim(ClaimTypes.NameIdentifier, user.Name ),
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Role),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
