using System.Globalization;

namespace Application.Dtos;

public record LoginResponseDto(bool Flag, string Message = null!, string Token = null!);
