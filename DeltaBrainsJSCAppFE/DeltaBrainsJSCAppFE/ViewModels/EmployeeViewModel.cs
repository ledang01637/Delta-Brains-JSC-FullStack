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
using System.Windows.Markup;
using Microsoft.AspNetCore.SignalR.Client;

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
            await GetTasks();
        }

        public async Task ConnectSignalRAsync()
        {
            try
            {

                _connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:7089/hubs/task")
                    .WithAutomaticReconnect()
                    .Build();

                _connection.Closed += async (error) =>
                {
                    Console.WriteLine("Connection closed");
                    await Task.Delay(2000);
                    await _connection.StartAsync();
                };

                _connection.Reconnected += (connectionId) =>
                {
                    Console.WriteLine("Reconnected: " + connectionId);
                    return Task.CompletedTask;
                };

                _connection.Reconnecting += (error) =>
                {
                    Console.WriteLine("Reconnecting...");
                    return Task.CompletedTask;
                };

                SetupHubEvents();

                await _connection.StartAsync();

                Console.WriteLine("Kết nối SignalR thành công: " + _connection.State);

            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError($"Lỗi không xác định {ex.Message}");
            }

        }

        private void SetupHubEvents()
        {
            _connection.On<NotificationRes>("SendTaskAssigned", async (data) =>
            {
                Notification = data;
                await GetTasks();
            });
        }

        public async Task GetTasks()
        {
            try
            {
                IsLoading = true;

                var userId = GetFromToken.GetUserId();

                if(userId == 0)
                {
                    MessageBoxHelper.ShowError("Vui lòng đăng nhập lại");

                    var loginWindow = new LoginWindow();
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

                    if(data == null || data.Data == null)
                    {
                        MessageBoxHelper.ShowError("Lỗi lấy dữ liệu");
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

        public Task ExecuteLogout()
        {
            try
            {

                var result = MessageBoxHelper.ShowQuestion("Bạn có muốn đăng xuất?");

                if (result == MessageBoxResult.Yes)
                {
                    AppMemory.Instance.CachedTasks = null;
                    AuthStorage.ClearToken();

                    var loginWindow = new LoginWindow();
                    loginWindow.Show();

                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window != loginWindow)
                        {
                            window.Close();
                        }
                    }


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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
