namespace MySpot.Core.Exceptions;

public sealed class InvalidEmployeeNameException : CustomException
{
    public InvalidEmployeeNameException() : base("Name is empty")
    {
    }
}
