using DTOsB.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModelsB.Authentication_and_Authorization_B;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace B_Tech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUserB> _userManager;
        private readonly SignInManager<ApplicationUserB> _signInManager;
        //private readonly IUserService _userService;
        private readonly IConfiguration _configuration;



        public AccountController(UserManager<ApplicationUserB> userManager, SignInManager<ApplicationUserB> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_userService = userService;
            _configuration = configuration;

        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            return Ok("Welcome to the User API Controller.");
        }


        //[HttpPost("CheckNumber")]
        //public async Task<IActionResult> phoneCheck(CheckNumDTo model)
        //{
        //    var user = await _userManager(model.PhoneNumber);

        //    if (user == null)
        //    {
        //        return BadRequest("wrong Email");
        //    }
        //    if (user.PhoneNumber != null)
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Register");
        //    }
        //}

        [HttpPost("CheckNumber")]
        public async Task<IActionResult> PhoneCheck(CheckNumDTo model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);

            if (user != null)
            {
                return Ok(new { redirectTo = "sign-in", Message = "Phone number exists, please proceed to sign in." });
            }
            else
            {
                return Ok(new { redirectTo = "sign-up", Message = "Phone number does not exist, please proceed to sign up." });
            }
        }





        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(new { Message = "Wrong Email" });
            }

            var validPass = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!validPass)
            {
                return Unauthorized(new { Message = "Wrong Password" });
            }

            List<Claim> claims = new List<Claim>()
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, model.Email.Split('@')[0]),
        new Claim(ClaimTypes.Email, model.Email),
        new Claim("UserType", user.UserType) 
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["jwt:Issuer"],
                audience: _configuration["jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var TokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { Message = "Login successful", Token = TokenStr });
        }


        [HttpGet("GetCurrentUser")]
        [Authorize]

        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var userType = User.FindFirstValue("UserType");

            var userInfo = new
            {
                UserId = userId,
                UserName = userName,
                Email = email,
                UserType = userType
            };

            return Ok(userInfo);
        }


        [HttpGet("GetUserById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var userInfo = new
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                UserType = user.UserType
            };

            return Ok(userInfo);
        }




        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var Exist = await _userManager.FindByEmailAsync(model.Email);
            if (Exist == null)
            {
                var user = new ApplicationUserB
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    City = model.City,
                    Country = model.Country,
                    PostalCode = model.PostalCode,
                    UserType = model.UserType,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    model.UserType = "User";
                    await _userManager.AddToRoleAsync(user, "User");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(new { Message = "Registration successful", User = user });
                }
            }
            else
            {
                return StatusCode(500, "Already Exist");

            }

            return BadRequest(ModelState);
        }


        [HttpPost("signout")]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Successfully signed out" });
        }
    }
}
