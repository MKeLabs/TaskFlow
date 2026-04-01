using System.ComponentModel.DataAnnotations;

namespace TaskFlow.API.Auth;

public sealed record RegisterRequest(
    [property:EmailAddress][property:Required] string Email,
    [property:Required][property:MinLength(8)] string Password);

public sealed record LoginRequest(
    [property:EmailAddress][property:Required] string Email,
    [property:Required] string Password);

public sealed record LoginResponse(string AccessToken, DateTimeOffset ExpiresAtUtc, string Email, IReadOnlyList<string> Roles);
