using BhumiLogistics.Domain.Common;
using BhumiLogistics.Domain.Enums;

namespace BhumiLogistics.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public string PasswordHash { get; private set; } = string.Empty;

    private User() { } // EF Core

    public User(string fullName, string email, string phoneNumber, UserRole role, string passwordHash)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        Role = role;
        PasswordHash = passwordHash;
    }
}
