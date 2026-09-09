using MediatR;

namespace BhumiLogistics.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<string>; // returns JWT
