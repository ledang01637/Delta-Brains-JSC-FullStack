using DeltaBrainsJSCAppFE.Handel;
using DeltaBrainsJSCAppFE.Models.Response;
using DeltaBrainsJSCAppFE.Views;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DeltaBrainsJSCAppFE.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        private static readonly HttpClient _httpClient = new();
        private HubConnection _connection;

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }


        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; }
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

        public ICommand MainWindowCommand { get; }


        public MainViewModel()
        {
            MainWindowCommand = new RelayCommand<Window>((p) => true, (p) => Init());


            if (AppMemory.Instance.CachedTasks?.Any() == true)
            {
                ListTask = new ObservableCollection<TaskItemViewModel>(AppMemory.Instance.CachedTasks);
            }
        }

        private async void Init()
        {
            await ConnectSignalRAsync();
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
            _ = _connection.On<NotificationRes>("SendTaskAssigned", async (data) =>
            {
                Notification = data;
                Message = Notification.Message;
                await GetTasks();
            });
        }


        public async Task GetTasks()
        {
            try
            {
                IsLoading = true;

                string url = "https://localhost:7089/api/Task/get-list";

                ListTask = new ObservableCollection<TaskItemViewModel>();

                var response = await _httpClient.GetFromJsonAsync<ApiResponse<ObservableCollection<TaskRes>>>(url);

                if (response != null && response.Code == 200)
                {
                    foreach (var item in response.Data)
                    {
                        var vm = new TaskItemViewModel(item);
                        ListTask.Add(vm);
                        Debug.WriteLine(vm);
                    }
                }
                else
                {
                    MessageBoxHelper.ShowError(response?.Message ?? "Lỗi server");
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError($"Lỗi: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
