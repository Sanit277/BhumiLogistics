namespace BhumiLogistics.Domain.Exceptions;

public class PlotAlreadyLeasedException : DomainException
{
    public Guid PlotId { get; }

    public PlotAlreadyLeasedException(Guid plotId)
        : base($"Land plot '{plotId}' is already under an active lease and cannot accept new offers.")
    {
        PlotId = plotId;
    }
}
