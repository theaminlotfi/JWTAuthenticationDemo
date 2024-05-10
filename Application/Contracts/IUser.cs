using Application.Dtos;

namespace Application.Contracts;

public interface IUser
{
    Task<RegistrationResponseDto> RegisterUserAsync(RegisterUserDto registerUserDto);
    Task<LoginResponseDto> LoginUserAsync(LoginDto loginDto);
}
