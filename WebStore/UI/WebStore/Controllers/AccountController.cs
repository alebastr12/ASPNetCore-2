using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entitys;
using WebStore.Domain.Models;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        //private string _returnUrl;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            string returnUrl = null;
            string url = Request.Headers["Referer"].ToString();
            string site = Request.Headers["Host"];
            int index = url.IndexOf(site);
            if (index>-1)
                returnUrl = url.Substring(index + site.Count());
            
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl);
            }
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var loginResult = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (!loginResult.Succeeded)
            {
                ModelState.AddModelError("", "Имя пользователя или пароль неверны");
                return View(model);
            }
            if (Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var newUser = new User()
            {
                UserName= model.UserName,
                Email= model.Email,
                PhoneNumber= model.PhoneNumber
            };
            var createResult = await userManager.CreateAsync(newUser, model.Password);
            if (!createResult.Succeeded)
            {
                ModelState.AddModelError("", createResult.Errors.ElementAt(0).ToString());
                return View(model);
            }
            await signInManager.SignInAsync(newUser, false);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}