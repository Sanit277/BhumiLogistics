using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BhumiLogistics.Infrastructure.Services;

/// <summary>
/// Wraps ASP.NET Core Identity's battle-tested PasswordHasher without
/// pulling in the full Identity membership system.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(string password) => _hasher.HashPassword(null!, password);

    public bool Verify(string password, string passwordHash) =>
        _hasher.VerifyHashedPassword(null!, passwordHash, password) != PasswordVerificationResult.Failed;
}
