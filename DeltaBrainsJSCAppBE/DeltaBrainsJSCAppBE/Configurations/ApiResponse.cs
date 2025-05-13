using System.Text.Json.Serialization;

namespace BTBackendOnline2.Configurations
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T Data { get; set; }

        public ApiResponse(int code, bool isSuccess, string message, T data = default)
        {
            Code = code;
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> Success(T data, string message = "Thành công")
        {
            return new ApiResponse<T>(200, true, message, data);
        }

        public static ApiResponse<T> NoData(string message = "Không có dữ liệu")
        {
            return new ApiResponse<T>(204, true, message);
        }

        public static ApiResponse<T> Fail(string message, int code = 400)
        {
            return new ApiResponse<T>(code, false, message);
        }

        public static ApiResponse<T> NotFound(string message = "Không tìm thấy dữ liệu")
        {
            return new ApiResponse<T>(404, false, message);
        }

        public static ApiResponse<T> Error(string message = "Lỗi hệ thống")
        {
            return new ApiResponse<T>(500, false, message);
        }
    }
}