using DeltaBrainsJSCAppFE.Handel;
using DeltaBrainsJSCAppFE.Models.Request;
using DeltaBrainsJSCAppFE.Models.Response;
using DeltaBrainsJSCAppFE.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel;
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
            set 
            { 
                _username = value; 
                OnPropertyChanged(); 
                LoginCommand.RaiseCanExecuteChanged(); 
            }
        }

        public string Password
        {
            get => _password;
            set 
            { 
                _password = value; 
                OnPropertyChanged(); 
                LoginCommand.RaiseCanExecuteChanged(); 
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set 
            { 
                _isLoading = value; 
                OnPropertyChanged();
                LoginCommand.RaiseCanExecuteChanged(); 
            }
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
                MessageBoxHelper.ShowWarning("Lỗi hệ thống: Không xác định được cửa sổ.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBoxHelper.ShowWarning("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.");
                return;
            }

            IsLoading = true;

            try
            {
                //Xác thực đăng nhập
                var loginResult = await AuthHandel.Login(Username, Password);

                if (!loginResult.IsSuccess || string.IsNullOrEmpty(loginResult.Data?.Token))
                {
                    MessageBoxHelper.ShowError($"{loginResult.Message}");
                    return;
                }


                //Lấy quyền người dùng
                var role = GetFromToken.GetRole(loginResult.Data.Token);

                Window newWindow = role switch
                {
                    "admin" => App.ServiceProvider.GetRequiredService<ManagerWindow>(),
                    "employee" => App.ServiceProvider.GetRequiredService<EmployeeWindow>(),
                    _ => throw new NotImplementedException()
                };

                //Lưu lại token
                AuthStorage.SaveToken(loginResult.Data);

                if (newWindow != null)
                {
                    newWindow.Show();
                    parentWindow.Close();
                }
                else
                {
                    MessageBoxHelper.ShowWarning("Tài khoản không có quyền truy cập phù hợp.");
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBoxHelper.ShowError($"Lỗi kết nối: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError($"Lỗi không xác định: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
