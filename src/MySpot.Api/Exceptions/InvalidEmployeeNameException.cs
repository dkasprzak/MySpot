﻿namespace MySpot.Api.Exceptions;

public sealed class InvalidEmployeeNameException : CustomException
{
    public InvalidEmployeeNameException() : base("Name is empty")
    {
    }
}
