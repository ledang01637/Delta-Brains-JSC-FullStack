using AutoMapper;
using BTBackendOnline2.Configurations;
using DeltaBrainJSC.DB;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;
using DeltaBrainsJSCAppBE.Hubs;
using DeltaBrainsJSCAppBE.Models;
using DeltaBrainsJSCAppBE.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Task = DeltaBrainsJSCAppBE.Models.Task;
using TaskStatus = DeltaBrainsJSCAppBE.Enum.TaskStatus;

namespace DeltaBrainsJSCAppBE.Services.Implements
{
    public class TaskService : ITask
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;
        private readonly INotification _notification;
        private readonly IHubContext<NotificationHub> _hubContext;


        public TaskService(DBContext context, IMapper mapper, ILogger<TaskService> logger, INotification notification, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _notification = notification;
            _hubContext = hubContext;
        }
        public async Task<ApiResponse<TaskRes>> Create(TaskReq request)
        {
            try
            {
                var task = _mapper.Map<Task>(request);
                task.Created = DateTime.UtcNow;
                task.Updated = DateTime.UtcNow;
                task.IsCurrent = true;

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                var notificationReq = new NotificationReq
                {
                    UserId = task.UserId,
                    Title = "Bạn có công việc mới được giao",
                    Message = $"Công việc '{task.Title}' đã được giao cho bạn.",
                    RelatedTaskId = task.Id
                };

                var notificationRes = await CreateNotification(notificationReq);

                if(notificationRes == null) 
                {
                    return ApiResponse<TaskRes>.Fail("Lỗi khi tạo và gửi thông báo");
                }

                await SenDataHubAsync(notificationRes);

                var response = _mapper.Map<TaskRes>(task);
                return ApiResponse<TaskRes>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo Task");
                return ApiResponse<TaskRes>.Error();
            }
        }

        private async Task<NotificationRes?> CreateNotification(NotificationReq notificationReq)
        {
            try
            {
                var notification = _mapper.Map<Notification>(notificationReq);
                notification.CreatedAt = DateTime.UtcNow;

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                return _mapper.Map<NotificationRes>(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo thông báo");
                return null;
            }
        }

        private async System.Threading.Tasks.Task SenDataHubAsync(NotificationRes notificationRes)
        {
            await _hubContext.Clients.All.SendAsync("SendTaskAssigned", notificationRes);
        }

        public async Task<ApiResponse<List<TaskRes>>> GetAll()
        {
            try
            {
                var tasks = await _context.Tasks
                        .Include(ut => ut.Assignee)
                        .Include(ut => ut.AssignedByUser)
                        .ToListAsync();

                if (!tasks.Any())
                {
                    return ApiResponse<List<TaskRes>>.NoData();
                }

                var response = _mapper.Map<List<TaskRes>>(tasks);

                return ApiResponse<List<TaskRes>>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách công việc");
                return ApiResponse<List<TaskRes>>.Error();
            }
        }

        public async Task<ApiResponse<List<TaskRes>>> GetTasksByUserId(int userId)
        {
            try
            {
                var tasks = await _context.Tasks
                    .Include(ut => ut.Assignee)
                    .Where(ut => ut.UserId == userId && ut.IsCurrent)
                    .ToListAsync();

                if (!tasks.Any())
                {
                    return ApiResponse<List<TaskRes>>.NoData();
                }

                var response = _mapper.Map<List<TaskRes>>(tasks);

                return ApiResponse<List<TaskRes>>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách công việc theo người dùng");
                return ApiResponse<List<TaskRes>>.Error();
            }
        }
        public async Task<ApiResponse<TaskRes>> Update(TaskReq request, int id)
        {
            try
            {
                var exist = await ExistTask(id);

                if (exist == null)
                {
                    return ApiResponse<TaskRes>.NotFound();
                }

                _mapper.Map(request, exist);

                exist.Updated = DateTime.UtcNow;
                exist.IsCurrent = true;

                _context.Tasks.Update(exist);
                await _context.SaveChangesAsync();

                var response = _mapper.Map<TaskRes>(exist);
                return ApiResponse<TaskRes>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật task");
                return ApiResponse<TaskRes>.Error();
            }
        }

        private async Task<DeltaBrainsJSCAppBE.Models.Task?> ExistTask(int id)
        {
            return await _context.Tasks
                .Include(ut => ut.Assignee)
                .Include(ut => ut.AssignedByUser)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

    }
}
