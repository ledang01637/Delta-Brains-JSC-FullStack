using BTBackendOnline2.Configurations;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;

namespace DeltaBrainsJSCAppBE.Services.Interfaces
{
    public interface IRole
    {
        /// <summary>
        /// Thêm mới Role
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<RoleRes>> Create(RoleReq request);

        /// <summary>
        /// Lấy danh sách Role
        /// </summary
        /// <returns></returns>
        Task<ApiResponse<List<RoleRes>>> GetAll();
    }
}
