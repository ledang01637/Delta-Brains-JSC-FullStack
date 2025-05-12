using AutoMapper;
using BTBackendOnline2.Configurations;
using DeltaBrainJSC.DB;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;
using DeltaBrainsJSCAppBE.Handle;
using DeltaBrainsJSCAppBE.Models;
using DeltaBrainsJSCAppBE.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeltaBrainsJSCAppBE.Services.Implements
{
    public class UserService : IUser
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public UserService(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<UserRes>> Create(UserReq request)
        {
            try
            {
                var exist = _context.Users.FirstOrDefault(a => a.Email.Equals(request.Email));

                if (exist != null)
                    return ApiResponse<UserRes>.Fail("Email đã tồn tại");

                var user = _mapper.Map<User>(request);
                user.Password = CompareSHA256.ToSHA256(request.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await _context.Entry(user).Reference(u => u.Role).LoadAsync();

                var response = _mapper.Map<UserRes>(user);

                return ApiResponse<UserRes>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserRes>.Fail("Lỗi khi thêm: " + ex.Message);
            }

        }

        public Task<ApiResponse<List<UserRes>>> GetAll()
        {
            try
            {
                var users = _context.Users.Include(u => u.Role).ToList();
                var response = _mapper.Map<List<UserRes>>(users);

                return System.Threading.Tasks.Task.FromResult(ApiResponse<List<UserRes>>.Success(response));
            }
            catch (Exception ex)
            {
                return System.Threading.Tasks.Task.FromResult(ApiResponse<List<UserRes>>.Fail("Lỗi khi lấy dữ liệu: " + ex.Message));
            }
        }
    }
}
