namespace BhumiLogistics.Domain.Exceptions;

public class InvalidLandAreaException : DomainException
{
    public InvalidLandAreaException(string message) : base(message) { }
}
