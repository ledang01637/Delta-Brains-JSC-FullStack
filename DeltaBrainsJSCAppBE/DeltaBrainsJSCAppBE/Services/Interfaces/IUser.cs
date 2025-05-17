using BTBackendOnline2.Configurations;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;

namespace DeltaBrainsJSCAppBE.Services.Interfaces
{
    public interface IUser
    {
        /// <summary>
        /// Thêm mới User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<UserRes>> Create(UserReq request);


        /// <summary>
        /// Cập nhật User
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResponse<UserRes>> Updae(UserReq request, int id);

        /// <summary>
        /// Lấy danh sách User
        /// </summary
        /// <returns></returns>
        Task<ApiResponse<List<UserRes>>> GetAll();
    }
}
