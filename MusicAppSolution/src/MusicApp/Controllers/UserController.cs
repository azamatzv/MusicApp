﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.DataTransferObjects;
using N_Tier.Application.Services;
using N_Tier.Core.DTOs;
using N_Tier.DataAccess.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace MusicApp.Controllers;

[Route("api/user")]
//[Authorize]
public class UserController : ApiController
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

    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp([FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdUser = await _userService.AddUserAsync(userDto);
            return Ok(new { Message = "User successfully registered", User = createdUser });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message, Details = ex.InnerException?.Message });
        }
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = await _userService.AuthenticateAsync(loginDto);
            if (user == null)
                return Unauthorized(new { Message = "Invalid username or password" });

            var accessToken = _jwtTokenHandler.GenerateAccessToken(user);
            var refreshToken = _jwtTokenHandler.GenerateRefreshToken();

            return Ok(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken,
                User = user
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message, Details = ex.InnerException?.Message });
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var isDeleted = await _userService.DeleteUserAsync(id);
            return Ok(isDeleted);
        }
        catch (Exception ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedUser = await _userService.UpdateUserAsync(id, userDto);
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}