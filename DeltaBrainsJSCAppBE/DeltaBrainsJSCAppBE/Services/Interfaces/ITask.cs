using BTBackendOnline2.Configurations;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;

namespace DeltaBrainsJSCAppBE.Services.Interfaces
{
    public interface ITask
    {
        /// <summary>
        /// Thêm mới Task
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<TaskRes>> Create(TaskReq request);

        /// <summary>
        /// Lấy danh sách Task
        /// </summary
        /// <returns></returns>
        Task<ApiResponse<List<TaskRes>>> GetAll();
    }
}
