using AutoMapper;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;
using DeltaBrainsJSCAppBE.Models;

namespace DeltaBrainsJSCAppBE.AutoMapper
{
    public class UserMapper : Profile
    {
        public UserMapper() 
        {
            CreateMap<User, UserRes>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));
            CreateMap<UserReq, User>();
        }
    }
}
