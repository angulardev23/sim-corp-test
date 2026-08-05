using System;

namespace BackendTest.Domain.Entities;

public sealed class Person
{
    public Person(int id, string firstName, string lastName, decimal yearOfBirth)
    {
        if (yearOfBirth > DateTime.UtcNow.Year)
        {
            throw new ArgumentOutOfRangeException(nameof(yearOfBirth), "Customer can not be born after current year");
        }

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        YearOfBirth = yearOfBirth;
    }

    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public decimal YearOfBirth { get; }
}
