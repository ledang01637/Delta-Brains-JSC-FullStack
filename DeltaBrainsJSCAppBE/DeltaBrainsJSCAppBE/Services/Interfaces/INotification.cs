using BTBackendOnline2.Configurations;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;
using DeltaBrainsJSCAppBE.Models;

namespace DeltaBrainsJSCAppBE.Services.Interfaces
{
    public interface INotification
    {
        /// <summary>
        /// Tạo thông báo
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ApiResponse<NotificationRes>> Create(NotificationReq request);
    }
}
