using BKStore_MVC.ViewModel;
using BKStore_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BKStore_MVC.ViewModels;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IDeliveryClientRepository deliveryClientRepository;

        public RoleManager<IdentityRole> RoleManager { get; }

        public AccountController(UserManager<ApplicationUser> userManager,IDeliveryClientRepository deliveryClientRepository,
              SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            RoleManager = roleManager;
            this.deliveryClientRepository = deliveryClientRepository;
        }
        public IActionResult Register()
        {
            return View("Register");
        }
        public async Task<IActionResult> SaveRegister(RegisterBS viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = viewModel.UserName,
                    PasswordHash = viewModel.Password,
                    Email = viewModel.Email,
                    ImagePath = "blank-profile-picture.png",
                    LockoutEnabled = false, // Explicitly set LockoutEnabled to false
                    LockoutEnd = null
                };

                IdentityResult result = await userManager.CreateAsync(applicationUser, viewModel.Password);
                if (result.Succeeded)
                {
                    applicationUser.LockoutEnabled = false;
                    await userManager.UpdateAsync(applicationUser);

                    if (userManager.Users.Count() < 2)
                        await userManager.AddToRoleAsync(applicationUser, "Admin");
                    await signInManager.SignInAsync(applicationUser, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Register", viewModel);
        }

        public IActionResult Login()
        {
            return View("Login");
        }
        public async Task<IActionResult> SaveLogin(LoginBS loginBS)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appuser =
                    await userManager.FindByNameAsync(loginBS.UserName);

                if (appuser != null)
                {
                    if (appuser.LockoutEnabled == false)
                    {
                        bool found =
                        await userManager.CheckPasswordAsync(appuser, loginBS.Password);
                        if (found == true)
                        {
                            await signInManager.SignInAsync(appuser, loginBS.RememberMe);
                            return RedirectToAction("Index", "Book");
                        }
                    }
                    else
                    {
                        DeliveryClients delivery = deliveryClientRepository.GetByUserID(appuser.Id);
                        if (await userManager.IsInRoleAsync(appuser, "Delivery") & delivery==null)
                        {
                            return RedirectToAction("AddDelivery", "Delivery", new { UserId = appuser.Id });
                        }
        
                    }
                }
                ModelState.AddModelError("", "Username OR Password Wrong");
            }
            return View("Login", loginBS);
        }
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return View(nameof(Login));
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddAdmin()
        {
            var lstrole = RoleManager.Roles.ToList();
            List<string?> roles = lstrole.Select(x => x.Name).ToList();
            ViewData["RoleList"] = roles;
            return View("AddAdmin");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveAdmin(RegistersRoles viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = viewModel.UserName,
                    Email = viewModel.Email,
                    LockoutEnabled = viewModel.Role != "Delivery"
                };

                IdentityResult result = await userManager.CreateAsync(applicationUser, viewModel.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(applicationUser, viewModel.Role);

                    // Explicitly update the LockoutEnabled property if necessary
                    if (viewModel.Role == "Delivery")
                    {
                        applicationUser.LockoutEnabled = false;
                        await userManager.UpdateAsync(applicationUser);
                    }

                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Register", viewModel);
        }


    }
}
