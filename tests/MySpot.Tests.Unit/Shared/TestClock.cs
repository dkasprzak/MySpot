﻿using MySpot.Application.Services;

namespace MySpot.Tests.Unit.Shared;

public class TestClock : IClock
{
    public DateTime Current() => new(2023, 03, 06);

}
