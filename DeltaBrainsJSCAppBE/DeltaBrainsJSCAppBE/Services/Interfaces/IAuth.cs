using BTBackendOnline2.Configurations;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;

namespace DeltaBrainsJSCAppBE.Services.Interfaces
{
    public interface IAuth
    {
        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ApiResponse<LoginRes> Login(LoginReq request);
    }
}
