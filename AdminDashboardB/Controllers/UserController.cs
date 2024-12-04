using ApplicationB.Services_B.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsB.Authentication_and_Authorization_B;

namespace DTOsB.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUserB> _userManager;
        private readonly SignInManager<ApplicationUserB> _signInManager;
        private readonly IUserService _userService;


        public UserController(UserManager<ApplicationUserB> userManager, SignInManager<ApplicationUserB> signInManager, IUserService userservice)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userservice;
        }
        public IActionResult Index()
        {
            return View();
        }




        //[HttpPost]
        //public async Task<IActionResult> Edit( )
        //{


        //    return RedirectToAction("Index");

        //}


        //public IActionResult Register()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterVM model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUserB
        //        {
        //            UserName = model.UserName,
        //            Email = model.Email,
        //            PhoneNumber = model.PhoneNumber,
        //            Address = model.Address,
        //            City = model.City,
        //            Country = model.Country,
        //            PostalCode = model.PostalCode,
        //            UserType = model.UserType, // This captures Admin or User from the form
        //        };

        //        var result = await _userManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            model.UserType = "User";
        //            await _userManager.AddToRoleAsync(user, "User");


        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("Login", "Admin"); // Redirect to homepage after successful registration
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    return View(model); // Re-display the form if there are validation errors
        //}


        //public async Task<IActionResult> SignOut()
        //{
        //    await _signInManager.SignOutAsync(); // Sign the user out

        //    return RedirectToAction("Login", "Admin"); // Redirect to homepage after signing out
        //}
    }
}
