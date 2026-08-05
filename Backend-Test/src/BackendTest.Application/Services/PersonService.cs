using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Application.Exceptions;
using BackendTest.Application.Models;
using BackendTest.Application.Repositories;
using BackendTest.Domain.Entities;

namespace BackendTest.Application.Services
{
    public sealed class PersonService
    {
        private readonly IPersonRepository _repository;

        public PersonService(IPersonRepository repository) => _repository = repository;

        public async Task<IReadOnlyList<PersonData>> GetAllAsync(CancellationToken cancellationToken) =>
            (await _repository.GetAllAsync(cancellationToken)).Select(ToData).ToArray();

        public async Task<PersonData> GetByIdAsync(int id, CancellationToken cancellationToken) =>
            ToData(await FindByIdAsync(id, cancellationToken));

        public async Task<PersonData> AddAsync(PersonData request, CancellationToken cancellationToken)
        {
            var person = ToEntity(request);
            await _repository.AddAsync(person, cancellationToken);
            return ToData(person);
        }

        public async Task<PersonData> UpdateAsync(int routeId, PersonData request, CancellationToken cancellationToken)
        {
            EnsureMatchingIds(routeId, request.Id);
            var existingPerson = await FindByIdAsync(routeId, cancellationToken);
            var person = ToEntity(request);
            await _repository.ReplaceAsync(person, cancellationToken);
            return ToData(person);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken) =>
            await _repository.RemoveAsync(await FindByIdAsync(id, cancellationToken), cancellationToken);

        private async Task<Person> FindByIdAsync(int id, CancellationToken cancellationToken) =>
            await _repository.FindByIdAsync(id, cancellationToken)
            ?? throw new ResourceNotFoundException("Person", id);

        private static Person ToEntity(PersonData person) =>
            new(person.Id, person.Firstname, person.Lastname, person.YearOfBirth);

        private static PersonData ToData(Person person) =>
            new(person.Id, person.FirstName, person.LastName, person.YearOfBirth);

        private static void EnsureMatchingIds(int routeId, int entityId)
        {
            if (routeId != entityId)
            {
                throw new RequestConflictException("Route id does not match the person's id.");
            }
        }
    }
}
