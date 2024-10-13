using BKStore_MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BKStore_MVC.ViewModel;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using BKStore_MVC.Repository.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace BKStore_MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOrderRepository orderRepository;
        private readonly ICustomerRepository customerRepository;
        public UserController(UserManager<ApplicationUser> userManager,ICustomerRepository customerRepository,
            IOrderRepository orderRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            this.orderRepository = orderRepository;
            this.customerRepository = customerRepository;
        }
        [Authorize]
        public async Task<IActionResult> MyAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            AccountUser myAccount = new AccountUser()
            {
                Name = user.UserName ?? "",
                Email = user.Email,
                ImagePath = user.ImagePath,
                Phone = user.PhoneNumber,
            };
            return View("MyAccount", myAccount);
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img");

                // Ensure the uploads folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                user.ImagePath = uniqueFileName;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("MyAccount");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please select a valid image file.");
            }

            return RedirectToAction("MyAccount");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.ToListAsync(); // Materialize the users list first
            List<UsersInfoVM> UsersInfo = new List<UsersInfoVM>();

            foreach (var user in users)
            {
                var userInfo = new UsersInfoVM
                {
                    UserID = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.Now,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
                };
                UsersInfo.Add(userInfo);
            }

            return View("GetAll", UsersInfo);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LockAccount(string ID)
        {
            // Retrieve the user by their ID
            var user = await _userManager.FindByIdAsync(ID);
            if (user == null)
            {
                return NotFound(); // Handle the case where the user is not found
            }

            // Enable the lockout feature
            user.LockoutEnabled = true;

            // Optionally, set the LockoutEnd property to a future date if you want to lock the account for a specific duration
            user.LockoutEnd = DateTimeOffset.Now.AddDays(30);

            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Handle the case where the update fails
                return BadRequest(result.Errors);
            }

            return RedirectToAction(nameof(GetAll));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnLockAccount(string ID)
        {
            // Retrieve the user by their ID
            var user = await _userManager.FindByIdAsync(ID);
            if (user == null)
            {
                return NotFound(); // Handle the case where the user is not found
            }

            // Enable the lockout feature
            user.LockoutEnabled = false;

            // Optionally, set the LockoutEnd property to a future date if you want to lock the account for a specific duration
            user.LockoutEnd = null;

            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Handle the case where the update fails
                return BadRequest(result.Errors);
            }

            return RedirectToAction(nameof(GetAll));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Detailed(string ID)
        {
            var users = await _userManager.Users.ToListAsync(); // Materialize the users list first
            var user = await _userManager.FindByIdAsync(ID);
            if (user == null)
            {
                return NotFound(); // Handle the case where the user is not found
            }
            DetaledUserInfoVM UsersInfo = new DetaledUserInfoVM()
            {
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                IsLocked = user.LockoutEnabled
            };
            Customer customer = customerRepository.GetByUserID(ID);
            if (customer != null) {
                Order order = orderRepository.GetByCustomerID(customer.ID);
                UsersInfo.CustomerID = customer.ID;
                UsersInfo.FullName = customer.Name;
                UsersInfo.Address = customer.Address;
                UsersInfo.OrderID = order.OrderId;
                UsersInfo.ImagePath = user.ImagePath;
            }
            return View("GetAll", UsersInfo);
        }

    }
}