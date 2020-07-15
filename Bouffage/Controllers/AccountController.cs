using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bouffage.Data;
using Bouffage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bouffage.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private IPasswordHasher<User> passwordHasher;
        private IPasswordValidator<User> passwordValidator;
        private IUserValidator<User> userValidator;
        private readonly BouffageContext _context;

        public AccountController(UserManager<User> userMgr, SignInManager<User> signinMgr, IPasswordHasher<User> passwordHash,
            IPasswordValidator<User> passwordVal, IUserValidator<User> userValid, BouffageContext context)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            passwordHasher = passwordHash;
            passwordValidator = passwordVal;
            userValidator = userValid;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            /*LoginViewModel login = new LoginViewModel();
            login.ReturnUrl = returnUrl;*/  //mislam deka e za wrong pass
            return View(returnUrl);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                User appUser = await userManager.FindByEmailAsync(email);
                if (appUser != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, password, false, false);
                    if (result.Succeeded)
                    {
                        if ((await userManager.IsInRoleAsync(appUser, "Admin")))
                        {
                            //return Redirect(login.ReturnUrl ?? "/");
                            return RedirectToAction("Index", "Admin", null);
                        }
                        if ((await userManager.IsInRoleAsync(appUser, "User")))
                        {
                            return RedirectToAction("Index", "Home"/*, new { id = appUser.TeacherId }*/);
                        }
                    }
                }
                ModelState.AddModelError(nameof(email), "Login Failed: Invalid Email or password");
            }
            return View(email);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", null);
        }

        [Authorize]
        public async Task<IActionResult> UserInfo()
        {
            User curruser = await userManager.GetUserAsync(User);
            string userDetails = curruser.Username;
            return View(curruser);
        }

             // CAN YOU NOT!?
        public IActionResult AccessDenied()
        {
            return View();
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}