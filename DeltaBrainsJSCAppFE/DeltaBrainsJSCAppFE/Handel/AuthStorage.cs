using DeltaBrainsJSCAppFE.Models.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeltaBrainsJSCAppFE.Handel
{
    public class AuthStorage
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"appfe_token.json");

        //Lưu thông tin đăng nhập
        public static void SaveToken(LoginRes token)
        {
            var json = JsonSerializer.Serialize(token);
            File.WriteAllText(FilePath, json);
        }

        //Load thông tin
        public static LoginRes LoadToken()
        {
            if (!File.Exists(FilePath)) return null;

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<LoginRes>(json);
        }

        //Xóa file
        public static void ClearToken()
        {
            if (File.Exists(FilePath)) File.Delete(FilePath);
        }

        //Kiểm tra token hợp lệ
        public static bool IsTokenValid(LoginRes token)
        {
            return token != null && token.Expiration > DateTime.UtcNow;
        }

    }
}
