using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.DataTransferObjects.Authentication;
using N_Tier.Application.Services;
using N_Tier.Core.DTOs.UserDtos;
using N_Tier.DataAccess.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace MusicApp.Controllers
{

    //[Authorize]
    [Route("api/users")]
    [Authorize(Policy = "User")]
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenHandler _jwtTokenHandler;

        public UserController(IUserService userService, IJwtTokenHandler jwtTokenHandler)
        {
            _jwtTokenHandler = jwtTokenHandler;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("initiate-registration")]
        public async Task<IActionResult> InitiateRegistration([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var (userId, message) = await _userService.InitiateUserRegistrationAsync(userDto);
                return Ok(new { UserId = userId, Message = message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, Details = ex.InnerException?.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("verify-registration")]
        public async Task<IActionResult> VerifyRegistration([FromBody] VerifyRegistrationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.VerifyAndCompleteRegistrationAsync(dto.UserId, dto.OtpCode);
                return Ok(new { Message = "Registration completed successfully", User = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message, Details = ex.InnerException?.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid input data", Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, userDto);
                if (updatedUser == null)
                    return NotFound(new { Message = "User not found" });

                return Ok(new { Message = "User updated successfully", User = updatedUser });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { Message = "An unexpected error occurred", Details = ex.Message });
            }


        }
    }
}
