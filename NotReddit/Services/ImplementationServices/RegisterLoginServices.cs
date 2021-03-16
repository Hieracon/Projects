using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotReddit.Models;

namespace NotReddit.Services.ImplementationServices
{
    public class RegisterLoginServices
    {

        public UserManager<User> user_manager;
        public SignInManager<User> _signInManager;

        public RegisterLoginServices (UserManager<User> user_manager, SignInManager<User> _signInManager)
        {
            this.user_manager = user_manager;
            this._signInManager = _signInManager;
        }

        public async Task<IdentityResult> Register(User user, string Password)
        {

            var result = await user_manager.CreateAsync(user, Password);
            return result;
        }

        public async Task<SignInResult> Login(string Email, string Password, bool RememberMe)
        {

            var result = await _signInManager.PasswordSignInAsync(Email, Password, RememberMe, lockoutOnFailure: false);
            return result;
        }

    }
}

