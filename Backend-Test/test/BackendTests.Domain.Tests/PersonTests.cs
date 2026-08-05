using System;
using BackendTest.Domain.Entities;
using Xunit;

namespace BackendTests.Domain.Tests;

public sealed class PersonTests
{
    [Fact]
    public void Constructor_WhenBirthYearIsInFuture_ThrowsArgumentOutOfRangeException()
    {
        var futureYear = DateTime.UtcNow.Year + 1;

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Person(1, "Jane", "Doe", futureYear));

        Assert.Equal("yearOfBirth", exception.ParamName);
    }
}
