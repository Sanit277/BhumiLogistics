using BhumiLogistics.Domain.Enums;
using MediatR;

namespace BhumiLogistics.Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand(
    string FullName, string Email, string PhoneNumber, string Password, UserRole Role) : IRequest<Guid>;
