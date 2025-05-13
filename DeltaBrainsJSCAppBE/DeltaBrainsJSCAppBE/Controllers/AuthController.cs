using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DeltaBrainsJSCAppBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;

        public AuthController(IAuth authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginReq request)
        {
            var result = _authService.Login(request);
            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var result = _authService.Logout();
            return Ok(result);
        }


    }
}
