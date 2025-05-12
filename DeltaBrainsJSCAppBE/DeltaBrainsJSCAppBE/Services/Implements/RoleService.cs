using AutoMapper;
using BTBackendOnline2.Configurations;
using DeltaBrainJSC.DB;
using DeltaBrainsJSCAppBE.DTOs;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;
using DeltaBrainsJSCAppBE.Models;
using DeltaBrainsJSCAppBE.Services.Interfaces;

namespace DeltaBrainsJSCAppBE.Services.Implements
{
    public class RoleService : IRole
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public RoleService(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResponse<RoleRes>> Create(RoleReq request)
        {
            try
            {
                var exist = _context.Roles.FirstOrDefault(a => a.Name.ToLower().Equals(request.Name));

                if (exist != null)
                    return ApiResponse<RoleRes>.Fail("Tên quyền đã tồn tại");

                var role = _mapper.Map<Role>(request);

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                var response = _mapper.Map<RoleRes>(role);

                return ApiResponse<RoleRes>.Success(response);
            }
            catch(Exception ex)
            {
                return ApiResponse<RoleRes>.Fail("Lỗi khi thêm: " + ex.Message);
            }

        }

        public Task<ApiResponse<List<RoleRes>>> GetAll()
        {
            try
            {
                var roles = _context.Roles.ToList();
                var response = _mapper.Map<List<RoleRes>>(roles);

                return System.Threading.Tasks.Task.FromResult(ApiResponse<List<RoleRes>>.Success(response));
            }
            catch (Exception ex)
            {
                return System.Threading.Tasks.Task.FromResult(ApiResponse<List<RoleRes>>.Fail("Lỗi khi lấy dữ liệu: " + ex.Message));
            }
        }
    }
}
