using IdentittVault.Entities;
using IdentittVault.Models;
using IdentittVault.Repositories;
using IdentittVault.Services;
using IdentittVault.System.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IdentittVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class UsersController : CrudController<UserModel, User>
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService) : base(userService)
        {
            this._userService = userService;
        }

        protected override void OnCreate(UserModel model)
        {
            PasswordResult passwordResult = _userService.ValidatePasswordStrength(model.Password);
            
            if (!passwordResult.Success)
            {
                foreach (var error in passwordResult.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }
        }

        
    }
}
