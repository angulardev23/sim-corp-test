using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Application.Repositories;
using BackendTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendTest.Infrastructure.Persistence
{
    public sealed class PersonRepository : IPersonRepository
    {
        private readonly BackendTestDbContext _context;

        public PersonRepository(BackendTestDbContext context) => _context = context;

        public async Task<IReadOnlyList<Person>> GetAllAsync(CancellationToken cancellationToken) =>
            await _context.People.AsNoTracking()
                .Select(person => new Person(person.Id, person.FirstName, person.LastName, person.YearOfBirth))
                .ToArrayAsync(cancellationToken);

        public async Task<Person?> FindByIdAsync(int id, CancellationToken cancellationToken)
        {
            var person = await _context.People.AsNoTracking()
                .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
            return person is null ? null : new Person(person.Id, person.FirstName, person.LastName, person.YearOfBirth);
        }

        public async Task AddAsync(Person person, CancellationToken cancellationToken)
        {
            _context.People.Add(ToRecord(person));
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task ReplaceAsync(Person person, CancellationToken cancellationToken)
        {
            _context.People.Update(ToRecord(person));
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(Person person, CancellationToken cancellationToken)
        {
            _context.People.Remove(ToRecord(person));
            await _context.SaveChangesAsync(cancellationToken);
        }

        private static PersonRecord ToRecord(Person person) =>
            new(person.Id, person.FirstName, person.LastName, person.YearOfBirth);
    }
}
