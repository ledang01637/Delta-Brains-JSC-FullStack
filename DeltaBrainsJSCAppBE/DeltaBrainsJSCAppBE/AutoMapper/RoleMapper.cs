using AutoMapper;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;
using DeltaBrainsJSCAppBE.Models;

namespace DeltaBrainsJSCAppBE.AutoMapper
{
    public class RoleMapper : Profile
    {
        public RoleMapper() 
        {
            CreateMap<Role, RoleRes>();
            CreateMap<RoleReq, Role>();
        }
    }
}
