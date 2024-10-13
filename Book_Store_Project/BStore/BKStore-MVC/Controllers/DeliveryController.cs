using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BKStore_MVC.Controllers
{
    public class DeliveryController : Controller
    {
        UserManager<ApplicationUser> UserManager;
        private readonly IDeliveryClientRepository deliveryClientRepository;

        public DeliveryController(UserManager<ApplicationUser> userManager,
            IDeliveryClientRepository deliveryClientRepository)
        {
            UserManager = userManager;
            this.deliveryClientRepository = deliveryClientRepository;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            List<DeliveryClients> deliverylst = deliveryClientRepository.GetAll();
            List<DeliveryVM> deliveryVM = new List<DeliveryVM>();

            // Retrieve the user by their ID
            var users = await UserManager.Users.ToListAsync();

            foreach (var item in deliverylst)
            {
                ApplicationUser u = users.FirstOrDefault(u => u.Id == item.UserID) ?? new ApplicationUser();

                DeliveryVM delivery = new DeliveryVM
                {
                    UserID = item.UserID,
                    FullName = item.FullName,
                    UserName = u.UserName,
                    NationalID = item.NationalID,
                    IsLocked = u.LockoutEnd.HasValue && u.LockoutEnd.Value > DateTimeOffset.Now,
                    Phone = u.PhoneNumber
                };

                deliveryVM.Add(delivery);
            }

            return View("GetAll", deliveryVM);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDelivery(string UserId)
        {
            //string userId = Request.Cookies["UserId"]??"";
            ApplicationUser appuser = await UserManager.FindByIdAsync(UserId);
            if (appuser != null)
            {
                DeliveryVM deliveryVM = new DeliveryVM();
                deliveryVM.Email = appuser.Email;
                deliveryVM.UserName = appuser.UserName;
                deliveryVM.UserID = UserId;
                return View("AddDelivery", deliveryVM);

            }


            return View("AddDelivery");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveAdd(DeliveryVM deliveryVM)
        {
            ApplicationUser appuser =
                   await UserManager.FindByIdAsync(deliveryVM.UserID);
            if (appuser != null)
            {
                appuser.Email = deliveryVM.Email;
                appuser.UserName = deliveryVM.UserName;
                appuser.PhoneNumber = deliveryVM.Phone;
                appuser.LockoutEnabled = true;
                await UserManager.UpdateAsync(appuser);
                DeliveryClients deliveryClients = new DeliveryClients();
                deliveryClients.FullName = deliveryVM.FullName;
                deliveryClients.NationalID = deliveryVM.NationalID;
                deliveryClients.IsLocked = true;
                deliveryClients.UserID = appuser.Id;
                deliveryClientRepository.Add(deliveryClients);
                deliveryClientRepository.Save();
                return RedirectToAction("Index", "Home");
            }
            return View(nameof(AddDelivery), deliveryVM);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LockAccount(string ID)
        {
            // Retrieve the user by their ID
            var user = await UserManager.FindByIdAsync(ID);
            if (user == null)
            {
                return NotFound(); // Handle the case where the user is not found
            }

            // Enable the lockout feature
            user.LockoutEnabled = true;

            // Optionally, set the LockoutEnd property to a future date if you want to lock the account for a specific duration
            user.LockoutEnd = DateTimeOffset.Now.AddDays(30);

            // Update the user in the database
            var result = await UserManager.UpdateAsync(user);
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
            var user = await UserManager.FindByIdAsync(ID);
            if (user == null)
            {
                return NotFound(); // Handle the case where the user is not found
            }

            // Enable the lockout feature
            user.LockoutEnabled = false;

            // Optionally, set the LockoutEnd property to a future date if you want to lock the account for a specific duration
            user.LockoutEnd = null;

            // Update the user in the database
            var result = await UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Handle the case where the update fails
                return BadRequest(result.Errors);
            }

            return RedirectToAction(nameof(GetAll));
        }

    }
}
