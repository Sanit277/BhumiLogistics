using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Entities;
using BhumiLogistics.Domain.Exceptions;
using MediatR;

namespace BhumiLogistics.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IApplicationDbContext _dbContext;

    public RegisterCommandHandler(
        IUserRepository userRepository, IPasswordHasher passwordHasher, IApplicationDbContext dbContext)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existing is not null)
            throw new DomainException($"An account with email '{request.Email}' already exists.");

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = new User(request.FullName, request.Email, request.PhoneNumber, request.Role, passwordHash);

        await _userRepository.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
