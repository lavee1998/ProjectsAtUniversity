using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.DTOs;

namespace TodoList2.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // api/Account/Login
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            if (_signInManager.IsSignedIn(User))
                await _signInManager.SignOutAsync();

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, isPersistent: false,
                    lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Ok();
                }

                ModelState.AddModelError("", "Bejelentkezés sikertelen!");
                return Unauthorized();
            }

            return Unauthorized();
        }

        [HttpPost("Signout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}