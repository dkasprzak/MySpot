using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects;

public record EmployeeName
{
    public string Value { get; set; }

    public EmployeeName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidEmployeeNameException();
        }
        Value = value;
    }

    public static implicit operator string(EmployeeName name) => name.Value;
    public static implicit operator EmployeeName(string name) => new(name);
}
