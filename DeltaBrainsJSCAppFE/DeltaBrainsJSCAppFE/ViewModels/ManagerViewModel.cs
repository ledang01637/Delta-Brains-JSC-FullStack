using DeltaBrainsJSCAppFE.Views;
using System;
using System.Windows.Input;
using System.Windows;
using DeltaBrainsJSCAppFE.Models.Response;
using System.Net.Http;
using System.Net.Http.Json;
using System.ComponentModel;
using System.Collections.ObjectModel;
using DeltaBrainsJSCAppFE.Handel;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using static System.Net.WebRequestMethods;
using DeltaBrainsJSCAppFE.Models;
using DeltaBrainsJSCAppFE.Models.Request;
using System.Diagnostics;

namespace DeltaBrainsJSCAppFE.ViewModels
{
    public class ManagerViewModel : BaseViewModel
    {
        private static readonly HttpClient _httpClient = new();

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
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
        public AsyncRelayCommand<object> ManagerCommand { get; }
        public AsyncRelayCommand<object> AddNewCommand { get; }
        public AsyncRelayCommand<object> EdiCommand { get; }

        public ManagerViewModel()
        {
            ListTask = new ObservableCollection<TaskItemViewModel>();
            ManagerCommand = new AsyncRelayCommand<object>((p) => Init());
            AddNewCommand = new AsyncRelayCommand<object>((p) => AddNew());
            EdiCommand = new AsyncRelayCommand<object>((p) => Edit(p as TaskItemViewModel));
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

        public async Task GetTasks()
        {
            try
            {
                IsLoading = true;
                ListTask.Clear();

                string url = "https://localhost:7089/api/Task/get-list";

                var response = await _httpClient.GetFromJsonAsync<ApiResponse<ObservableCollection<TaskRes>>>(url);

                if (response != null && response.Code == 200)
                {
                    foreach (var item in response.Data)
                        ListTask.Add(new TaskItemViewModel(item));
                }
                else
                {
                    MessageBoxHelper.ShowError("Lỗi server");
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError(ex.Message);
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
        public async Task AddNew()
        {
            var taskWindow = new TaskWindow();

            if (taskWindow.DataContext is TaskViewModel vm)
            {
                EventHandler? handler = null;
                handler = async (s, args) =>
                {
                    vm.TaskSaved -= handler; 
                    await GetTasks();
                };

                vm.TaskSaved += handler;
                taskWindow.ShowDialog();
            }
            else
            {
                MessageBoxHelper.ShowError("Lỗi khởi tạo view model");
                taskWindow.Close();
            }
        }

        public async Task Edit(TaskItemViewModel taskItem)
        {
            if (taskItem?.Task == null)
            {
                MessageBoxHelper.ShowError("Dữ liệu task không hợp lệ");
                return;
            }

            var taskWindow = new TaskWindow(taskItem.Task);

            if (taskWindow.DataContext is TaskViewModel vm)
            {
                EventHandler? handler = null;
                handler = async (s, args) =>
                {
                    vm.TaskSaved -= handler;
                    await GetTasks();
                };

                vm.TaskSaved += handler;
            }

            taskWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
