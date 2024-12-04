using ApplicationB.Contracts_B.User;
using AutoMapper;
using DTOsB.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ModelsB.Authentication_and_Authorization_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.User
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUserB> _userManager;
        private readonly IMapper _mapper;

        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUserB> userManager, IMapper mapper,
            IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId != null ? userId : "38981d39-6f02-4c16-a231-292adf2e5637"; 
        }
        public async Task<IEnumerable<UserDto>> GetAllAppUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetAppUserByIdAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetAppUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return _mapper.Map<UserDto>(user);
        }

        public async Task UpdateAppUserAsync(UserDto userDto)
        {
            var user = _mapper.Map<ApplicationUserB>(userDto);
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteAppUserAsync(string id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<IdentityResult> CreateAppUserAsync(UserDto UserDto) // تنفيذ جديد
        {
            var user = _mapper.Map<ApplicationUserB>(UserDto);
            return await _userRepository.CreateUserAsync(user, UserDto.Password);
        }
    }
}
