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


        /// <summary>
        /// Cập nhật Task
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<TaskRes>> Update(TaskUpdate request);

        /// <summary>
        /// Lấy công việc của một 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        Task<ApiResponse<TaskRes>> GetTasksByUserId(int userId);


    }

}
