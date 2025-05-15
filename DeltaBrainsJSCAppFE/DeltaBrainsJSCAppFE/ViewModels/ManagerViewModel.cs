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

        public ManagerViewModel()
        {

            LogoutCommand = new AsyncRelayCommand<object>(

                async (p) => await ExecuteLogout());

            if (AppMemory.Instance.CachedTasks?.Any() == true)
            {
                ListTask = new ObservableCollection<TaskItemViewModel>(AppMemory.Instance.CachedTasks);
            }

            Init();
        }

        private async void Init()
        {
            await GetTasks();
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
                    }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
