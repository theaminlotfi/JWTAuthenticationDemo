#nullable disable

using Application.Contracts;
using Application.Dtos;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Repositories;

public class UserRepository : IUser
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration; 
    
    public UserRepository(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    private async Task<User> FindUserByEmail(string email) => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    
    private string GenarateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<LoginResponseDto> LoginUserAsync(LoginDto loginDto)
    {
        var getUser = await FindUserByEmail(loginDto.Email);
        if (getUser == null) return new LoginResponseDto(false, "User not found.");

        bool checkPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, getUser.Password);
        if (checkPassword)
            return new LoginResponseDto(true, "Login succeeded", GenarateJwtToken(getUser));
        else
            return new LoginResponseDto(false, "Login failed");
    }

    public async Task<RegistrationResponseDto> RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        var getUser = await FindUserByEmail(registerUserDto.Email);
        if (getUser != null)
            return new RegistrationResponseDto(false, "User already exists.");

        _context.Users.Add(new User()
        {
            Name = registerUserDto.Name,
            Email = registerUserDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password)
        });
        await _context.SaveChangesAsync();
        return new RegistrationResponseDto(true, "Registration completed.");
    }
}
