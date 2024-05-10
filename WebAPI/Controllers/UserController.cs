using Application.Contracts;
using Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUser user;

    public UserController(IUser user)
    {
        this.user = user;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> LoginUser(LoginDto loginDto)
    {
        var result = await user.LoginUserAsync(loginDto);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponseDto>> RegisterUser(RegisterUserDto registerUserDto)
    {
        var result = await user.RegisterUserAsync(registerUserDto);
        return Ok(result);
    }
}
