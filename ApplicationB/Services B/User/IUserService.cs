using DTOsB.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.User
{
    public interface IUserService
    {
       public string GetCurrentUserId();
        Task<IEnumerable<UserDto>> GetAllAppUsersAsync();
        Task<UserDto> GetAppUserByIdAsync(string id);
        Task<UserDto> GetAppUserByEmailAsync(string email);
        Task UpdateAppUserAsync(UserDto userDto);
        Task DeleteAppUserAsync(string id);

        Task<IdentityResult> CreateAppUserAsync(UserDto UserDto); // إضافة جديدة
    }
}
