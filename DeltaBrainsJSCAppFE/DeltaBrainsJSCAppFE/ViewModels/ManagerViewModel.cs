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
using Microsoft.Extensions.DependencyInjection;
using Windows.System;
using Windows.UI;
using Microsoft.Extensions.Logging;

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
        public AsyncRelayCommand<object> DeleteCommand { get; }

        public ManagerViewModel()
        {
            ListTask = new ObservableCollection<TaskItemViewModel>();
            ManagerCommand = new AsyncRelayCommand<object>((p) => Init());
            AddNewCommand = new AsyncRelayCommand<object>((p) => AddNew());
            EdiCommand = new AsyncRelayCommand<object>((p) => Edit(p as TaskItemViewModel));
            DeleteCommand = new AsyncRelayCommand<object>((p) => Delete(p as TaskItemViewModel));
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

        //Lấy toàn bộ danh sách công việc
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

                    var loginWindow = App.ServiceProvider.GetRequiredService<LoginWindow>();
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


        //Thêm mới công việc
        public async Task AddNew()
        {
            CallBackTaskViewModel();
        }

        //Chỉnh sửa công việc
        public async Task Edit(TaskItemViewModel taskItem)
        {
            if (taskItem?.Task == null)
            {
                MessageBoxHelper.ShowError("Dữ liệu task không hợp lệ");
                return;
            }

            CallBackTaskViewModel(taskItem.Task);
        }

        //CallBack từ TaskViewModel
        public void CallBackTaskViewModel(TaskRes taskRes = default)
        {
            try
            {
                var taskWindow = new TaskWindow(taskRes);

                if (taskWindow.DataContext is TaskViewModel vm)
                {
                    // Dùng weak event để tránh rò rỉ bộ nhớ
                    WeakEventManager<TaskViewModel, EventArgs>.AddHandler(
                        vm,
                        nameof(vm.TaskSaved),
                        async (s, args) =>
                        {
                            await GetTasks();
                        });
                }

                taskWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError($"Lỗi khi mở cửa sổ task: {ex.Message}");
            }
        }

        public async Task Delete(TaskItemViewModel taskItem)
        {
            try
            {
                if (taskItem?.Task == null)
                {
                    MessageBoxHelper.ShowError("Dữ liệu task không hợp lệ");
                    return;
                }

                var result = MessageBoxHelper.ShowQuestion("Bạn có chắc chắn muốn xóa task này?");

                if (result == MessageBoxResult.Yes)
                {

                    string url = "https://localhost:7089/api/Task/delete-task";


                    var response = await _httpClient.PostAsJsonAsync(url, taskItem?.Task.Id);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBoxHelper.ShowError("Xóa thất bại");
                        return;
                    }

                    MessageBoxHelper.ShowInfo("Xóa thành công");
                    await GetTasks();

                }
            }
            catch(Exception ex)
            {
                MessageBoxHelper.ShowError("Lỗi không xác định: " + ex.Message);
                return;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
