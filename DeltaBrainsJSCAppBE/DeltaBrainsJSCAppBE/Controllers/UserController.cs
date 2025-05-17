using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeltaBrainsJSCAppBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _service;
        public UserController(IUser service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddNew([FromBody] UserReq request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var response = await _service.Create(request);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> GetList()
        {
            var response = await _service.GetAll();

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> Update(UserReq update, int id)
        {
            var response = await _service.Updae(update, id);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
