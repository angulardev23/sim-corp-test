namespace BackendTest.Infrastructure.Persistence
{
    internal sealed class PersonRecord
    {
        internal PersonRecord(int id, string firstName, string lastName, decimal yearOfBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            YearOfBirth = yearOfBirth;
        }

        public int Id { get; private set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public decimal YearOfBirth { get; private set; }
    }
}
