using DeltaBrainsJSCAppFE.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                    return new ApiResponse<LoginRes> { IsSuccess = false, Message = StatusAPI(response.StatusCode) };
                }

                var content = await response.Content.ReadFromJsonAsync<ApiResponse<LoginRes>>();
                if (content == null)
                {
                    return new ApiResponse<LoginRes> { IsSuccess = false, Message = "Không nhận được dữ liệu phản hồi từ server." };
                }


                return new ApiResponse<LoginRes> { IsSuccess = true, Data = content.Data };
            }
            catch (Exception ex)
            {
                return new ApiResponse<LoginRes> { IsSuccess = false, Message = ex.Message };
            }
        }
        private static string StatusAPI(HttpStatusCode httpStatusCode)
        {
            switch (httpStatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return "Không được phép truy cập (401)";
                case HttpStatusCode.BadRequest:
                    return "Yêu cầu không hợp lệ (400)";
                case HttpStatusCode.InternalServerError:
                    return "Lỗi máy chủ (500)";
                default:
                    return $"Lỗi không xác định: {(int)httpStatusCode}";
            }
        }

    }
}
