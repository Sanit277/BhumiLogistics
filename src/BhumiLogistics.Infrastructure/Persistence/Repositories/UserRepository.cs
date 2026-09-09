using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) => _context = context;

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken) =>
        await _context.Users.AddAsync(user, cancellationToken);
}
