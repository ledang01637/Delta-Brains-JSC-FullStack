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
    public class GetRoleFromToken
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
                MessageBox.Show($"Lỗi giải mã token: {ex.Message}", "Lỗi Token", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
