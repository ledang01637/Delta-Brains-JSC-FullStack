using DeltaBrainsJSCAppFE.Handel;
using DeltaBrainsJSCAppFE.Models.Request;
using DeltaBrainsJSCAppFE.Models.Response;
using DeltaBrainsJSCAppFE.Views;
using Microsoft.IdentityModel.Tokens;
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

        public async Task LoginAsync(Window? parentWindow)
        {
            if (parentWindow == null)
            {
                MessageBox.Show("Lỗi hệ thống: Không xác định được cửa sổ cha.",
                              "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.",
                              "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsLoading = true;

            try
            {
                var loginResult = await AuthHandel.Login(Username, Password);

                if (!loginResult.IsSuccess || string.IsNullOrEmpty(loginResult.Data?.Token))
                {
                    MessageBox.Show($"{loginResult.Message}",
                                  "Đăng nhập thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var role = GetRoleFromToken.GetRole(loginResult.Data.Token);

                Window newWindow = role switch
                {
                    "admin" => new ManagerWindow(),
                    "employee" => new EmployeeWindow(),
                    _ => throw new NotImplementedException()
                };

                if (newWindow != null)
                {
                    newWindow.Show();
                    parentWindow.Close();

                    AuthStorage.SaveToken(loginResult.Data);
                }
                else
                {
                    MessageBox.Show("Tài khoản không có quyền truy cập phù hợp.",
                                   "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"Lỗi kết nối: {httpEx.Message}",
                               "Lỗi mạng", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SecurityTokenException tokenEx)
            {
                MessageBox.Show($"Lỗi xác thực: {tokenEx.Message}",
                               "Lỗi bảo mật", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định: {ex.Message}",
                               "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        
    }
}
