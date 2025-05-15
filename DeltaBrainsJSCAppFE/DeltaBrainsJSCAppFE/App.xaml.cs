using DeltaBrainsJSCAppFE.Handel;
using DeltaBrainsJSCAppFE.Views;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;
using static System.Net.WebRequestMethods;

namespace DeltaBrainsJSCAppFE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private void Application_Startup(object sender, StartupEventArgs e)
        //{
        //    var mainWindow = new MainWindow();
        //    mainWindow.ShowDialog();
        //}
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var authLogin = AuthStorage.LoadToken();

            if (authLogin != null && !string.IsNullOrEmpty(authLogin.Token) && AuthStorage.IsTokenValid(authLogin))
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
                var role = GetFromToken.GetRole(token);
                return role switch
                {
                    "admin" => new ManagerWindow(),
                    "employee" => new EmployeeWindow(),
                    _ => null
                };
            }
            catch (HttpRequestException httpEx)
            {
               MessageBoxHelper.ShowError($"Lỗi kết nối: {httpEx.Message}");
               return null;
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError($"Lỗi không xác định: {ex.Message}");
                return null;
            }
        }
    }

}
