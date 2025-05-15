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

        [HttpPost("update-task")]
        public async Task<IActionResult> AssignTask([FromBody] TaskUpdate request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");

            var response = await _service.Update(request);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        //[HttpPost("assign")]
        //public async Task<IActionResult> AssignTask(TaskAssignmentDto dto)
        //{
        //    // Lưu dữ liệu vào DB...

        //    // Gửi sự kiện realtime tới người dùng được gán
        //    await _hubContext.Clients.User(dto.AssignedUserId)
        //        .SendAsync("TaskAssigned", new
        //        {
        //            TaskId = dto.TaskId,
        //            Message = "Bạn được gán một công việc mới!"
        //        });

        //    return Ok();
        //}
    }
}
