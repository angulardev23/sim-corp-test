using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BackendTest.Domain.Entities;

namespace BackendTest.Application.Repositories
{
    public interface IPersonRepository
    {
        Task<IReadOnlyList<Person>> GetAllAsync(CancellationToken cancellationToken);
        Task<Person?> FindByIdAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(Person person, CancellationToken cancellationToken);
        Task ReplaceAsync(Person person, CancellationToken cancellationToken);
        Task RemoveAsync(Person person, CancellationToken cancellationToken);
    }
}
