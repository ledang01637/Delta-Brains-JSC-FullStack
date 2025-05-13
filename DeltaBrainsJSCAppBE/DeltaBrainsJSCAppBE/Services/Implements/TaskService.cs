using AutoMapper;
using BTBackendOnline2.Configurations;
using DeltaBrainJSC.DB;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;
using DeltaBrainsJSCAppBE.Models;
using DeltaBrainsJSCAppBE.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = DeltaBrainsJSCAppBE.Models.Task;
using TaskStatus = DeltaBrainsJSCAppBE.Enum.TaskStatus;

namespace DeltaBrainsJSCAppBE.Services.Implements
{
    public class TaskService : ITask
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;

        public TaskService(DBContext context, IMapper mapper, ILogger<TaskService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ApiResponse<TaskRes>> Create(TaskReq request)
        {
            try
            {
                var lowerStatus = request.Status.ToLower();

                if (lowerStatus != "chưa thực hiện" &&
                    lowerStatus != "đang thực hiện" &&
                    lowerStatus != "hoàn thành")
                {
                    return ApiResponse<TaskRes>.Fail("Chỉ chấp nhận: chưa thực hiện, đang thực hiện, hoàn thành");
                }


                var task = _mapper.Map<Task>(request);

                task.Created = DateTime.UtcNow;
                task.Updated = DateTime.UtcNow;

                _context.Tasks.Add(task);

                await _context.SaveChangesAsync();

                

                var response = _mapper.Map<TaskRes>(task);


                return ApiResponse<TaskRes>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo task");
                return ApiResponse<TaskRes>.Error();
            }
        }


        public async Task<ApiResponse<List<TaskRes>>> GetAll()
        {
            try
            {
                var tasks = await _context.Tasks
                    .Include(t => t.User)
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



    }
}
