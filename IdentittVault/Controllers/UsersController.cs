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
        private readonly AccountService _accountService;
        private readonly IdentittVaultSecure _secure;

        public UsersController(IUserService userService, AccountService accountService, IdentittVaultSecure secure) : base(userService)
        {
            this._userService = userService;
            this._accountService = accountService;
            this._secure = secure;
        }

        protected override void OnCreate(UserModel model)
        {
            PasswordResult passwordResult = _secure.ValidatePasswordStrength(model.Password);

            if (!passwordResult.Success)
            {
                foreach (var error in passwordResult.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }
        }

        /// <summary>
        /// API for create an entity
        /// </summary>
        /// <param name="model">A non-null entity data</param>
        /// <returns>The created entity with it's id</returns>
        [HttpPost("{id}/accounts")]
        public async Task<IActionResult> CreateAccount([FromRoute] Guid id, [FromBody] AccountModel model)
        {
            PasswordResult passwordResult = _secure.ValidatePasswordStrength(model.Password);

            if (passwordResult.Success)
            {
                model.UserId = id;
                Account entity = model.ToEntity();
                await _accountService.CreateAsync(entity);
                model = entity.ToModel();

                return CreatedAtAction(nameof(ReadAccount), new { id = model.UserId, accountId = model.Id }, model);
            }

            return BadRequest();
        }

        /// <summary>
        /// API for create an entity
        /// </summary>
        /// <param name="model">A non-null entity data</param>
        /// <returns>The created entity with it's id</returns>
        [HttpGet("{id}/accounts/{accountId}")]
        public async Task<ActionResult<AccountModel>> ReadAccount([FromRoute] Guid id, [FromRoute] Guid accountId)
        {
            //If not found, throws an exception NotFound and returns 404.
            Account account = await _accountService.ReadAsync(accountId);

            if (account.UserId.Equals(id))
            {
                AccountModel model = account.ToModel(true);
                return Ok(model);
            }
            else
            {
                ModelState.AddModelError("UserId", "Invalid UserId.");
            }

            return BadRequest();
        }

    }
}
