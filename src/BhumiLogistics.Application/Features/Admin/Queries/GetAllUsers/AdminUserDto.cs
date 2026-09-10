using BhumiLogistics.Domain.Enums;

namespace BhumiLogistics.Application.Features.Admin.Queries.GetAllUsers;

public sealed record AdminUserDto(
    Guid Id,
    string FullName,
    string Email,
    string PhoneNumber,
    UserRole Role,
    DateTimeOffset CreatedAtUtc);