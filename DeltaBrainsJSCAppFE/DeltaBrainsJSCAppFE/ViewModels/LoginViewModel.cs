using DeltaBrainsJSCAppFE.Handel;
using DeltaBrainsJSCAppFE.Models.Request;
using DeltaBrainsJSCAppFE.Models.Response;
using DeltaBrainsJSCAppFE.Views;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DeltaBrainsJSCAppFE.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private bool _isLoading;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); LoginCommand.RaiseCanExecuteChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); LoginCommand.RaiseCanExecuteChanged(); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); LoginCommand.RaiseCanExecuteChanged(); }
        }

        public AsyncRelayCommand<object> LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand<object>(

                async (p) => await LoginAsync(p as Window),
                (p) => CanLogin());
        }

        private bool CanLogin()
        {
            return !IsLoading &&
                   !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password);
        }

        public async Task LoginAsync(Window? p)
        {
            if(p == null)
            {
                MessageBox.Show("Lỗi hệ thống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            IsLoading = true;

            try
            {
                var result = await AuthHandel.Login(Username, Password);

                if (!result.IsSuccess)
                {
                    MessageBox.Show("Sai tài khoản mật khẩu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var role = GetRoleFromToken(result.Data.Token);

                if (role == "admin")
                {
                    new ManagerWindow().Show();
                    p.Close();
                }
                else if (role == "employee")
                {
                    new EmployeeWindow().Show();
                    p.Close();
                }
                else
                {
                    MessageBox.Show("Tài khoản không có quyền truy cập.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đăng nhập: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public static string? GetRoleFromToken(string token)
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
