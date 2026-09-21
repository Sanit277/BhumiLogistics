using BhumiLogistics.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.Platform.Queries.GetPlatformDisclosure;

/// <summary>Public-facing disclosure record. No sensitive data — this is meant to be visible to anyone.</summary>
public sealed record PlatformDisclosureDto(
    string BusinessName, string PanNumber, string? VatNumber, string RegisteredAddress,
    string ContactEmail, string ContactPhone,
    string GrievanceOfficerName, string GrievanceOfficerEmail, string GrievanceOfficerPhone);

public sealed record GetPlatformDisclosureQuery : IRequest<PlatformDisclosureDto>;

public class GetPlatformDisclosureQueryHandler : IRequestHandler<GetPlatformDisclosureQuery, PlatformDisclosureDto>
{
    private readonly IApplicationDbContext _dbContext;

    public GetPlatformDisclosureQueryHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<PlatformDisclosureDto> Handle(GetPlatformDisclosureQuery request, CancellationToken cancellationToken)
    {
        var settings = await _dbContext.PlatformSettings.FirstAsync(cancellationToken);

        return new PlatformDisclosureDto(
            settings.BusinessName, settings.PanNumber, settings.VatNumber, settings.RegisteredAddress,
            settings.ContactEmail, settings.ContactPhone,
            settings.GrievanceOfficerName, settings.GrievanceOfficerEmail, settings.GrievanceOfficerPhone);
    }
}