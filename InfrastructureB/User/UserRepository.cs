using ApplicationB.Contracts_B.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModelsB.Authentication_and_Authorization_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureB.User
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUserB> _userManager;

        public UserRepository(UserManager<ApplicationUserB> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUserB>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUserB> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<ApplicationUserB> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task UpdateUserAsync(ApplicationUserB user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUserB user, string password) // تنفيذ جديد
        {
            return await _userManager.CreateAsync(user, password);
        }
    }
}
