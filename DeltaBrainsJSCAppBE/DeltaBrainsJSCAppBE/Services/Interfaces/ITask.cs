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
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResponse<TaskRes>> Update(TaskReq request, int id);


        /// <summary>
        /// Cập nhật Task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> Delete(int id);


        /// <summary>
        /// Cập nhật Task
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApiResponse<List<TaskRes>>> GetTasksByUserId(int userId);


    }

}
