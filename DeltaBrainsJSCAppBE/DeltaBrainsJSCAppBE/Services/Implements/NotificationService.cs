using AutoMapper;
using BTBackendOnline2.Configurations;
using DeltaBrainJSC.DB;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;
using DeltaBrainsJSCAppBE.Handle;
using DeltaBrainsJSCAppBE.Hubs;
using DeltaBrainsJSCAppBE.Models;
using DeltaBrainsJSCAppBE.Services.Interfaces;
using System;

namespace DeltaBrainsJSCAppBE.Services.Implements
{
    public class NotificationService : INotification
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public NotificationService(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<NotificationRes>> Create(NotificationReq request)
        {
            try
            {
                var notification = _mapper.Map<Notification>(request);

                notification.CreatedAt = DateTime.Now;
                _context.Notifications.Add(notification);

                await _context.SaveChangesAsync();

                var response = _mapper.Map<NotificationRes>(notification);



                return ApiResponse<NotificationRes>.Success(response);
            }
            catch
            {
                return ApiResponse<NotificationRes>.Error();
            }

        }

    }
}
