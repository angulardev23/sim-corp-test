using System.ComponentModel.DataAnnotations;
using BackendTest.Application.Models;

namespace BackendTest.Api.Contracts
{
    public sealed class PersonContract
    {
        public int Id { get; init; }

        [Required]
        public string Firstname { get; init; } = string.Empty;

        [Required]
        public string Lastname { get; init; } = string.Empty;

        public decimal YearOfBirth { get; init; }

        public PersonData ToApplicationModel() => new(Id, Firstname, Lastname, YearOfBirth);
    }
}
