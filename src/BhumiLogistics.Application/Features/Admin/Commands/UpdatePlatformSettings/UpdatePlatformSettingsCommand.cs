using MediatR;

namespace BhumiLogistics.Application.Features.Admin.Commands.UpdatePlatformSettings;

public sealed record UpdatePlatformSettingsCommand(
    string BusinessName, string PanNumber, string? VatNumber, string RegisteredAddress,
    string ContactEmail, string ContactPhone,
    string GrievanceOfficerName, string GrievanceOfficerEmail, string GrievanceOfficerPhone) : IRequest;