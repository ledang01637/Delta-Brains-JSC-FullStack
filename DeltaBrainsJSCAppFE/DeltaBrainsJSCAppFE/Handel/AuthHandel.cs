using DeltaBrainsJSCAppFE.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DeltaBrainsJSCAppFE.Handel
{
    public class AuthHandel
    {
        private static readonly HttpClient _httpClient = new();

        public static async Task<ApiResponse<LoginRes>> Login(string username, string password)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7089/api/Auth/login", new
                {
                    Username = username,
                    Password = password
                });

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<LoginRes> { IsSuccess = false, Message = "Lỗi đăng nhập" };
                }

                var content = await response.Content.ReadFromJsonAsync<ApiResponse<LoginRes>>();

                return new ApiResponse<LoginRes> { IsSuccess = true, Data = content.Data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<LoginRes> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
