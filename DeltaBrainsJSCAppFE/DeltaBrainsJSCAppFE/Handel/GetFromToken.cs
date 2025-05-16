using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeltaBrainsJSCAppFE.Handel
{
    public class GetFromToken
    {
        public static string? GetRole(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var roleClaim = jwtToken.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.Role || c.Type.Equals("role", StringComparison.OrdinalIgnoreCase));

                return roleClaim?.Value?.ToLower();
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError($"Lỗi không xác định: {ex.Message}");
                return null;
            }
        }
        public static int GetUserId()
        {
            try
            {
                var authLogin = AuthStorage.LoadToken();

                if (authLogin != null && !string.IsNullOrEmpty(authLogin.Token) && AuthStorage.IsTokenValid(authLogin))
                {

                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(authLogin.Token);

                    var value = jwtToken.Claims.FirstOrDefault(c => c.Type.Equals("Id"));

                    if (!string.IsNullOrEmpty(value?.Value?.ToLower()))
                    {
                        var userId = value?.Value?.ToLower();

                        if (string.IsNullOrEmpty(userId))
                        {
                            return 0;
                        }
                        return int.Parse(userId);
                    } 
                    else
                        return 0;
                }
                return 0;
                
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError($"Lỗi không xác định: {ex.Message}");
                return 0;
            }
        }


    }
}
