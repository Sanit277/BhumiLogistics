using BhumiLogistics.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.Admin.Commands.UpdatePlatformSettings;

public class UpdatePlatformSettingsCommandHandler : IRequestHandler<UpdatePlatformSettingsCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdatePlatformSettingsCommandHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task Handle(UpdatePlatformSettingsCommand request, CancellationToken cancellationToken)
    {
        var settings = await _dbContext.PlatformSettings.FirstAsync(cancellationToken);

        settings.Update(
            request.BusinessName, request.PanNumber, request.VatNumber, request.RegisteredAddress,
            request.ContactEmail, request.ContactPhone,
            request.GrievanceOfficerName, request.GrievanceOfficerEmail, request.GrievanceOfficerPhone);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}