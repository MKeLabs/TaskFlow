using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.Auth;

public sealed record RegisterRequest(
    [EmailAddress][Required] string Email,
    [Required][MinLength(8)] string Password);

public sealed record LoginRequest(
    [EmailAddress][Required] string Email,
    [Required] string Password);

public sealed record LoginResponse(string AccessToken, DateTimeOffset ExpiresAtUtc, string Email, IReadOnlyList<string> Roles);
