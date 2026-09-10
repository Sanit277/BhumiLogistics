using BhumiLogistics.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.Admin.Queries.GetAllUsers;

public sealed record GetAllUsersQuery : IRequest<IReadOnlyList<AdminUserDto>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IReadOnlyList<AdminUserDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllUsersQueryHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<AdminUserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken) =>
        await _dbContext.Users
            .OrderByDescending(u => u.CreatedAtUtc)
            .Select(u => new AdminUserDto(u.Id, u.FullName, u.Email, u.PhoneNumber, u.Role, u.CreatedAtUtc))
            .ToListAsync(cancellationToken);
}