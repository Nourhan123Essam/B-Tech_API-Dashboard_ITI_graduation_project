using Microsoft.AspNetCore.Identity;
using ModelsB.Authentication_and_Authorization_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Contracts_B.User
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUserB>> GetAllUsersAsync();
        Task<ApplicationUserB> GetUserByIdAsync(string id);
        Task<ApplicationUserB> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(ApplicationUserB user);
        Task DeleteUserAsync(string id);
        Task<IdentityResult> CreateUserAsync(ApplicationUserB user, string password);
    }
}
