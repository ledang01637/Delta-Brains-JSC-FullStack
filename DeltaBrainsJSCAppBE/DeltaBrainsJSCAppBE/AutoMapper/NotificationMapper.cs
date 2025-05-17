using AutoMapper;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;
using DeltaBrainsJSCAppBE.Models;

namespace DeltaBrainsJSCAppBE.AutoMapper
{
    public class NotificationMapper : Profile
    {
        public NotificationMapper()
        {
            CreateMap<NotificationReq, Notification>();
            CreateMap<Notification, NotificationRes>();

        }
    }
}
