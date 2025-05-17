using DeltaBrainsJSCAppFE.Models.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Net.Http.Json;
using DeltaBrainsJSCAppFE.Handel;
using System.Diagnostics;
using DeltaBrainsJSCAppFE.Views;
using DeltaBrainsJSCAppFE.Notification;
using System.Windows.Markup;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace DeltaBrainsJSCAppFE.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        private static readonly HttpClient _httpClient = new();
        private HubConnection _connection;


        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private NotificationRes _notification;

        public NotificationRes Notification
        {
            get => _notification;
            set
            {
                _notification = value;
                OnPropertyChanged(nameof(Notification));
            }
        }

        private ObservableCollection<TaskItemViewModel> _listTask;

        public ObservableCollection<TaskItemViewModel> ListTask
        {
            get => _listTask;
            set
            {
                _listTask = value;
                OnPropertyChanged(nameof(ListTask));
            }
        }

        public AsyncRelayCommand<object> LogoutCommand { get; }
        public AsyncRelayCommand<object> EmployeeCommand { get; }

        public EmployeeViewModel()
        {
            EmployeeCommand = new AsyncRelayCommand<object>((p) => Init());

            ListTask = new ObservableCollection<TaskItemViewModel>();

            LogoutCommand = new AsyncRelayCommand<object>(

                async (p) => await ExecuteLogout());

            if (AppMemory.Instance.CachedTasks?.Any() == true)
            {
                ListTask = new ObservableCollection<TaskItemViewModel>(AppMemory.Instance.CachedTasks);
            }
        }

        private async Task Init()
        {
            await ConnectSignalRAsync();
            await GetTasks();
        }

        //Khởi tạo SignalR
        private async Task ConnectSignalRAsync()
        {
            try
            {
                var authLogin = AuthStorage.LoadToken();

                if (authLogin == null || string.IsNullOrEmpty(authLogin.Token) || !AuthStorage.IsTokenValid(authLogin))
                {
                    MessageBoxHelper.ShowError("Lỗi xác thực, vui lòng đăng nhập lại");
                    RedirectToLogin();
                    return;
                }

                _connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:7089/hubs/task")
                    .WithAutomaticReconnect()
                    .Build();

                RegisterSignalREvents();
                SetupHubEvents();

                await StartConnectionAsync();
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError($"Lỗi khi thiết lập kết nối SignalR: {ex.Message}");
            }
        }

        //Đăng ký các sự kiện SignalR
        private void RegisterSignalREvents()
        {
            _connection.Closed += async (error) =>
            {
                MessageBoxHelper.ShowWarning("Mất kết nối, đang thử lại...");
                await Task.Delay(3000);
                await StartConnectionAsync();
            };

            _connection.Reconnecting += (error) =>
            {
                MessageBoxHelper.ShowWarning("Đang kết nối lại...");
                return Task.CompletedTask;
            };

            _connection.Reconnected += (connectionId) =>
            {
                MessageBoxHelper.ShowInfo("Đã kết nối lại với server.");
                return Task.CompletedTask;
            };
        }

        //Lăng nghe sự kiện SignalR
        private void SetupHubEvents()
        {
            _connection.On<string>("TaskUpdate", async (data) =>
            {
                if (SendToastNotification.IsAppInBackground())
                {
                    SendToastNotification.SendNotification();
                }
                await GetTasks();
            });

            _connection.On<NotificationRes>("SendTaskAssigned", async (data) =>
            {
                if (SendToastNotification.IsAppInBackground())
                {
                    SendToastNotification.SendNotification(data);
                }
                await GetTasks();
            });
        }

        //Khỏi tạo kết nối
        private async Task StartConnectionAsync()
        {
            try
            {
                if (_connection.State != HubConnectionState.Connected)
                {
                    await _connection.StartAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError($"Lỗi khi kết nối lại: {ex.Message}");
                await Task.Delay(5000);
                await StartConnectionAsync();
            }
        }


        //Lấy danh sách công việc theo UserId
        public async Task GetTasks()
        {
            try
            {
                IsLoading = true;

                ListTask.Clear();

                var userId = GetFromToken.GetUserId();

                if (userId == 0)
                {
                    MessageBoxHelper.ShowError("Vui lòng đăng nhập lại");

                    var loginWindow = App.ServiceProvider.GetRequiredService<LoginWindow>();
                    loginWindow.Show();

                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window != loginWindow)
                        {
                            window.Close();
                        }
                    }

                    return;
                }

                string url = "https://localhost:7089/api/Task/get-task-by-userId";


                var response = await _httpClient.PostAsJsonAsync(url, userId);

                if (response.IsSuccessStatusCode)
                {

                    var data = await response.Content.ReadFromJsonAsync<ApiResponse<ObservableCollection<TaskRes>>>();

                    if (data == null || data.Data == null)
                    {
                        MessageBoxHelper.ShowError("Lỗi lấy dữ liệu nhân viên");
                        return;
                    }

                    if (!data.Data.Any())
                    {
                        MessageBoxHelper.ShowInfo("Bạn hiện không có công việc nào");
                        return;
                    }

                    foreach (var item in data.Data)
                    {
                        var vm = new TaskItemViewModel(item);
                        ListTask.Add(vm);
                    }
                }
                else
                {
                    MessageBoxHelper.ShowError("Lỗi server");
                }
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

        //Logout
        public Task ExecuteLogout()
        {
            try
            {
                if (MessageBoxHelper.ShowQuestion("Bạn có muốn đăng xuất?") == MessageBoxResult.Yes)
                {
                    AppMemory.Instance.CachedTasks = null;
                    AuthStorage.ClearToken();
                    RedirectToLogin();
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

            return Task.CompletedTask;
        }

        //Hiện login
        private static void RedirectToLogin()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var loginWindow = App.ServiceProvider.GetRequiredService<LoginWindow>();
                loginWindow.Show();

                foreach (Window window in Application.Current.Windows)
                {
                    if (window != loginWindow)
                    {
                        window.Close();
                    }
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
