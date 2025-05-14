using DeltaBrainsJSCAppFE.Handel;
using DeltaBrainsJSCAppFE.Views;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace DeltaBrainsJSCAppFE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var authLogin = AuthStorage.LoadToken();

            if (authLogin != null && !string.IsNullOrEmpty(authLogin.Token) &&  AuthStorage.IsTokenValid(authLogin))
            {
               var window = CheckRole(authLogin.Token);

               window?.Show();
            }
            else
            {
                var loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
            }
        }

        private static Window? CheckRole(string token)
        {
            try
            {
                var role = GetRoleFromToken.GetRole(token);
                return role switch
                {
                    "admin" => new ManagerWindow(),
                    "employee" => new EmployeeWindow(),
                    _ => null
                };
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"Lỗi kết nối: {httpEx.Message}",
                               "Lỗi mạng", MessageBoxButton.OK, MessageBoxImage.Error);
               return null;
            }
            catch (SecurityTokenException tokenEx)
            {
                MessageBox.Show($"Lỗi xác thực: {tokenEx.Message}",
                               "Lỗi bảo mật", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định: {ex.Message}",
                               "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }

}
