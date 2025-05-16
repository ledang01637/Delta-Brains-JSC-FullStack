using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.Hubs;
using DeltaBrainsJSCAppBE.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DeltaBrainsJSCAppBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITask _service;
        private readonly IHubContext<NotificationHub> _hubContext;
        public TaskController(ITask service, IHubContext<NotificationHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddNew([FromBody] TaskReq request)
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
        [HttpPost("get-task-by-userId")]
        public async Task<IActionResult> GetTasksByUserId([FromBody] int userId)
        {
            var response = await _service.GetTasksByUserId(userId);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("update-task/{id}")]
        public async Task<IActionResult> Update(int id ,[FromBody] TaskUpdate request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var response = await _service.Update(request, id);

             return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
