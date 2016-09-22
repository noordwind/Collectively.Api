using System;
using System.Threading.Tasks;
using Coolector.Core.Domain.Users;

namespace Coolector.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByNameAsync(string name);
        Task AddAsync(User user);
    }
}