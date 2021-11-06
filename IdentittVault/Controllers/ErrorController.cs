using IdentittVault.System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentittVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        public IActionResult Error()
        {
            IExceptionHandlerFeature context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (context.Error is IdentittVaultException exception)
            {
                return Problem(
                    detail: exception.Message, 
                    statusCode: exception.StatusCode);
            }

            return Problem();
        }
    }
}
