using ApplicationB.Services_B.User;
using AutoMapper;
using DTOsB.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsB.Authentication_and_Authorization_B;

namespace DTOsB.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<ApplicationUserB> _userManager;
        private SignInManager<ApplicationUserB> _signInManager;
        private IMapper _mapper;
        private IUserService _userService;

        public AdminController(UserManager<ApplicationUserB> userManager, SignInManager<ApplicationUserB> signInManager, IMapper mapper, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userService = userService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or Password.");
                return View(model);
            }
            if (user.UserType == "Admin")
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "This account has been locked out due to multiple failed login attempts.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    return View();
                }
            }

            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Admin");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var UserDto = await _userService.GetAllAppUsersAsync();
            return View("users", UserDto);
        }

        public IActionResult LockUnlock(string? id)
        {
            return RedirectToAction();
        }

        public async Task<IActionResult> GetUserById(string id)
        {
            var userDto = await _userService.GetAppUserByIdAsync(id);

            if (userDto == null)
            {
                return NotFound();
            }
            return View("GetOne", userDto);
        }

        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return View(user); // Return user for view
        }

        //مربطهاش ب الservice بسبب مشاكل فى ال mapping 

        [HttpGet("{id}/edit")]
        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View("UpdateUser", user); // Return user for editing
        }

        //[HttpPost("{id}/edit")]
        //public async Task<IActionResult> UpdateUser(ApplicationUserB user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var existingUser = await _userManager.FindByIdAsync(user.Id);
        //        if (existingUser == null)
        //        {
        //            return NotFound();
        //        }

        //        existingUser.UserName = user.UserName;
        //        existingUser.Email = user.Email;
        //        existingUser.Address = user.Address;
        //        existingUser.PhoneNumber = user.PhoneNumber;
        //        existingUser.City = user.City;
        //        existingUser.Country = user.Country;
        //        existingUser.PostalCode = user.PostalCode;
        //        existingUser.UserType = user.UserType;

        //        //_mapper.Map(user, existingUser);


        //        var result = await _userManager.UpdateAsync(existingUser);
        //        if (result.Succeeded)
        //        {

        //            if (user.UserType == "Admin")
        //            {
        //                await _userManager.AddToRoleAsync(user, "Admin");
        //            }
        //            else
        //            {
        //                await _userManager.AddToRoleAsync(user, "User");
        //            }

        //            return RedirectToAction("GetAllUsers"); // Redirect to user list after update
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    return View(user);
        //}

        //public async Task<IActionResult> DeleteUser(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}

        //[HttpPost, ActionName("DeleteUser")]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user != null)
        //    {
        //        var result = await _userManager.DeleteAsync(user);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("GetAllUsers");
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }
        //    return RedirectToAction("GetAllUsers");
        //}

        [AllowAnonymous]
        public IActionResult CreateUser()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                //var user = new ApplicationUserB
                //{
                //    UserName = model.UserName,
                //    Email = model.Email,
                //    PhoneNumber = model.PhoneNumber,
                //    Address = model.Address,
                //    City = model.City,
                //    Country = model.Country,
                //    PostalCode = model.PostalCode,
                //    UserType = model.UserType, 
                //};
                var existingEmail = await _userManager.FindByEmailAsync(model.Email);
                if (existingEmail != null)
                {         
                    ModelState.AddModelError("Email", "The email address is already in use.");
                    return View(model); 
                }

                var user = _mapper.Map<ApplicationUserB>(model);

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.UserType == "Admin")
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("GetAllUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}
