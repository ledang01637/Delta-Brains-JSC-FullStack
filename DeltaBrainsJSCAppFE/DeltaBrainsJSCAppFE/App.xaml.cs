using DeltaBrainsJSCAppFE.Handel;
using DeltaBrainsJSCAppFE.ViewModels;
using DeltaBrainsJSCAppFE.Views;
using System.Net.Http;
using System.Windows;
using CommunityToolkit.WinUI.Notifications;
using DeltaBrainsJSCAppFE.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace DeltaBrainsJSCAppFE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Khởi tạo DI
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            Application_Startup(this, e);
        }

        //Cấu hình DI
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ManagerWindow>();
            services.AddTransient<ManagerViewModel>();

            services.AddTransient<EmployeeWindow>();
            services.AddTransient<EmployeeViewModel>();

            services.AddTransient<LoginWindow>();
            services.AddTransient<LoginViewModel>();
        }

        //Khi load kiểm tra token có lưu lại trên local ko nếu không hiện form đăng nhập
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
                var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
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
                    "admin" => ServiceProvider.GetRequiredService<ManagerWindow>(),
                    "employee" => ServiceProvider.GetRequiredService<EmployeeWindow>(),
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
