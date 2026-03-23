using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Context;
using RoyalVilla_API.Models.DTOs;
using RoyalVilla_API.Services;

namespace RoyalVilla_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(
            typeof(ApiResponse<object>),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(
            typeof(ApiResponse<object>),
            StatusCodes.Status400BadRequest
        )]
        [ProducesResponseType(
            typeof(ApiResponse<object>),
            StatusCodes.Status409Conflict
        )]
        [ProducesResponseType(
            typeof(ApiResponse<object>),
            StatusCodes.Status404NotFound
        )]
        public async Task<ActionResult<ApiResponse<UserDTO>>> Register([FromBody]
            SignupRequestDTO signupRequestDTO
        )
        {
            try
            {
                if (signupRequestDTO == null)
                {
                    return BadRequest(
                        ApiResponse<object>.BadRequest("Registration information is required.")
                    );
                }

                if (await _authService.IsEmailExistAsync(signupRequestDTO.Email))
                {
                    return Conflict(
                        ApiResponse<object>.Conflict($"User with email {signupRequestDTO.Email} already exists.")
                    );
                }
                var user = await _authService.SignupAsync(signupRequestDTO);

                if (user == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registration failed."));
                }
                var response = ApiResponse<UserDTO>.CreatedAt(user, "User created successfully");
                return CreatedAtAction(nameof(Register), response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(
                    500,
                    $"An error occured while trying to signup.:",
                    ex.Message
                );
                return StatusCode(500, errorResponse);
            }
        }


        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(
            typeof(ApiResponse<object>),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(
            typeof(ApiResponse<object>),
            StatusCodes.Status400BadRequest
        )]
        [ProducesResponseType(
            typeof(ApiResponse<object>),
            StatusCodes.Status409Conflict
        )]
        [ProducesResponseType(
            typeof(ApiResponse<object>),
            StatusCodes.Status404NotFound
        )]
        public async Task<ActionResult<ApiResponse<UserDTO>>> Login([FromBody]
            LoginRequestDTO loginRequestDTO
        )
        {
            try
            {
                if (loginRequestDTO == null)
                {
                    return BadRequest(
                        ApiResponse<object>.BadRequest("Login information is required.")
                    );
                }

                var loginResponse = await _authService.LoginAsync(loginRequestDTO);

                if (loginResponse == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Invalid credentials"));
                }
                var response = ApiResponse<LoginResponseDTO>.Ok(loginResponse, "Login successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(
                    500,
                    $"An error occured while trying to signup.:",
                    ex.Message
                );
                return StatusCode(500, errorResponse);
            }
        }
    }
}
